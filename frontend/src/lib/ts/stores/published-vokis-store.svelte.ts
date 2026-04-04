import { ApiVokisCatalog, RJO } from "$lib/ts/backend-communication/backend-services";
import { ErrCodes, type Err } from "$lib/ts/err";
import type { PublishedVokiBriefInfo, PublishedVokiViewState } from "$lib/ts/voki";
import type { ResponseResult } from "../backend-communication/result-types";

export namespace PublishedVokisStore {

    type StateObj = {
        state: "loading" | "ok" | "errs";
        data?: PublishedVokiBriefInfo;
        errs?: Err[];
        vokiId: string;
    };

    type CacheEntry = { obj: StateObj; expiresAt: number };

    const TTL_MS = 5 * 60 * 1000;
    const SOFT_ERR_TTL_MS = 5_000;
    const MAX_BATCH = 50;
    const MAX_CACHE_SIZE = 500;

    const cache = new Map<string, CacheEntry>();
    const pendingIds = new Set<string>();
    let timer: ReturnType<typeof setTimeout> | undefined;

    export function Get(id: string): PublishedVokiViewState {
        const now = Date.now();
        const entry = cache.get(id);

        if (entry && entry.expiresAt > now) {
            return entry.obj as PublishedVokiViewState;
        }

        return GetWithForceRefresh(id);
    }

    export function GetWithForceRefresh(id: string): PublishedVokiViewState {
        const entry = ensureCacheEntry(id);
        setLoading(entry);
        enqueue(id);
        return entry.obj as PublishedVokiViewState;
    }

    export function Invalidate(id: string) {
        const entry = ensureCacheEntry(id);
        setLoading(entry);
        entry.expiresAt = 0;
        enqueue(id);
    }

    export function Clear() {
        cache.clear();
        pendingIds.clear();
        timer = undefined;
    }
    export function FetchOne(id: string): Promise<ResponseResult<PublishedVokiBriefInfo>> {
        const entry = ensureCacheEntry(id);
        setLoading(entry);
        return ApiVokisCatalog.fetchJsonResponse<{
            vokis: PublishedVokiBriefInfo[];
        }>("/vokis/brief-info", RJO.POST({ ids: [id] })).then(response => {
            if (response.isSuccess && response.data.vokis.length > 0) {
                const data = response.data.vokis[0];
                updateOk(entry, data);
                return { isSuccess: true, data };
            }
            updateErr(entry, [{ message: "Could not load Voki data" }], SOFT_ERR_TTL_MS);
            if (response.isSuccess) {
                return { isSuccess: false, errs: [{ message: "Voki not found", code: ErrCodes.NotFound.Voki }] };
            }
            return response;
        })
    }

    function enqueue(id: string) {
        pendingIds.add(id);
        if (!timer) {
            timer = setTimeout(flushBatch, 0);
        }
    }

    async function flushBatch() {
        const idsAll = Array.from(pendingIds);
        pendingIds.clear();
        timer = undefined;

        if (idsAll.length === 0) return;

        for (let i = 0; i < idsAll.length; i += MAX_BATCH) {
            await fetchAndApplyChunk(idsAll.slice(i, i + MAX_BATCH));
        }
    }

    async function fetchAndApplyChunk(ids: string[]) {
        try {
            const response = await ApiVokisCatalog.fetchJsonResponse<{
                vokis: PublishedVokiBriefInfo[];
            }>("/vokis/brief-info", RJO.POST({ ids }));

            if (response.isSuccess) {
                const dict: Record<string, PublishedVokiBriefInfo> =
                    Object.fromEntries(response.data.vokis.map(v => [v.id, v]));

                for (const id of ids) {
                    const entry = cache.get(id) ?? ensureCacheEntry(id);
                    const data = dict[id];

                    if (data) {
                        updateOk(entry, data);
                    } else {
                        updateErr(entry, [
                            { message: "Published Voki not found", code: 23011 } as Err
                        ], TTL_MS);
                    }
                }
            } else {
                handleFailure(ids, response.errs as Err[]);
            }

        } catch {
            handleNetworkError(ids);
        }
    }


    function ensureCacheEntry(id: string): CacheEntry {
        const existing = cache.get(id);
        if (existing) {
            return existing;
        }

        if (cache.size >= MAX_CACHE_SIZE) {
            let oldestKey: string | undefined;
            let oldestTs = Infinity;

            for (const [key, entry] of cache.entries()) {
                if (entry.expiresAt < oldestTs) {
                    oldestTs = entry.expiresAt;
                    oldestKey = key;
                }
            }
            if (oldestKey !== undefined) {
                cache.delete(oldestKey);
            }
        }

        const obj = $state<StateObj>({ state: "loading", vokiId: id });
        const created: CacheEntry = { obj, expiresAt: 0 };
        cache.set(id, created);
        return created;
    }

    function setLoading(entry: CacheEntry) {
        entry.obj.state = "loading";
        delete entry.obj.data;
        delete entry.obj.errs;
    }

    function updateOk(entry: CacheEntry, data: PublishedVokiBriefInfo) {
        entry.obj.state = "ok";
        entry.obj.data = data;
        delete entry.obj.errs;
        entry.expiresAt = Date.now() + TTL_MS;
    }

    function updateErr(entry: CacheEntry, errs: Err[], ttl: number) {
        entry.obj.state = "errs";
        entry.obj.errs = errs;
        delete entry.obj.data;
        entry.expiresAt = Date.now() + ttl;
    }

    function handleFailure(ids: string[], errs: Err[]) {
        for (const id of ids) {
            const entry = cache.get(id) ?? ensureCacheEntry(id);
            updateErr(entry, errs, SOFT_ERR_TTL_MS);
        }
    }

    function handleNetworkError(ids: string[]) {
        const netErr: Err = { message: "Network error", code: 0 } as Err;
        for (const id of ids) {
            const entry = cache.get(id) ?? ensureCacheEntry(id);
            updateErr(entry, [netErr], SOFT_ERR_TTL_MS);
        }
    }
}

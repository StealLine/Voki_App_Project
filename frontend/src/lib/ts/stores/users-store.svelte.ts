import { ApiUserProfiles, RJO } from "../backend-communication/backend-services";
import type { Err } from "../err";
import type { UserProfilePreview } from "../users";


export namespace UsersStore {
    export type UserProfilePreviewWithState =
        | { state: "loading" }
        | { state: "ok"; data: UserProfilePreview }
        | { state: "errs"; errs: Err[] };

    type StateObj = { state: "loading" | "ok" | "errs"; data?: UserProfilePreview; errs?: Err[] };
    type CacheEntry = { obj: StateObj; expiresAt: number };

    const TTL_MS = 10 * 60 * 1000;
    const SOFT_ERR_TTL_MS = 5_000;
    const MAX_BATCH = 100;
    const MAX_CACHE_SIZE = 500;


    const cache = new Map<string, CacheEntry>();
    const pendingIds = new Set<string>();
    let timer: ReturnType<typeof setTimeout> | undefined;


    export function Get(id: string): UserProfilePreviewWithState {
        const now = Date.now();
        const cached = cache.get(id);
        if (cached && cached.expiresAt > now) {
            return cached.obj as UserProfilePreviewWithState;
        }

        return GetWithForceRefresh(id);
    }

    export function GetWithForceRefresh(id: string): UserProfilePreviewWithState {
        const entry = ensureCacheEntry(id);
        setLoading(entry);
        enqueue(id);
        return entry.obj as UserProfilePreviewWithState;
    }

    export function Invalidate(id: string): void {
        const entry = ensureCacheEntry(id);
        entry.obj.state = "loading";
        delete entry.obj.data;
        delete entry.obj.errs;
        entry.expiresAt = 0;
        enqueue(id);
    }


    function enqueue(id: string): void {
        pendingIds.add(id);
        if (!timer) {
            timer = setTimeout(flushBatch, 0);
        }
    }

    async function flushBatch(): Promise<void> {
        const idsAll = Array.from(pendingIds);
        pendingIds.clear();
        timer = undefined;

        if (idsAll.length === 0) {
            return;
        }

        for (let i = 0; i < idsAll.length; i += MAX_BATCH) {
            const chunk = idsAll.slice(i, i + MAX_BATCH);
            await fetchAndApplyChunk(chunk);
        }
    }

    async function fetchAndApplyChunk(ids: string[]): Promise<void> {

        try {
            const response = await ApiUserProfiles.fetchJsonResponse<{ users: Record<string, UserProfilePreview> }>(
                "/users/preview", RJO.POST({ userIds: ids })
            );


            if (response.isSuccess) {
                for (const id of ids) {
                    const entry = cache.get(id) ?? ensureCacheEntry(id);
                    const userData = response.data.users[id];

                    if (userData) {
                        updateOk(entry, userData);
                    } else {
                        updateErr(entry, [{ message: "User not found", code: 23001 } as Err], TTL_MS);
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
            let oldestExpiresAt = Infinity;
            for (const [key, entry] of cache.entries()) {
                if (entry.expiresAt < oldestExpiresAt) {
                    oldestExpiresAt = entry.expiresAt;
                    oldestKey = key;
                }
            }
            if (oldestKey !== undefined) {
                cache.delete(oldestKey);
            }
        }

        const obj = $state<StateObj>({ state: "loading" });
        const fresh: CacheEntry = { obj, expiresAt: 0 };
        cache.set(id, fresh);
        return fresh;
    }


    function setLoading(entry: CacheEntry): void {
        entry.obj.state = "loading";
        delete entry.obj.data;
        delete entry.obj.errs;
    }

    function updateOk(entry: CacheEntry, data: UserProfilePreview): void {
        entry.obj.state = "ok";
        entry.obj.data = data;
        delete entry.obj.errs;
        entry.expiresAt = Date.now() + TTL_MS;
    }

    function updateErr(entry: CacheEntry, errs: Err[], ttl: number): void {
        entry.obj.state = "errs";
        entry.obj.errs = errs;
        delete entry.obj.data;
        entry.expiresAt = Date.now() + ttl;
    }

    function handleFailure(ids: string[], errs: Err[]): void {
        for (const id of ids) {
            const entry = cache.get(id) ?? ensureCacheEntry(id);
            updateErr(entry, errs, SOFT_ERR_TTL_MS);
        }
    }

    function handleNetworkError(ids: string[]): void {
        const netErr: Err = { message: "Network error", code: 0 } as Err;
        for (const id of ids) {
            const entry = cache.get(id) ?? ensureCacheEntry(id);
            updateErr(entry, [netErr], SOFT_ERR_TTL_MS);
        }
    }
}

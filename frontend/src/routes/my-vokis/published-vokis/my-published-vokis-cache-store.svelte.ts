import { ApiVokisCatalog, RJO } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import type { PublishedVokiBriefInfo, PublishedVokiViewState } from "$lib/ts/voki";
import { SvelteMap } from "svelte/reactivity";



type CacheEntry = { entry: PublishedVokiViewState; expiresAt: number };

const TTL_MS = 5 * 60 * 1000; // 5 min
const SOFT_ERR_TTL_MS = 5000;
const MAX_BATCH = 50;

export namespace MyPublishedVokisCacheStore {
    const cache = new SvelteMap<string, CacheEntry>();
    const pendingIds = new Set<string>();
    let timer: ReturnType<typeof setTimeout> | undefined;

    export function Get(id: string): PublishedVokiViewState {
        const now = Date.now();
        const cached = cache.get(id);

        if (cached && cached.expiresAt > now) {
            return cached.entry;
        }

        return GetWithForceRefresh(id);
    }

    export function GetWithForceRefresh(id: string): PublishedVokiViewState {
        enqueue(id);
        return { state: "loading", vokiId: id };
    }

    export function Invalidate(id: string): void {
        cache.delete(id);
    }
    export function Clear(): void {
        cache.clear();
        pendingIds.clear();
        timer = undefined;
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

        if (idsAll.length === 0) return;

        for (let i = 0; i < idsAll.length; i += MAX_BATCH) {
            const chunk = idsAll.slice(i, i + MAX_BATCH);
            await fetchAndApplyChunk(chunk);
        }
    }

    async function fetchAndApplyChunk(ids: string[]): Promise<void> {
        try {
            const response = await ApiVokisCatalog.fetchJsonResponse<{
                vokis: PublishedVokiBriefInfo[];
            }>("/vokis/brief-info", RJO.POST({ ids }));

            if (response.isSuccess) {
                const vokisDict: Record<string, PublishedVokiBriefInfo> = Object.fromEntries(
                    response.data.vokis.map(v => [v.id, v])
                );

                for (const id of ids) {
                    const vokiData = vokisDict[id];
                    if (vokiData) {
                        cache.set(id, {
                            entry: { state: "ok", data: vokiData },
                            expiresAt: Date.now() + TTL_MS
                        });
                    } else {
                        cache.set(id, {
                            entry: {
                                state: "errs",
                                errs: [{ message: "Published Voki not found", code: 23011 }],
                                vokiId: id
                            },
                            expiresAt: Date.now() + TTL_MS
                        });
                    }
                }
            } else {
                handleFailure(ids, response.errs as Err[]);
            }
        } catch {
            handleNetworkError(ids);
        }
    }

    function updateErr(id: string, errs: Err[], ttl: number): void {
        cache.set(id, {
            entry: { state: "errs", errs, vokiId: id },
            expiresAt: Date.now() + ttl
        });
    }

    function handleFailure(ids: string[], errs: Err[]): void {
        for (const id of ids) {
            updateErr(id, errs, SOFT_ERR_TTL_MS);
        }
    }

    function handleNetworkError(ids: string[]): void {
        const netErr: Err = { message: "Network error", code: 0 } as Err;
        for (const id of ids) {
            updateErr(id, [netErr], SOFT_ERR_TTL_MS);
        }
    }
}

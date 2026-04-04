import { redirect, type ServerLoad } from "@sveltejs/kit";
import { MyVokisPageTabMarker } from "./tab-marker";

export const load: ServerLoad = async ({ cookies }) => {
    const currentTab: MyVokisPageTabMarker.Tab = MyVokisPageTabMarker.get(cookies) ?? "draft-vokis";
    throw redirect(307, `/my-vokis/${currentTab}`);
};  
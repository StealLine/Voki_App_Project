import type { LayoutServerLoad } from './$types';
import { MyVokisPageTabMarker } from './tab-marker';

export const load: LayoutServerLoad = async ({ url, cookies }): Promise<{ currentTab: MyVokisPageTabMarker.Tab }> => {
	const currentTab = extractTabFromPathname(url.pathname);
	if (currentTab !== null) {
		MyVokisPageTabMarker.set(cookies, currentTab);
	}
	return {
		currentTab: currentTab ?? "draft-vokis"
	};
};
function extractTabFromPathname(pathname: string): MyVokisPageTabMarker.Tab | null {
	for (const segment of pathname.split('/')) {
		if (MyVokisPageTabMarker.isTab(segment)) {
			console.log("erly ret: ", segment);
			return segment;
		}
	}
	return null;
}

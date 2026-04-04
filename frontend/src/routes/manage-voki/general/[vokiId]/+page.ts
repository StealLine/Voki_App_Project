import { redirect } from '@sveltejs/kit';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ params }) => {
    throw redirect(308, `/manage-voki/general/${params.vokiId}/main`);
};
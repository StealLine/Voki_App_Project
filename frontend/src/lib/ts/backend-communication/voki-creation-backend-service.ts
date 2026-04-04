import type { Language } from "../language";
import { BackendService, RJO } from "./backend-services";
import type { ResponseResult } from "./result-types";



export type VokiDetails = {
    description: string;
    language: Language;
    hasMatureContent: boolean;
}

export type DraftVokiPublishingData = {
    issues: VokiPublishingIssue[]
    primaryAuthorId: string
    coAuthorIds: string[]
    userIdsToBecomeManagers: string[]
}
export type VokiPublishingIssueType = 'Problem' | 'Warning';
export type VokiPublishingIssue = {
    type: VokiPublishingIssueType;
    message: string;
    source: string;
    fixRecommendation: string;
}

export type VokiSuccessfullyPublishedData = {
    id: string;
    cover: string;
    name: string;
}
export interface IVokiCreationBackendService {
    setVokiCoverToDefault(vokiId: string): Promise<ResponseResult<{ newCover: string; }>>;
    updateVokiCover(vokiId: string, newCover: string): Promise<ResponseResult<{ newCover: string; }>>;
    updateVokiName(vokiId: string, newName: string): Promise<ResponseResult<{ newVokiName: string; }>>;
    getVokiName(vokiId: string): Promise<ResponseResult<{ vokiName: string; }>>;
    updateVokiTags(vokiId: string, tags: string[]): Promise<ResponseResult<{ newTags: string[]; }>>;
    updateVokiDetails(vokiId: string, details: VokiDetails): Promise<ResponseResult<VokiDetails>>;

    loadPublishingData(vokiId: string): Promise<ResponseResult<DraftVokiPublishingData>>;
    publishWithNoIssues(vokiId: string, coAuthorIds: string[], userIdsToBecomeManagers: string[]): Promise<ResponseResult<VokiSuccessfullyPublishedData>>;
    publishWithWarningsIgnored(vokiId: string, coAuthorIds: string[], userIdsToBecomeManagers: string[]): Promise<ResponseResult<VokiSuccessfullyPublishedData>>;
    ensureVokiExists(vokiId: string): Promise<boolean>;
}
class VokiCreationBackendService extends BackendService implements IVokiCreationBackendService {
    constructor(baseUrl: string) {
        super(baseUrl);
    }

    public async getVokiName(vokiId: string): Promise<ResponseResult<{ vokiName: string; }>> {
        return await this.fetchJsonResponse<{ vokiName: string; }>(
            `/vokis/${vokiId}/voki-name`, { method: 'GET' }
        );
    }


    public async setVokiCoverToDefault(vokiId: string): Promise<ResponseResult<{ newCover: string; }>> {
        return await this.fetchJsonResponse<{ newCover: string }>(
            `/vokis/${vokiId}/set-cover-to-default`,
            { method: 'PATCH' }
        );
    }

    public async updateVokiCover(vokiId: string, newCover: string): Promise<ResponseResult<{ newCover: string; }>> {
        return await this.fetchJsonResponse<{ newCover: string; }>(
            `/vokis/${vokiId}/update-cover`,
            RJO.PATCH({ newCover: newCover })
        );
    }

    public async updateVokiName(vokiId: string, newName: string): Promise<ResponseResult<{ newVokiName: string; }>> {
        return await this.fetchJsonResponse<{ newVokiName: string; }>(
            `/vokis/${vokiId}/update-name`,
            RJO.PATCH({ newName })
        );
    }

    public async updateVokiTags(vokiId: string, newTags: string[]): Promise<ResponseResult<{ newTags: string[]; }>> {
        return await this.fetchJsonResponse<{ newTags: string[]; }>(
            `/vokis/${vokiId}/update-tags`,
            RJO.PATCH({ newTags })
        );
    }

    public async updateVokiDetails(vokiId: string, details: VokiDetails): Promise<ResponseResult<VokiDetails>> {
        return await this.fetchJsonResponse<VokiDetails>(
            `/vokis/${vokiId}/update-details`,
            RJO.PATCH({
                newDescription: details.description,
                newLanguage: details.language,
                newHasMatureContent: details.hasMatureContent
            })
        );
    }
    public async loadPublishingData(vokiId: string): Promise<ResponseResult<DraftVokiPublishingData>> {
        return await this.fetchJsonResponse<DraftVokiPublishingData>(
            `/vokis/${vokiId}/publishing-data`, { method: 'GET' }
        );
    }
    public async publishWithNoIssues(vokiId: string, coAuthorIds: string[], userIdsToBecomeManagers: string[]): Promise<ResponseResult<VokiSuccessfullyPublishedData>> {
        return await this.fetchJsonResponse<VokiSuccessfullyPublishedData>(
            `/vokis/${vokiId}/publish-with-no-issues`, RJO.POST({
                coAuthorIdsToPublishWith: coAuthorIds,
                managerIdsToPublishWith: userIdsToBecomeManagers
            })
        );
    }
    public async publishWithWarningsIgnored(vokiId: string, coAuthorIds: string[], userIdsToBecomeManagers: string[]): Promise<ResponseResult<VokiSuccessfullyPublishedData>> {
        return await this.fetchJsonResponse<VokiSuccessfullyPublishedData>(
            `/vokis/${vokiId}/publish-with-warnings-ignored`, RJO.POST({
                coAuthorIdsToPublishWith: coAuthorIds,
                managerIdsToPublishWith: userIdsToBecomeManagers
            })
        );
    }
    public async ensureVokiExists(vokiId: string): Promise<boolean> {
        let res = await this.fetchVoidResponse(
            `/vokis/${vokiId}/ensure-exists`, { method: 'GET' }
        );
        return res.isSuccess;
    }
}

export const ApiVokiCreationGeneral = new VokiCreationBackendService('/api/voki-creation/general');

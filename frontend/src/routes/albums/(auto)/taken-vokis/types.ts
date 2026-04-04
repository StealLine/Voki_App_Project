export type VokiIdToBriefVokiTakenData = Record<string, BriefVokiTakenData>;
export type BriefVokiTakenData = { timesTaken: number; lastTimeTaken: Date };
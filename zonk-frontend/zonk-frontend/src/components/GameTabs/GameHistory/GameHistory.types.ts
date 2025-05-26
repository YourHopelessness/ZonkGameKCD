
export type GameHistoryList = Array<{
    id: string,
    mode: string,
    currentName: string,
    opponentName: string,
    curentScore: number,
    opponentScore: number,
    endedAt: Date,
    isWin: boolean
}>;
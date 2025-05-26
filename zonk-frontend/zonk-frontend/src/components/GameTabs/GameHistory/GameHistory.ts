import { GameHistoryList } from "./GameHistory.types";

export default function GetGameHistory(){
    const history : GameHistoryList = [
        { id: "123", currentName:"Toyo", curentScore: 1500, isWin: true, endedAt: new Date(), opponentName :"test2", opponentScore: 1070, mode: "PvP"},
        { id: "133", currentName:"Toyo", curentScore: 1700, isWin: false, endedAt: new Date(), opponentName :"agent", opponentScore: 2180, mode: "PvE"},
        { id: "153", currentName:"Toyo", curentScore: 2150, isWin: true, endedAt: new Date(), opponentName :"agent", opponentScore: 1070, mode: "PvE"},
    ]
    return history;
}
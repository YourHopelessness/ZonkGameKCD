import { ActiveGamesList } from "./ActiveGames.types";

export default function GetActiveGames(){
    const activeGames : ActiveGamesList = [
        { id: "123", currentName:"Toyo", curentScore: 1500, opponentName :"test2", opponentScore: 1070, mode: "PvP", isYourTurn: false},
        { id: "133", currentName:"Toyo", curentScore: 1700, opponentName :"agent", opponentScore: 2180, mode: "PvE", isYourTurn: true},
        { id: "153", currentName:"Toyo", curentScore: 2150, opponentName :"agent", opponentScore: 1070, mode: "PvE", isYourTurn: true},
    ]
    return activeGames;
}
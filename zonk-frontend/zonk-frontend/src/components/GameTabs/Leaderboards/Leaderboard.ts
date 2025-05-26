import { LeaderboardList } from "./Leaderboard.types"

export default function GetLeaderboards(){
    const leaderboard : LeaderboardList = [
        { username: "JohnDot", total_score: 10000, top_number: 1, win_rate: 80 },
        { username: "Manny", total_score: 7000, top_number: 2, win_rate: 90},
        { username: "Bill", total_score: 4000, top_number: 3, win_rate: 73},
    ]
    return leaderboard;
}
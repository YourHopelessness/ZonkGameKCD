import { User } from "../Auth/Auth.types";
import { StartGameProps } from "../NewGamePanel/NewGamePanel.types";
import { ActiveGamesList } from "./ActiveGames/ActiveGames.types";
import { GameHistoryList } from "./GameHistory/GameHistory.types";
import { LeaderboardList } from "./Leaderboards/Leaderboard.types";

export type GameTabProps = {
    user: User,
    leaderboard: LeaderboardList,
    activeGames: ActiveGamesList,
    historyGames: GameHistoryList,
    onStartGame: (p: StartGameProps) => void
}

export type GameTabsViewProps = React.PropsWithChildren<{
    tab: string;
    onTabChange: (t: string) => void;
    isUserAuthorized?: boolean;
}>;
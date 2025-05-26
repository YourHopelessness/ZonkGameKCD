import { useState } from "react";
import GameTabsView from "./GameTabsView";
import LeaderboardView from "./Leaderboards/LeaderboardView";
import ActiveGamesView from "./ActiveGames/ActiveGamesView";
import GameHistoryView from "./GameHistory/GameHistoryView";
import { GameTabProps } from "./GameTabs.types";

export default function GameTabs({ 
    user, 
    leaderboard, 
    activeGames, 
    historyGames, 
    onStartGame 
    } : GameTabProps) {
  const [tab, setTab] = useState("leaderboard");

  if (!user) {
    return (
      <LeaderboardView entries={leaderboard} />
    );
  }

  let panel = null;
  if (tab === "leaderboard") panel = <LeaderboardView entries={leaderboard} />;
  if (tab === "active") panel = (
    <ActiveGamesView 
        games={activeGames} 
        onStartGame={onStartGame} />
  );
  if (tab === "history") panel = (
    <GameHistoryView history={historyGames} />
  );

  return (
    <GameTabsView
      tab={tab}
      onTabChange={setTab}
      isUserAuthorized={user != null}
    >
      {panel}
    </GameTabsView>
  );
}
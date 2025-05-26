import { useNavigate } from "react-router-dom";
import { User } from "../../components/Auth/Auth.types";
import GetActiveGames from "../../components/GameTabs/ActiveGames/ActiveGames";
import GetGameHistory from "../../components/GameTabs/GameHistory/GameHistory";
import GameTabs from "../../components/GameTabs/GameTabs";
import GetLeaderboards from "../../components/GameTabs/Leaderboards/Leaderboard";
import NewGamePanel from "../../components/NewGamePanel/NewGamePanel";
import { StartGameProps } from "../../components/NewGamePanel/NewGamePanel.types"
import styles from "./HomePage.module.css";

export default function HomePage() {
  const navigate = useNavigate();
  const startGame = (props : StartGameProps) => {
    navigate(`/play?targetScore=${props.targetScore}&mode=${props.mode}`);
  };
  const user : User = {
    username: "Имя пользователя",
    _id: "123"
  }

  return (
    <>
       <main className={styles.wrapper}>
        <div className={styles.panelHolder}>
          <NewGamePanel onStartGame={startGame} />
        </div>
        <div className={styles.tabsHolder}>
          <GameTabs
            user={user}
            leaderboard={GetLeaderboards()}
            historyGames={GetGameHistory()}
            activeGames={GetActiveGames()}
            onStartGame={startGame} />
        </div>
      </main>
    </>
  );
}
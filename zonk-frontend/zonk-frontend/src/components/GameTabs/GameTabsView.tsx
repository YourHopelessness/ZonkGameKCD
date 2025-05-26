import styles from "./GameTabs.module.css";
import { GameTabsViewProps } from "./GameTabs.types";

export default function GameTabsView({ 
    tab, 
    onTabChange, 
    isUserAuthorized, 
    children } : GameTabsViewProps) {
  return (
    <>
      <div className={styles.tabBar}>
        <div
            className={`${styles.tab} ${
                tab === "leaderboard" ? styles.tabActive : styles.tabInactive
            }`}
            onClick={() => onTabChange("leaderboard")}>Leaderboard
        </div>
        {isUserAuthorized && (
        <div
          className={`${styles.tab} ${
            tab === "active" ? styles.tabActive : styles.tabInactive
          }`}
          onClick={() => onTabChange("active")}>
          Active Games
        </div>)}
        {isUserAuthorized && (
        <div
          className={`${styles.tab} ${
            tab === "history" ? styles.tabActive : styles.tabInactive
          }`}
          onClick={() => onTabChange("history")}
        >
          History
        </div>)}
      </div>

      <div className={styles.panelBox}>{children}</div>
    </>
  );
}
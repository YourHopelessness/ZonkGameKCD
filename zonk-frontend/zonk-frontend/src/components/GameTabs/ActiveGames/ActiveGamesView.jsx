import styles from "./ActiveGames.module.css";

export default function ActiveGamesView( { games, onStartGame } ) {
  if (!games?.length) {
    return (
      <div className={styles.emptyBox}>
        <div
          className={styles.emptyImg}
          style={{ backgroundImage: 'url("/zonk-empty-games.png")' }}
        />
        <div>
          <p className={styles.emptyHead}>No active games</p>
          <p className={styles.emptyText}>Start a new game to see it here.</p>
        </div>
        <button className={styles.emptyBtn} onClick={onStartGame}>
          Start Game
        </button>
      </div>
    );
  }

  return (
    <div className={styles.wrapper}>
      {games.map((g) => (
        <div key={g.id} className={styles.row}>
          <div>
            <div className={styles.title}>{`Game with ${g.opponentName}, mode ${g.mode}`}</div>
            <div className={styles.badge}>{
                (g.isYourTurn && "Your Turn") ||
                `You: ${g.curentScore} - Opponent: ${g.opponentScore}`
            }</div>
          </div>
          <button className={styles.btn} onClick={() => onStartGame(g)}>
            Continue
          </button>
        </div>
      ))}
    </div>
  );
}
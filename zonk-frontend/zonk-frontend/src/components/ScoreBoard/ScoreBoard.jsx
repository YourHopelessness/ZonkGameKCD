import styles from "./ScoreBoard.module.css";

export default function ScoreBoard({ players, target }) {
  return (
    <div className={styles.card}>
      <h3 className={styles.title}>Target: {target}</h3>

      {players.map((p) => (
        <div key={p.name} className={styles.row}>
          <span className={`${styles.name} ${p.isActive ? styles.active : ""}`}>
            {p.name}
          </span>
          <span className={styles.score}>{p.score}</span>
        </div>
      ))}
    </div>
  );
}

import styles from "./LeaderboardView.module.css";

export default function LeaderboardView({ entries }) {
  return (
    <div className={styles.wrapper}>
      {(entries?.length ? entries : Array(5).fill(null)).map((e, i) => (
        <div key={i} className={styles.row}>
          <div
            className={styles.avatar}
            style={{
              backgroundImage: `url('${e?.avatarUrl ?? "/default-avatar.png"}')`,
            }}
          />
          <div className="flex-1">
            <div className={styles.name}>{e?.username ?? `Player ${i + 1}`}</div>
            <div className={styles.score}>{e?.total_score ?? "—"} pts</div>
          </div>
        </div>
      ))}
    </div>
  );
}
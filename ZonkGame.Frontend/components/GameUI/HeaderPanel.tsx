import styles from './GameUI.module.css';

/** Панель очков */
export default function HeaderPanel({
  playerScore,
  targetScore,
  opponentScore
}: {
  playerScore: number;
  targetScore: number;
  opponentScore: number;
}) {
  const renderBox = (label: string, value: number) => (
    <div className={styles.scoreBox}>
      <div className={styles.scoreLabel}>{label}</div>
      <div className={styles.scoreValue}>{value}</div>
    </div>
  );

  return (
    <header className={styles.headerPanel}>
      {renderBox('Player total score', playerScore)}
      {renderBox('Turn Score', playerScore)}
      {renderBox('Target score', targetScore)}
      {renderBox('Opponent score', opponentScore)}
    </header>
  );
}
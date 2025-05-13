import styles from './GameUI.module.css';

/** Информация о раунде */
export default function RoundInfo({ currentRound }: { currentRound: number }) {
  return (
    <div className={styles.roundInfo}>
      <div className={styles.roundLabel}>Раунд</div>
      <div className={styles.roundValue}>{currentRound}</div>
    </div>
  );
}
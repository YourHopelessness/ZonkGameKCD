import React, { useState } from 'react';
import Link from 'next/link';
import styles from './HomePage.module.css';

export interface LeaderboardEntry {
  name: string;
  score: number;
  highlight?: boolean;
}

interface HomePageProps {
  leaderboard: LeaderboardEntry[];
  recentGames: string[];
  activeGames: string[];
}

const HomePage: React.FC<HomePageProps> = ({ leaderboard, recentGames, activeGames }) => {
  const [mode, setMode] = useState<'PvP' | 'PvE'>('PvE');
  const [target, setTarget] = useState<number>(2000);

  const handleMode = (m: 'PvP' | 'PvE') => () => setMode(m);
  const onTargetChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const val = parseInt(e.target.value, 10);
    if (!isNaN(val)) setTarget(val);
  };

  return (
    <>
      <section className={styles.hero}>
         <div className={styles.toggleGroup}>
          <button
            className={`${styles.toggleButton} ${mode === 'PvP' ? styles.active : ''}`}
            onClick={handleMode('PvP')}
          >
            PvP
          </button>
          <button
            className={`${styles.toggleButton} ${mode === 'PvE' ? styles.active : ''}`}
            onClick={handleMode('PvE')}
          >
            PvE
          </button>
        </div>
        <div className={styles.targetGroup}>
          <div className={styles.heroSubtitle}>Target score:</div>
          <input
            type="number"
            value={target}
            onChange={onTargetChange}
            className={styles.targetInput}
            min={0}
          />
        </div>  
        <Link
          href={`/play?mode=${mode}&target=${target}`}
          className={styles.startButton}>Start game
        </Link>
      </section>

      <main className="content">
        <div className="leaderboard section">
          <h2 className="section-title">Leaderboard</h2>
          <ul>
            {leaderboard.map(item => (
              <li key={item.name} className={item.highlight ? 'highlight' : ''}>
                {item.name} {item.score}
              </li>
            ))}
          </ul>
        </div>

        <div className="recent-games section">
          <h2 className="section-title">My recent games</h2>
          <ul>
            {recentGames.map((g, i) => (
              <li key={i}>{g}</li>
            ))}
          </ul>
        </div>

        <div className="active-games section">
          <h2 className="section-title">My active games</h2>
          <ul>
            {activeGames.map((g, i) => (
              <li key={i}>{g}</li>
            ))}
          </ul>
        </div>
      </main>
    </>
  );
};

export default HomePage;
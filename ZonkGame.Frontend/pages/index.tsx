import type { NextPage } from 'next';
import React from 'react';
import Header from '../components/Header';
import HomePage, { LeaderboardEntry } from '../components/HomePage/HomePage';
import GamesFooter, { GameCard } from '../components/GameFooter';

const leaderboardData: LeaderboardEntry[] = [
  { name: 'Popo', score: 35700 },
  { name: 'Meme', score: 31000, highlight: true },
  { name: 'Aboba', score: 29850 },
  { name: 'AI agent', score: 27000 }
];

const recentGamesData = [
  'PvP 2000: me 2160 – popo 1800',
  'PvP 2000: me 1070 – aboba 2190',
  'PvE 1500: me 1570 – agent 1570'
];

const activeGamesData = [
  'PvP 2000: me 135 – popo 670'
];

const moreGames: GameCard[] = [
  { id: 'g1', title: 'Zonk Duel', description: 'Challenge a friend' },
  { id: 'g2', title: 'Zonk Marathon', description: 'Endurance mode' },
  { id: 'g3', title: 'Zonk Blitz', description: 'Fast-paced fun' }
];

const Home: NextPage = () => (
  <>
    <Header />
    <HomePage
      leaderboard={leaderboardData}
      recentGames={recentGamesData}
      activeGames={activeGamesData}
    />
    <GamesFooter games={moreGames} />
  </>
);

export default Home;

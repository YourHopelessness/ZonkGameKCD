import React from 'react';

export interface GameCard {
  id: string;
  title: string;
  description: string;
}

interface GamesFooterProps {
  games: GameCard[];
}

const GamesFooter: React.FC<GamesFooterProps> = ({ games }) => (
  <footer className="content">
    {games.map(game => (
      <div key={game.id} className="section">
        <h2 className="section-title">{game.title}</h2>
        <p>{game.description}</p>
      </div>
    ))}
  </footer>
);

export default GamesFooter;
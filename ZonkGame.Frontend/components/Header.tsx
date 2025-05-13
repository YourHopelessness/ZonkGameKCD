import Link from 'next/link';
import React from 'react';

const Header: React.FC = () => (
  <header className="header">
    <div className="logo">
      <img src="/logo.svg" alt="Zonk Game" />
    </div>
    <nav>
      <Link href="/play" className="nav-link">Play</Link>
      <Link href="/faq" className="nav-link">FAQ</Link>
      <Link href="/stats" className="nav-link">Stats</Link>
      <button className="btn profile">Profile</button>
      <button className="btn logout">Logout</button>
    </nav>
  </header>
);

export default Header;
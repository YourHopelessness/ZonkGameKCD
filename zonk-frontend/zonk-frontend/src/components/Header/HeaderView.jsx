import styles from './Header.module.css';

export function HeaderView({ isHomeOrPlay, user, onPlay, onSignUp }) {
  return (
    <header className={styles.header}>
      <div className={styles.left}>
        <div className={styles.logo}>
          <svg viewBox="0 0 48 48" fill="none">
            <path d="M24 45.8096C19.6865..." fill="currentColor"></path>
          </svg>
        </div>
        <h2 className={styles.title}>Zonk</h2>
      </div>
      <div className={styles.right}>
        <nav className={styles.nav}>
          <a href="/faq">FAQ</a>
          {user && <>
            <a href="/profile">Profile</a>
            <a href="/stats">Stats</a>
          </>}
        </nav>
        {user ? (
          <div
            className={styles.avatar}
            style={{ backgroundImage: `url('${user.avatarUrl || "/default-avatar.png"}')` }}
            title={user.name}
          />
        ) : (
          <button className={styles.signup} onClick={onSignUp}>Sign Up</button>
        )}
        {!isHomeOrPlay && (
          <button className={styles.playBtn} onClick={onPlay}>Play</button>
        )}
      </div>
    </header>
  );
}

import '../styles/global.css';
import type { AppProps } from 'next/app';

function ZonkGame({ Component, pageProps }: AppProps) {
  return <Component {...pageProps} />;
}

export default ZonkGame;

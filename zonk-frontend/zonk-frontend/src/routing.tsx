import { Routes, Route } from 'react-router-dom';
import Header from './components/Header/Header';
import HomePage from './pages/HomePage/HomePage';
import PlayPage from './pages/PlayPage/PlayPage';
//import FaqPage from './pages/FaqPage';
//import ProfilePage from './pages/ProfilePage';
//import StatsPage from './pages/StatsPage';
//import NotFound from './pages/NotFound';

export default function App() {
  return (
    <>
      <Header />
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/play/*" element={<PlayPage />} />
      </Routes>
    </>
  );
}

/*

        <Route path="/faq" element={<FaqPage />} />
        <Route path="/profile" element={<ProfilePage />} />
        <Route path="/stats" element={<StatsPage />} />
        <Route path="*" element={<NotFound />} />*/

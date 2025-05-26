import { useLocation, useNavigate } from "react-router-dom";
import { HeaderView } from "./HeaderView.jsx";

const mockUser = {
  name: "Alice",
  avatarUrl: "https://randomuser.me/api/portraits/women/44.jpg"
  // или null, если не залогинен
};

export default function Header() {
    console.log('Header');
  const location = useLocation();
  const navigate = useNavigate();
  const isHomeOrPlay = location.pathname === '/' || location.pathname.startsWith('/play');

  const user = mockUser; // или null если не залогинен

  return (
    <HeaderView
      isHomeOrPlay={isHomeOrPlay}
      user={user}
      onPlay={() => navigate('/')}
      onSignUp={() => navigate('/signup')}
    />
  );
}

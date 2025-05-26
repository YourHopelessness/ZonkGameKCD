export type HeaderViewProps = {
  isHomeOrPlay: boolean;
  user: { name: string; avatarUrl?: string } | null;
  onPlay: () => void;
  onSignUp: () => void;
};
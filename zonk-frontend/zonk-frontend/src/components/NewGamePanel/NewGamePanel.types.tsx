export type StartGameProps = {
    mode : string,
    targetScore : number
}

export interface NewGamePanelProps {
  onStartGame?: (params: StartGameProps) => void;
}
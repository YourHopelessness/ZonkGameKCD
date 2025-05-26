import { StartGameProps } from "../../NewGamePanel/NewGamePanel.types";

export type ActiveGamesList = Array<{
    id: string,
    mode: string,
    currentName: string,
    opponentName: string,
    curentScore: number,
    opponentScore: number,
    isYourTurn: boolean
}>;

export interface ActiveGamesViewProps {
  games : ActiveGamesList,
  onStartGame?: (params: StartGameProps) => void
}
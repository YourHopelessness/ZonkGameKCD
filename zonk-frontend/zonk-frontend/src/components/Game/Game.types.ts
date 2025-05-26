import { User } from "../Auth/Auth.types";

export type Game = {
    _id: string,
    created_at: Date,
    ended_at?: Date,
    players?: PlayerState[],
    round_count: number | 1,
    winner?: User,
    mode: "PvP" | "PvE",
    current_roll: (1|2|3|4|5|6)[],
}

export type PlayerState = {
    user: User,
    selected_dice: (1|2|3|4|5|6)[],
    turn_score: number | 0,
    total_player_score: number | 0,
    is_current_player: boolean | false
}
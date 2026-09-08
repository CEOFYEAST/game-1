namespace game_1;

public enum GameState
{
    // Title screen: waiting for the player to start a run.
    Start,

    // A run is in progress and mobs are spawning.
    Playing,

    // The player ran out of touches; waiting for them to quit.
    GameOver
}

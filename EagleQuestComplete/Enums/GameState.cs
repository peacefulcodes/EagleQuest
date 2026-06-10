namespace EagleQuest.Enums
{
    // VIVA: Enums give meaningful names to states instead of magic numbers.
    // GameState tracks whether the game is running, paused, over, or won.

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        LevelComplete,
        Win
    }
}

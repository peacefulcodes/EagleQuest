namespace EagleQuest.Managers
{
    // VIVA: ScoreManager controls the score.
    // The score field is private — only AddScore() and Reset() can change it.
    // This is ENCAPSULATION — data is protected inside the class.

    public class ScoreManager
    {
        private int score;
        private int highScore;

        public int Score
        {
            get { return score; }
        }

        public int HighScore
        {
            get { return highScore; }
        }

        public ScoreManager()
        {
            score = 0;
            highScore = 0;
        }

        public void AddScore(int points)
        {
            score += points;

            // Update high score if current score exceeds it
            if (score > highScore)
                highScore = score;
        }

        public void Reset()
        {
            score = 0;
        }
    }
}

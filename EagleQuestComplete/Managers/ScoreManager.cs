namespace EagleQuest.Managers
{
    

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

            
            if (score > highScore)
                highScore = score;
        }

        public void Reset()
        {
            score = 0;
        }
    }
}

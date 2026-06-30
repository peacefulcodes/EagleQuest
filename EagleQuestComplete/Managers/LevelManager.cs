namespace EagleQuest.Managers
{
    

    public class LevelManager
    {
        private int currentLevel;
        private int timeLeft;       // in seconds
        private int foodRequired;
        private int maxLevel;

        public int CurrentLevel
        {
            get { return currentLevel; }
        }

        public int TimeLeft
        {
            get { return timeLeft; }
        }

        public int FoodRequired
        {
            get { return foodRequired; }
        }

        public int MaxLevel
        {
            get { return maxLevel; }
        }

        public bool IsTimeUp
        {
            get { return timeLeft <= 0; }
        }

        private int tickCounter;
        private const int TICKS_PER_SECOND = 10;

        public LevelManager()
        {
            maxLevel = 3;
            currentLevel = 1;
            LoadLevel(1);
        }

        public void LoadLevel(int level)
        {
            currentLevel = level;
            
            tickCounter = -5;

           
            if (level == 1)
            {
                foodRequired = 5;
                timeLeft = 60;
            }
            else if (level == 2)
            {
                foodRequired = 5;
                timeLeft = 50;
            }
            else if (level == 3)
            {
                foodRequired = 7;
                timeLeft = 45;
            }
        }

        
        public void Tick()
        {
            tickCounter++;
            if (tickCounter >= TICKS_PER_SECOND)
            {
                tickCounter = 0;
                if (timeLeft > 0)
                    timeLeft--;
            }
        }

        public void AddTime(int seconds)
        {
            timeLeft += seconds;
        }

        public bool IsLevelComplete(int foodCollected)
        {
            return foodCollected >= foodRequired;
        }

        public bool HasNextLevel()
        {
            return currentLevel < maxLevel;
        }

        public void LoadNextLevel()
        {
            LoadLevel(currentLevel + 1);
        }

        public bool IsLastLevel()
        {
            return currentLevel == maxLevel;
        }
    }
}

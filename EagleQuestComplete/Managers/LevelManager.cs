namespace EagleQuest.Managers
{
    // VIVA: LevelManager controls level progression.
    // It knows how many food items are required per level,
    // how much time is allowed, and when to move to the next level.
    // All data is private with controlled public access — ENCAPSULATION.

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

        // Timer ticks at 100ms (10 ticks = 1 second)
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
            // Start tickCounter at -5 so first second takes 1.5 seconds
            // This gives the player a brief moment before countdown starts
            // and prevents leftover tickCounter state from previous level
            tickCounter = -5;

            // Each level: more food needed, less time
            if (level == 1)
            {
                foodRequired = 3;
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

        // Called every game tick to count down time
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

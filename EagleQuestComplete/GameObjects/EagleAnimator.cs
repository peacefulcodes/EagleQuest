using System.Drawing;

namespace EagleQuest.GameObjects
{
    

    public class EagleAnimator
    {
        private Image[] rightFrames;  
        private Image[] leftFrames;   
        private int currentFrame;
        private int tickCounter;
        private int frameInterval;
        private bool facingRight;

        public EagleAnimator(
            Image right0, Image right1, Image right2,
            Image left0,  Image left1,  Image left2,
            int interval = 3)
        {
            rightFrames   = new Image[] { right0, right1, right2 };
            leftFrames    = new Image[] { left0,  left1,  left2 };
            currentFrame  = 0;
            tickCounter   = 0;
            frameInterval = interval;
            facingRight   = true;
        }

        
        public void SetDirection(bool movingRight)
        {
            facingRight = movingRight;
        }

       
        public Image GetCurrentFrame()
        {
            tickCounter++;
            if (tickCounter >= frameInterval)
            {
                tickCounter = 0;
                currentFrame++;
                if (currentFrame >= 3)
                    currentFrame = 0;
            }

            
            return facingRight ? rightFrames[currentFrame] : leftFrames[currentFrame];
        }

        public void Reset()
        {
            currentFrame = 0;
            tickCounter  = 0;
        }
    }
}

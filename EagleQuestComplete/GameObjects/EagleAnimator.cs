using System.Drawing;

namespace EagleQuest.GameObjects
{
    // VIVA: EagleAnimator handles wing-flapping animation AND direction.
    // It contains two sets of 3 frames: left-facing and right-facing.
    // When player moves left, it shows left frames. Moving right shows right frames.
    // Player class CONTAINS an EagleAnimator — this is COMPOSITION.

    public class EagleAnimator
    {
        private Image[] rightFrames;  // eagle facing right (default)
        private Image[] leftFrames;   // eagle facing left (mirrored)
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

        // Called by Player.MoveLeft() and MoveRight() to update facing direction
        public void SetDirection(bool movingRight)
        {
            facingRight = movingRight;
        }

        // Called every tick — returns correct frame based on direction
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

            // Return left or right facing frame set
            return facingRight ? rightFrames[currentFrame] : leftFrames[currentFrame];
        }

        public void Reset()
        {
            currentFrame = 0;
            tickCounter  = 0;
        }
    }
}

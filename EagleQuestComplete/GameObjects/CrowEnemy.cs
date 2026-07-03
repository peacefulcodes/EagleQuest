using System.Drawing;
using System.Windows.Forms;

namespace EagleQuest.GameObjects
{
    

    public class CrowEnemy : Enemy
    {
        private bool movingRight;

        
        private Image frameLeft0;   
        private Image frameLeft1;   
        private Image frameRight0;  
        private Image frameRight1;  
        private int animTick;
        private bool hasAnimation;

        public CrowEnemy(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
            speed       = 4;
            damage      = 1;
            movingRight = true;
            animTick    = 0;
            hasAnimation = false;
            featherHitsRemaining = 1;   // Crow: 1 feather hit to destroy
        }

        
        public void SetAnimationFrames(Image left0, Image left1, Image right0, Image right1)
        {
            frameLeft0   = left0;
            frameLeft1   = left1;
            frameRight0  = right0;
            frameRight1  = right1;
            hasAnimation = true;
        }

        
        public override void Update()
        {
            if (!IsAlive) return;

            
            if (movingRight)
            {
                X += speed;
                if (X + Width >= formWidth) movingRight = false;
            }
            else
            {
                X -= speed;
                if (X <= 0) movingRight = true;
            }

            if (hasAnimation)
            {
                animTick++;
                bool wingUp = (animTick / 3) % 2 == 0;

                if (movingRight)
                    Sprite.Image = wingUp ? frameRight0 : frameRight1;
                else
                    Sprite.Image = wingUp ? frameLeft0  : frameLeft1;
            }
        }
    }
}

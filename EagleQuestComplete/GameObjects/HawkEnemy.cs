using System;
using System.Drawing;
using System.Windows.Forms;

namespace EagleQuest.GameObjects
{
   

    public class HawkEnemy : Enemy
    {
        private Player target;
        private bool divingDown;
        private int startY;
        private int diveTimer;

        
        private Image frameLeft1;
        private Image frameLeft2;
        private Image frameRight1;
        private Image frameRight2;
        private int animTick;
        private bool hasAnimation;
        private bool movingLeft; 

        public HawkEnemy(PictureBox pb, int formW, int formH, Player player)
            : base(pb, formW, formH)
        {
            speed        = 3;
            damage       = 1;
            target       = player;
            divingDown   = false;
            diveTimer    = 0;
            animTick     = 0;
            hasAnimation = false;
            movingLeft   = true;
            featherHitsRemaining = 2;   // Hawk: 2 feather hits to destroy
        }

        
        public void SetAnimationFrames(Image left1, Image left2, Image right1, Image right2)
        {
            frameLeft1   = left1;
            frameLeft2   = left2;
            frameRight1  = right1;
            frameRight2  = right2;
            hasAnimation = true;
        }

        public override void Update()
        {
            if (!IsAlive) return;

            // Patrol left
            X -= speed;
            movingLeft = true;

            if (X + Width < 0)
            {
                // Respawn on right side at a new height
                X = formWidth;
                Y = new Random().Next(60, formHeight / 2);
            }

            // Every 40 ticks, dive toward player Y
            diveTimer++;
            if (diveTimer >= 40)
            {
                diveTimer  = 0;
                divingDown = true;
                startY     = Y;
            }

            if (divingDown)
            {
                if (Y < target.Y) Y += speed + 3;
                else if (Y > target.Y) Y -= speed + 3;

                if (Math.Abs(Y - startY) > 100)
                    divingDown = false;
            }

            // Wing flap animation — switch frame every 3 ticks
            if (hasAnimation)
            {
                animTick++;
                bool wingUp = (animTick / 3) % 2 == 0;

                if (movingLeft)
                    Sprite.Image = wingUp ? frameLeft1 : frameLeft2;
                else
                    Sprite.Image = wingUp ? frameRight1 : frameRight2;
            }
        }
    }
}

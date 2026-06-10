using System.Drawing;
using System.Windows.Forms;

namespace EagleQuest.GameObjects
{
    // VIVA: CrowEnemy IS-A Enemy IS-A GameObject — three level inheritance chain.
    // Demonstrates POLYMORPHISM: Update() moves crow left/right AND animates wings.
    // COMPOSITION: CrowEnemy HAS-A simple 2-frame animator for wing flap.

    public class CrowEnemy : Enemy
    {
        private bool movingRight;

        // 2-frame wing animation
        private Image frameLeft0;   // facing left frame 0
        private Image frameLeft1;   // facing left frame 1 (wings different)
        private Image frameRight0;  // facing right frame 0
        private Image frameRight1;  // facing right frame 1
        private int animTick;
        private bool hasAnimation;

        public CrowEnemy(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
            speed       = 4;
            damage      = 1;
            movingRight = true;
            animTick    = 0;
            hasAnimation = false;
        }

        // Call this to give the crow wing-flap animation
        // frame0 = wings mid, frame1 = wings slightly shifted
        public void SetAnimationFrames(Image left0, Image left1, Image right0, Image right1)
        {
            frameLeft0   = left0;
            frameLeft1   = left1;
            frameRight0  = right0;
            frameRight1  = right1;
            hasAnimation = true;
        }

        // VIVA: This override moves crow AND animates its wings.
        // Polymorphism — same Update() call as all other GameObjects.
        public override void Update()
        {
            if (!IsAlive) return;

            // Move left or right
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

            // Animate wings every 3 ticks (300ms)
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

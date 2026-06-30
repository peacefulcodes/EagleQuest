using System.Drawing;
using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    

    public class RockObstacle : Obstacle
    {
        
        public bool SceneryOnly { get; set; }

        public RockObstacle(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
            SceneryOnly = false; // default: rock causes damage (Level 1)
        }

        public override void Update()
        {
            
        }

        
        public override void OnCollision(GameObject other)
        {
            // Rock never initiates damage — Player handles it in Player.OnCollision
        }
    }


    
    public class MountainTipObstacle : Obstacle
    {
        public MountainTipObstacle(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
        }

        public override void Update()
        {
           
        }

        
        public override Rectangle HitBox
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }
    }


    

    public class StormCloud : Obstacle
    {
        private int speed;

        public StormCloud(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
            speed = 2;
        }

        public override void Update()
        {
            if (!IsAlive) return;

            // Drift slowly from right to left
            X -= speed;

            // Wrap around to the right when off screen
            if (X + Width < 0)
            {
                X = formWidth + 10;
            }
        }
    }


   

    public class BulletObstacle : Obstacle
    {
        private int speed;

        public BulletObstacle(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
            speed = 8;
        }

        public override void Update()
        {
            if (!IsAlive) return;

            Y += speed;

            // Destroy bullet when it leaves the screen
            if (Y > formHeight)
            {
                Destroy();
            }
        }
    }
}

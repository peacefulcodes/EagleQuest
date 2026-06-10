using System.Windows.Forms;

namespace EagleQuest.GameObjects
{
    // VIVA: RockObstacle is a static obstacle — it doesn't move.
    // Its Update() does nothing (but it must override it because
    // the parent abstract class requires it). This is still valid polymorphism.

    public class RockObstacle : Obstacle
    {
        public RockObstacle(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
        }

        public override void Update()
        {
            // Rocks don't move — static obstacle
            // Update() is still required because GameObject declares it abstract
        }
    }


    // VIVA: StormCloud drifts slowly across the screen.
    // Same parent class as RockObstacle, totally different Update() behaviour.
    // That difference IS the polymorphism demonstration.

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


    // VIVA: BulletObstacle is fired by MilitaryPlane in Level 3.
    // It IS-A Obstacle IS-A GameObject — same clean inheritance chain.
    // Its Update() moves it straight down toward the ground.

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

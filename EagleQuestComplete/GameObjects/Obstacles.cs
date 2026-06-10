using System.Drawing;
using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    // VIVA: RockObstacle is a static obstacle — it doesn't move.
    // Its Update() does nothing (but it must override it because
    // the parent abstract class requires it). This is still valid polymorphism.

    public class RockObstacle : Obstacle
    {
        // When true this rock is scenery only — it does NOT damage the eagle.
        // Used in Level 2 and Level 3 where background rocks should be visual only.
        public bool SceneryOnly { get; set; }

        public RockObstacle(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
            SceneryOnly = false; // default: rock causes damage (Level 1)
        }

        public override void Update()
        {
            // Rocks don't move — static obstacle
            // Update() is still required because GameObject declares it abstract
        }

        // VIVA: Override OnCollision to honour SceneryOnly flag.
        // If SceneryOnly is true the obstacle does nothing on collision —
        // it is background decoration only.  Player.OnCollision still fires
        // first, so we must also skip damage there (handled via SceneryOnly
        // being checked in Player.OnCollision by casting the other object).
        public override void OnCollision(GameObject other)
        {
            // Rock never initiates damage — Player handles it in Player.OnCollision
        }
    }


    // VIVA: MountainTipObstacle represents the dangerous pointed tip
    // of a Level 1 mountain.  It is invisible (transparent PictureBox)
    // but has a precise HitBox placed at each mountain tip.
    // This gives fair, visible-area-only collision without a huge rectangle
    // that would make the level impossible.

    public class MountainTipObstacle : Obstacle
    {
        public MountainTipObstacle(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
        }

        public override void Update()
        {
            // Mountain tips don't move — static obstacle
        }

        // HitBox IS the full PictureBox for a tip — the PictureBox is already
        // sized to just the dangerous tip area, so no extra shrinking is needed.
        public override Rectangle HitBox
        {
            get { return new Rectangle(X, Y, Width, Height); }
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

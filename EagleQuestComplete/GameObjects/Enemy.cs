using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    

    public abstract class Enemy : GameObject, ICollidable
    {
        
        protected int speed;
        protected int damage;
        protected int formWidth;
        protected int formHeight;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int Damage
        {
            get { return damage; }
        }

        // VIVA: Encapsulation — feather hit points protected, only changed
        // through TakeFeatherHit(). Each subclass sets its own value.
        protected int featherHitsRemaining;

        public Enemy(PictureBox pb, int formW, int formH) : base()
        {
            Sprite = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            formWidth  = formW;
            formHeight = formH;
            featherHitsRemaining = 1;   // safe default — CrowEnemy
        }

        // VIVA: Called by FeatherProjectile.OnCollision only.
        // One feather removes exactly one hit. Reaches 0 → Destroy().
        // Damage applied from ONE side only — prevents double counting.
        public virtual void TakeFeatherHit()
        {
            if (!IsAlive)
                return;

            featherHitsRemaining--;

            if (featherHitsRemaining <= 0)
                Destroy();
        }

        public abstract override void Update();

        // Handles player collision only.
        // Feather damage is intentionally NOT handled here — only in
        // FeatherProjectile.OnCollision() — to avoid counting one collision twice.
        public virtual void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                // Player.OnCollision() handles the life-loss side
            }
        }
    }
}

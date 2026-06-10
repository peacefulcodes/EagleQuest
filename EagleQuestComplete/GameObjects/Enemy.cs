using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    // VIVA: Enemy is abstract and inherits from GameObject.
    // It is the parent of CrowEnemy, HawkEnemy, MilitaryPlane.
    // Each child overrides Update() differently — that is POLYMORPHISM.
    // Enemy also implements ICollidable.

    public abstract class Enemy : GameObject, ICollidable
    {
        // Protected so child classes can access directly
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

        public Enemy(PictureBox pb, int formW, int formH) : base()
        {
            Sprite = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            formWidth = formW;
            formHeight = formH;
        }

        // abstract — every enemy child must define its own movement
        public abstract override void Update();

        // VIVA: ICollidable implementation.
        // When an enemy hits the player, the player loses a life.
        // The enemy itself can also react — e.g. bounce back.
        public virtual void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                // Enemy continues — player handles damage in Player.OnCollision
            }

            if (other is FeatherProjectile)
            {
                // Enemy is destroyed by feather
                Destroy();
            }
        }
    }
}

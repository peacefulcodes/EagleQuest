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

        public Enemy(PictureBox pb, int formW, int formH) : base()
        {
            Sprite = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            formWidth = formW;
            formHeight = formH;
        }

        
        public abstract override void Update();

        
        public virtual void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                
            }

            if (other is FeatherProjectile)
            {
                
                Destroy();
            }
        }
    }
}

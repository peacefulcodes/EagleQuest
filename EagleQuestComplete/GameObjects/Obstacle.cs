using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    

    public abstract class Obstacle : GameObject, ICollidable
    {
        protected int formWidth;
        protected int formHeight;

        public Obstacle(PictureBox pb, int formW, int formH) : base()
        {
            Sprite = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            formWidth = formW;
            formHeight = formH;
        }

        public abstract override void Update();

        public virtual void OnCollision(GameObject other)
        {
            
        }
    }
}

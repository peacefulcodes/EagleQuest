using System.Drawing;
using System.Windows.Forms;

namespace EagleQuest.GameObjects
{
    
    public abstract class GameObject
    {
        
        private bool isAlive;
        private PictureBox sprite;

        public PictureBox Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public int X
        {
            get { return sprite.Left; }
            set { sprite.Left = value; }
        }

        public int Y
        {
            get { return sprite.Top; }
            set { sprite.Top = value; }
        }

        public int Width
        {
            get { return sprite.Width; }
        }

        public int Height
        {
            get { return sprite.Height; }
        }

        
        public Rectangle Bounds
        {
            get { return sprite.Bounds; }
        }

        
        public virtual Rectangle HitBox
        {
            get
            {
                
                return new Rectangle(X + 8, Y + 8, Width - 16, Height - 16);
            }
        }

        
        public abstract void Update();

        
        public virtual void Destroy()
        {
            isAlive = false;
            if (sprite != null)
            {
                sprite.Visible = false;
            }
        }

       
        public GameObject()
        {
            isAlive = true;
        }
    }
}

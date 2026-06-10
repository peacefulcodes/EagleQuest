using System.Drawing;
using System.Windows.Forms;

namespace EagleQuest.GameObjects
{
    // VIVA: GameObject is an ABSTRACT class.
    // Abstract means you cannot do "new GameObject()" directly.
    // It is the parent of Player, Enemy, Obstacle, Collectible.
    // This shows ABSTRACTION and is the root of our INHERITANCE tree.

    public abstract class GameObject
    {
        // ENCAPSULATION: private fields, accessed through public properties
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

        // X and Y positions come directly from the PictureBox location
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

        // Bounds is used for collision detection
        // Rectangle.IntersectsWith() checks if two rectangles overlap
        public Rectangle Bounds
        {
            get { return sprite.Bounds; }
        }

        // VIVA: HitBox is a smaller rectangle inside Bounds.
        // PNG images have transparent padding — using full Bounds causes
        // collisions before objects visually touch.
        // HitBox shrinks the collision area to the visible part of the sprite.
        // Child classes can override this for tighter or looser collision.
        public virtual Rectangle HitBox
        {
            get
            {
                // Default: shrink 8px on each side
                return new Rectangle(X + 8, Y + 8, Width - 16, Height - 16);
            }
        }

        // VIVA: abstract method means every child class MUST override this.
        // Player.Update() moves the player.
        // CrowEnemy.Update() moves the crow horizontally.
        // HawkEnemy.Update() makes the hawk dive.
        // Same method name, different behaviour = POLYMORPHISM.
        public abstract void Update();

        // virtual means child classes CAN override this if they want
        // but they don't have to — this default behaviour works for most
        public virtual void Destroy()
        {
            isAlive = false;
            if (sprite != null)
            {
                sprite.Visible = false;
            }
        }

        // Constructor sets isAlive to true when any object is created
        public GameObject()
        {
            isAlive = true;
        }
    }
}

using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    // VIVA: Obstacle is another abstract child of GameObject.
    // Obstacles are not enemies — they don't hunt the player.
    // They just exist in the sky and the eagle must avoid them.
    // RockObstacle and StormCloud inherit from this.

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
            // Obstacles don't get destroyed on collision with player
            // Player handles the damage in Player.OnCollision
        }
    }
}

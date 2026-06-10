using System.Windows.Forms;
using EagleQuest.Interfaces;
using EagleQuest.Enums;

namespace EagleQuest.GameObjects
{
    // VIVA: Collectible is abstract and implements both ICollidable and ICollectible.
    // Two interfaces on one class — shows that a class can implement multiple contracts.
    // FoodItem and PowerUp inherit from this.

    public abstract class Collectible : GameObject, ICollidable, ICollectible
    {
        protected int formWidth;
        protected int formHeight;

        public Collectible(PictureBox pb, int formW, int formH) : base()
        {
            Sprite = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            formWidth = formW;
            formHeight = formH;
        }

        public abstract override void Update();

        // ICollectible contract — child classes must define what happens on collect
        public abstract void Collect(Player player);

        // ICollidable contract — when eagle touches this, call Collect
        public void OnCollision(GameObject other)
        {
            if (other is Player && IsAlive)
            {
                Collect((Player)other);
                Destroy();
            }
        }
    }


    // VIVA: FoodItem IS-A Collectible IS-A GameObject.
    // When collected by PLAYER ONLY, it increases food count.
    // Enemy overlap does NOT collect/hide food.

    public class FoodItem : Collectible
    {
        public FoodItem(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
        }

        public override void Update()
        {
            // Food stays still — player must fly to it
            // Z-order is set once when the PictureBox is created in Game.MakePB
        }

        public override void Collect(Player player)
        {
            player.AddFood();
        }
    }


    // VIVA: PowerUp IS-A Collectible IS-A GameObject.
    // Different types of power-ups use the PowerUpType enum.
    // Collect() checks which type and applies the right effect.

    public class PowerUp : Collectible
    {
        private PowerUpType powerUpType;

        public PowerUp(PictureBox pb, int formW, int formH, PowerUpType type) : base(pb, formW, formH)
        {
            powerUpType = type;
        }

        public override void Update()
        {
            // Power-ups also stay still — player must fly to them
        }

        public override void Collect(Player player)
        {
            // Apply the correct effect based on type
            if (powerUpType == PowerUpType.SpeedBoost)
            {
                player.ActivateSpeedBoost(50); // 50 ticks of speed boost
            }
            else if (powerUpType == PowerUpType.Shield)
            {
                player.ActivateShield(60); // 60 ticks of shield
            }
            // TimeBoost is handled by Game class via event
        }
    }


    // VIVA: FeatherProjectile is what the eagle fires in Level 3.
    // It IS-A Collectible because it can collide and trigger effects.
    // Actually more accurate — it is its own GameObject subclass.
    // It destroys enemies on contact via ICollidable.

    public class FeatherProjectile : GameObject, ICollidable
    {
        private int speed;

        public FeatherProjectile(PictureBox pb) : base()
        {
            Sprite = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            speed = 10;
        }

        public override void Update()
        {
            if (!IsAlive) return;

            // Move upward
            Y -= speed;

            // Destroy if off screen
            if (Y + Height < 0)
            {
                Destroy();
            }
        }

        public void OnCollision(GameObject other)
        {
            if (other is Enemy && IsAlive)
            {
                // Feather destroys the enemy and itself
                other.Destroy();
                Destroy();
            }
        }
    }
}

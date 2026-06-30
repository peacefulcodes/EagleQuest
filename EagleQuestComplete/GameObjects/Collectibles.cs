using System.Windows.Forms;
using EagleQuest.Interfaces;
using EagleQuest.Enums;

namespace EagleQuest.GameObjects
{
    

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

       
        public abstract void Collect(Player player);

        
        public void OnCollision(GameObject other)
        {
            if (other is Player && IsAlive)
            {
                Collect((Player)other);
                Destroy();
            }
        }
    }


    

    public class FoodItem : Collectible
    {
        public FoodItem(PictureBox pb, int formW, int formH) : base(pb, formW, formH)
        {
        }

        public override void Update()
        {
            
        }

        public override void Collect(Player player)
        {
            player.AddFood();
        }
    }


    

    public class PowerUp : Collectible
    {
        private PowerUpType powerUpType;

        public PowerUp(PictureBox pb, int formW, int formH, PowerUpType type) : base(pb, formW, formH)
        {
            powerUpType = type;
        }

        public override void Update()
        {
            
        }

        public override void Collect(Player player)
        {
           
            if (powerUpType == PowerUpType.SpeedBoost)
            {
                player.ActivateSpeedBoost(50); 
            }
            else if (powerUpType == PowerUpType.Shield)
            {
                player.ActivateShield(60); 
            }
            
        }
    }


    

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

            
            Y -= speed;

            
            if (Y + Height < 0)
            {
                Destroy();
            }
        }

        public void OnCollision(GameObject other)
        {
            if (other is Enemy && IsAlive)
            {
                
                other.Destroy();
                Destroy();
            }
        }
    }
}

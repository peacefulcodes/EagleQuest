using System.Drawing;
using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    // VIVA: Player inherits from GameObject (IS-A relationship)
    // and implements ICollidable.
    // COMPOSITION: Player HAS-A EagleAnimator for wing animation + direction.
    // ENCAPSULATION: all fields private, accessed through properties/methods.

    public class Player : GameObject, ICollidable
    {
        private int health;
        private int lives;
        private int speed;
        private int foodCollected;
        private bool isShielded;
        private int shieldTimer;
        private bool isSpeedBoosted;
        private int speedBoostTimer;
        private int formWidth;
        private int formHeight;
        private int playTop;
        private int playBottom;
        private int invincibleTimer; // brief invincibility after being hit

        // COMPOSITION: Player has-a EagleAnimator
        private EagleAnimator animator;
        private bool hasAnimation;

        // Direction tracking for eagle facing
        private bool movingLeft;
        private bool movingRight;

        public int Health        { get { return health;        } set { health = value; } }
        public int Lives         { get { return lives;         } }
        public int Speed         { get { return speed;         } }
        public int FoodCollected { get { return foodCollected; } set { foodCollected = value; } }
        public bool IsShielded   { get { return isShielded;    } }

        // VIVA: Override HitBox to match the visible eagle body area.
        // Eagle PNG has transparent wing-tips and beak padding.
        // Shrinking hitbox = collision only when eagle body actually touches.
        public override System.Drawing.Rectangle HitBox
        {
            get
            {
                // Tighter box: 10px from each side, 8px from top/bottom
                return new System.Drawing.Rectangle(X + 10, Y + 8, Width - 20, Height - 16);
            }
        }

        public Player(PictureBox pb, int formW, int formH, int pTop, int pBottom) : base()
        {
            Sprite          = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            health          = 100;
            lives           = 3;
            speed           = 7;
            foodCollected   = 0;
            isShielded      = false;
            shieldTimer     = 0;
            isSpeedBoosted  = false;
            speedBoostTimer = 0;
            formWidth       = formW;
            formHeight      = formH;
            playTop         = pTop;
            playBottom      = pBottom;
            hasAnimation    = false;
            movingLeft      = false;
            movingRight     = true;
            invincibleTimer = 0; // default facing right
        }

        public void SetAnimator(EagleAnimator eagleAnimator)
        {
            animator     = eagleAnimator;
            hasAnimation = true;
        }

        // VIVA: POLYMORPHISM — this Update() runs when game loop calls obj.Update()
        public override void Update()
        {
            // Animate wings, update direction
            if (hasAnimation && animator != null)
            {
                // Tell animator which way eagle is facing
                if (movingLeft)       animator.SetDirection(false);
                else if (movingRight) animator.SetDirection(true);
                Sprite.Image = animator.GetCurrentFrame();
            }

            // Shield countdown
            if (isShielded)
            {
                shieldTimer--;
                if (shieldTimer <= 0)
                    DeactivateShield();
            }

            // Invincibility after hit countdown
            if (invincibleTimer > 0)
                invincibleTimer--;

            // Speed boost countdown
            if (isSpeedBoosted)
            {
                speedBoostTimer--;
                if (speedBoostTimer <= 0)
                {
                    isSpeedBoosted = false;
                    speed = 7;
                }
            }

            // Reset direction flags (set each tick by HandleInput)
            movingLeft  = false;
            movingRight = false;

            CheckBoundary();
        }

        public void MoveUp()
        {
            Y -= speed;
        }

        public void MoveDown()
        {
            Y += speed;
        }

        public void MoveLeft()
        {
            X -= speed;
            movingLeft  = true;
            movingRight = false;
        }

        public void MoveRight()
        {
            X += speed;
            movingRight = true;
            movingLeft  = false;
        }

        private void CheckBoundary()
        {
            if (Y < playTop)               Y = playTop;
            if (Y + Height > playBottom)   Y = playBottom - Height;
            if (X < 0)                     X = 0;
            if (X + Width > formWidth)     X = formWidth - Width;
        }

        public void LoseLife()
        {
            if (isShielded) return;
            if (invincibleTimer > 0) return; // still invincible from last hit
            lives--;
            health = 100;
            invincibleTimer = 20; // 2 seconds of invincibility after hit
            if (animator != null) animator.Reset();
        }

        // Hit flash — blinks during invincibility window
        public void ShowHitFlash()
        {
            Sprite.BackColor = Color.FromArgb(140, Color.OrangeRed);
        }

        public void ClearHitFlash()
        {
            Sprite.BackColor = Color.Transparent;
        }

        // Returns true if eagle is currently in invincibility window
        public bool IsInvincible
        {
            get { return invincibleTimer > 0; }
        }

        public void AddFood() { foodCollected++; }

        public void ActivateShield(int duration)
        {
            isShielded       = true;
            shieldTimer      = duration;
            Sprite.BackColor = Color.FromArgb(100, Color.CornflowerBlue);
        }

        // Deactivates the shield and clears the blue tint
        private void DeactivateShield()
        {
            isShielded  = false;
            shieldTimer = 0;
            Sprite.BackColor = Color.Transparent;
        }

        public void ActivateSpeedBoost(int duration)
        {
            isSpeedBoosted  = true;
            speedBoostTimer = duration;
            speed           = 11;
        }

        public void ResetForLevel()
        {
            foodCollected    = 0;
            isShielded       = false;
            shieldTimer      = 0;
            isSpeedBoosted   = false;
            speedBoostTimer  = 0;
            speed            = 7;
            movingLeft       = false;
            movingRight      = true;
            invincibleTimer  = 0;
            Sprite.BackColor = Color.Transparent;
            if (animator != null) animator.Reset();
            X = formWidth / 2 - Width / 2;
            Y = (playTop + playBottom) / 2 - Height / 2;
        }

        public void ResetFully()
        {
            lives  = 3;
            health = 100;
            ResetForLevel();
        }

        // VIVA: ICollidable — eagle reacts when touching enemies/obstacles
        public void OnCollision(GameObject other)
        {
            if (!IsAlive) return;
            if (invincibleTimer > 0) return; // can't be hurt right now

            // SceneryOnly rocks (Level 2 / Level 3 background rocks) do NOT damage
            RockObstacle rock = other as RockObstacle;
            if (rock != null && rock.SceneryOnly) return;

            if (other is Enemy || other is Obstacle)
            {
                if (isShielded)
                {
                    // Shield absorbs the hit — deactivate shield, no life loss
                    DeactivateShield();
                    // Grant a short invincibility window so one collision event
                    // doesn't fire multiple times (same as a normal hit)
                    invincibleTimer = 20;
                }
                else
                {
                    LoseLife();
                }
            }
        }
    }
}

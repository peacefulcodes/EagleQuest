using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EagleQuest.GameObjects
{
    

    public class MilitaryPlane : Enemy
    {
        private int fireTimer;
        private int fireInterval;
        private Image bulletImage;
        public List<BulletObstacle> BulletsToSpawn { get; private set; }

        public MilitaryPlane(PictureBox pb, int formW, int formH, Image bulletImg)
            : base(pb, formW, formH)
        {
            speed          = 5;
            damage         = 1;
            fireTimer      = 0;
            fireInterval   = 25;
            bulletImage    = bulletImg;
            BulletsToSpawn = new List<BulletObstacle>();
        }

        public override void Update()
        {
            if (!IsAlive) return;

            X -= speed;

            if (X + Width < 0)
            {
                X = formWidth + 20;
                Y = new Random().Next(55, formHeight / 3);
            }

            fireTimer++;
            if (fireTimer >= fireInterval)
            {
                fireTimer = 0;
                FireBullet();
            }
        }

        private void FireBullet()
        {
            PictureBox bulletPb  = new PictureBox();
            bulletPb.Size        = new Size(10, 20);
            bulletPb.Left        = X + Width / 2;
            bulletPb.Top         = Y + Height;
            bulletPb.BackColor   = Color.Transparent;
            bulletPb.SizeMode    = PictureBoxSizeMode.StretchImage;
            // Use same plane image tinted red, or bullet image if available
            bulletPb.BackColor   = Color.Red;

            BulletObstacle bullet = new BulletObstacle(bulletPb, formWidth, formHeight);
            BulletsToSpawn.Add(bullet);
        }

        public List<BulletObstacle> GetNewBullets()
        {
            List<BulletObstacle> newBullets = new List<BulletObstacle>(BulletsToSpawn);
            BulletsToSpawn.Clear();
            return newBullets;
        }
    }
}

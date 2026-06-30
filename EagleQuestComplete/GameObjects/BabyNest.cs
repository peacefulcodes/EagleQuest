using System.Drawing;
using System.Windows.Forms;
using EagleQuest.Interfaces;

namespace EagleQuest.GameObjects
{
    
    public class BabyNest : GameObject, ICollidable
    {
        private bool nestReached;

        public bool NestReached
        {
            get { return nestReached; }
        }

        public BabyNest(PictureBox pb) : base()
        {
            Sprite = pb;
            Sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            nestReached = false;

            
            DrawNest(pb);
        }

       
        public override System.Drawing.Rectangle HitBox
        {
            get
            {
               
                return new System.Drawing.Rectangle(X + 12, Y + 18, Width - 24, Height - 22);
            }
        }

        private void DrawNest(PictureBox pb)
        {
            
            Bitmap bmp = new Bitmap(pb.Width > 0 ? pb.Width : 75,
                                    pb.Height > 0 ? pb.Height : 60);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                
                using (SolidBrush nestBrush = new SolidBrush(Color.FromArgb(139, 90, 43)))
                    g.FillEllipse(nestBrush, 5, 20, bmp.Width - 10, bmp.Height - 22);
                using (SolidBrush innerBrush = new SolidBrush(Color.FromArgb(160, 110, 60)))
                    g.FillEllipse(innerBrush, 12, 26, bmp.Width - 24, bmp.Height - 32);

                using (SolidBrush babyBrush = new SolidBrush(Color.FromArgb(220, 180, 80)))
                    g.FillEllipse(babyBrush, bmp.Width/2 - 10, 8, 20, 22);
                using (SolidBrush headBrush = new SolidBrush(Color.FromArgb(240, 230, 200)))
                    g.FillEllipse(headBrush, bmp.Width/2 - 7, 5, 14, 16);

               
                using (SolidBrush eyeBrush = new SolidBrush(Color.Black))
                    g.FillEllipse(eyeBrush, bmp.Width/2, 7, 4, 4);

                
                using (SolidBrush beakBrush = new SolidBrush(Color.FromArgb(220, 160, 20)))
                {
                    Point[] beak = {
                        new Point(bmp.Width/2 + 6, 13),
                        new Point(bmp.Width/2 + 12, 15),
                        new Point(bmp.Width/2 + 6, 17)
                    };
                    g.FillPolygon(beakBrush, beak);
                }

                // "NEST" label
                using (Font f = new Font("Segoe UI", 7, FontStyle.Bold))
                using (SolidBrush tb = new SolidBrush(Color.White))
                    g.DrawString("NEST", f, tb, bmp.Width/2 - 14, bmp.Height - 14);
            }
            pb.Image = bmp;
            pb.BackColor = Color.Transparent;
        }

        public override void Update()
        {
            // Nest stays still
        }

        // VIVA: ICollidable — eagle touching nest triggers nest return check
        public void OnCollision(GameObject other)
        {
            if (other is Player && IsAlive)
            {
                nestReached = true;
            }
        }

        public void Reset()
        {
            nestReached = false;
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EagleQuest.Forms
{
    // The first screen the player sees.
    // Dark night sky, golden title, Play / How To Play / Exit buttons.

    public class StartForm : Form
    {
        private Button btnPlay;
        private Button btnHowToPlay;
        private Button btnExit;
        private System.Windows.Forms.Timer animTimer;
        private float cloudOffset = 0;

        public StartForm()
        {
            InitializeForm();
            InitializeControls();
            InitializeAnimation();
        }

        private void InitializeForm()
        {
            this.Text            = "Eagle Quest: Nest Rescue";
            this.Size            = new Size(900, 600);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = Color.Black;
            this.DoubleBuffered  = true;
        }

        private void InitializeControls()
        {
            int formW  = 900;
            int btnW   = 220;
            int centerX = (formW - btnW) / 2;  // 340 — centers a 220px button

            // ── PLAY BUTTON ──────────────────────────────
            btnPlay = new Button();
            btnPlay.Text      = "▶   PLAY";
            btnPlay.Size      = new Size(btnW, 52);
            btnPlay.Location  = new Point(centerX, 405);
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.FlatAppearance.BorderColor = Color.FromArgb(212, 168, 67);
            btnPlay.FlatAppearance.BorderSize  = 2;
            btnPlay.BackColor  = Color.FromArgb(175, 125, 25);
            btnPlay.ForeColor  = Color.FromArgb(40, 20, 0);
            btnPlay.Font       = new Font("Georgia", 15, FontStyle.Bold);
            btnPlay.Cursor     = Cursors.Hand;
            btnPlay.Click     += BtnPlay_Click;

            // ── HOW TO PLAY BUTTON ────────────────────────
            int smallW = 104;
            int gap    = 10;
            int totalSmall = smallW * 2 + gap;
            int smallStartX = (formW - totalSmall) / 2;  // centers the pair

            btnHowToPlay = new Button();
            btnHowToPlay.Text      = "How To Play";
            btnHowToPlay.Size      = new Size(smallW, 36);
            btnHowToPlay.Location  = new Point(smallStartX, 472);
            btnHowToPlay.FlatStyle = FlatStyle.Flat;
            btnHowToPlay.FlatAppearance.BorderColor = Color.FromArgb(120, 100, 60);
            btnHowToPlay.FlatAppearance.BorderSize  = 1;
            btnHowToPlay.BackColor = Color.FromArgb(30, 25, 10);
            btnHowToPlay.ForeColor = Color.FromArgb(200, 180, 120);
            btnHowToPlay.Font      = new Font("Segoe UI", 9);
            btnHowToPlay.Cursor    = Cursors.Hand;
            btnHowToPlay.Click    += BtnHowToPlay_Click;

            // ── EXIT BUTTON ───────────────────────────────
            btnExit = new Button();
            btnExit.Text      = "Exit";
            btnExit.Size      = new Size(smallW, 36);
            btnExit.Location  = new Point(smallStartX + smallW + gap, 472);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(120, 100, 60);
            btnExit.FlatAppearance.BorderSize  = 1;
            btnExit.BackColor = Color.FromArgb(30, 25, 10);
            btnExit.ForeColor = Color.FromArgb(200, 180, 120);
            btnExit.Font      = new Font("Segoe UI", 9);
            btnExit.Cursor    = Cursors.Hand;
            btnExit.Click    += BtnExit_Click;

            this.Controls.AddRange(new Control[] { btnPlay, btnHowToPlay, btnExit });
        }

        private void InitializeAnimation()
        {
            animTimer          = new System.Windows.Forms.Timer();
            animTimer.Interval = 50; // 20fps for smooth cloud drift
            animTimer.Tick    += AnimTimer_Tick;
            animTimer.Start();
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            cloudOffset += 0.4f;
            if (cloudOffset > this.Width + 100)
                cloudOffset = -200;
            this.Invalidate(); // Redraw the form
        }

        // OnPaint draws the entire beautiful start screen background
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int W = this.ClientSize.Width;
            int H = this.ClientSize.Height;

            // ── SKY GRADIENT ─────────────────────────────
            using (LinearGradientBrush skyBrush = new LinearGradientBrush(
                new Point(0, 0), new Point(0, H),
                Color.FromArgb(10, 8, 32), Color.FromArgb(30, 50, 110)))
            {
                g.FillRectangle(skyBrush, 0, 0, W, H);
            }

            // ── STARS ────────────────────────────────────
            DrawStars(g, W, H);

            // ── MOON ─────────────────────────────────────
            DrawMoon(g);

            // ── MOVING CLOUDS ────────────────────────────
            DrawStartClouds(g, W);

            // ── MOUNTAIN SILHOUETTES ─────────────────────
            DrawMountainSilhouettes(g, W, H);

            // ── TREE SILHOUETTES ─────────────────────────
            DrawTreeSilhouettes(g, W, H);

            // ── TITLE TEXT ───────────────────────────────
            DrawTitle(g, W);

            // ── SUBTITLE ─────────────────────────────────
            using (Font subFont = new Font("Georgia", 13, FontStyle.Italic))
            using (SolidBrush subBrush = new SolidBrush(Color.FromArgb(180, 160, 100)))
            {
                string sub = "\"Fly. Collect. Return. Save the nest.\"";
                SizeF subSize = g.MeasureString(sub, subFont);
                g.DrawString(sub, subFont, subBrush,
                    (W - subSize.Width) / 2, 370);
            }
        }

        private void DrawStars(Graphics g, int W, int H)
        {
            // Fixed stars — deterministic pattern using sin/cos
            Random rng = new Random(42); // fixed seed = same stars every frame
            for (int i = 0; i < 80; i++)
            {
                int sx = rng.Next(0, W);
                int sy = rng.Next(0, H / 2 + 30);
                int size = rng.Next(1, 3);
                int alpha = rng.Next(120, 255);
                using (SolidBrush starBrush = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255)))
                {
                    g.FillEllipse(starBrush, sx, sy, size, size);
                }
            }
        }

        private void DrawMoon(Graphics g)
        {
            // Glowing moon
            int mx = 120, my = 80, mr = 50;
            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(30, 212, 200, 160)))
                g.FillEllipse(glowBrush, mx - 20, my - 20, (mr + 20) * 2, (mr + 20) * 2);
            using (SolidBrush moonBrush = new SolidBrush(Color.FromArgb(212, 200, 168)))
                g.FillEllipse(moonBrush, mx, my, mr * 2, mr * 2);
            // Shadow to make crescent
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(26, 42, 90)))
                g.FillEllipse(shadowBrush, mx + 18, my - 10, mr * 2, mr * 2);
        }

        private void DrawStartClouds(Graphics g, int W)
        {
            // Two slow-moving cloud layers
            DrawCloud(g, (int)(100 + cloudOffset * 0.3f), 55, 160, 40, Color.FromArgb(25, 255, 255, 255));
            DrawCloud(g, (int)(400 + cloudOffset * 0.5f), 40, 130, 32, Color.FromArgb(20, 255, 255, 255));
            DrawCloud(g, (int)(650 + cloudOffset * 0.2f), 70, 100, 30, Color.FromArgb(18, 255, 255, 255));
        }

        private void DrawCloud(Graphics g, int x, int y, int w, int h, Color color)
        {
            using (SolidBrush b = new SolidBrush(color))
            {
                g.FillEllipse(b, x, y, w, h);
                g.FillEllipse(b, x + w / 4, y - h / 3, w * 2 / 3, h);
                g.FillEllipse(b, x + w / 2, y, w / 2, h * 2 / 3);
            }
        }

        private void DrawMountainSilhouettes(Graphics g, int W, int H)
        {
            Color darkMtn = Color.FromArgb(10, 15, 30);
            Point[][] mountains = {
                new Point[]{ new Point(-10,H), new Point(90,250), new Point(200,H) },
                new Point[]{ new Point(80, H), new Point(210,200), new Point(340,H) },
                new Point[]{ new Point(280,H), new Point(400,175), new Point(520,H) },
                new Point[]{ new Point(440,H), new Point(560,195), new Point(680,H) },
                new Point[]{ new Point(620,H), new Point(740,215), new Point(910,H) },
            };
            using (SolidBrush mtnBrush = new SolidBrush(darkMtn))
            {
                foreach (Point[] mtn in mountains)
                    g.FillPolygon(mtnBrush, mtn);
            }
        }

        private void DrawTreeSilhouettes(Graphics g, int W, int H)
        {
            Color darkTree = Color.FromArgb(5, 8, 18);
            int[] treeX = { 18, 55, 820, 860 };
            using (SolidBrush treeBrush = new SolidBrush(darkTree))
            {
                foreach (int tx in treeX)
                {
                    // Trunk
                    g.FillRectangle(treeBrush, tx + 4, H - 80, 8, 80);
                    // Layers of foliage
                    Point[] tri1 = { new Point(tx, H - 60), new Point(tx + 8, H - 120), new Point(tx + 16, H - 60) };
                    Point[] tri2 = { new Point(tx - 5, H - 85), new Point(tx + 8, H - 150), new Point(tx + 21, H - 85) };
                    g.FillPolygon(treeBrush, tri1);
                    g.FillPolygon(treeBrush, tri2);
                }
            }
        }

        private void DrawTitle(Graphics g, int W)
        {
            // Golden glow behind title
            using (Font glowFont = new Font("Georgia", 40, FontStyle.Bold))
            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(40, 212, 168, 67)))
            {
                g.DrawString("EAGLE QUEST", glowFont, glowBrush, (W - 500) / 2 - 3, 268);
                g.DrawString("EAGLE QUEST", glowFont, glowBrush, (W - 500) / 2 + 3, 272);
            }
            // Main title
            using (Font titleFont = new Font("Georgia", 40, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(220, 185, 80)))
            {
                string title = "EAGLE QUEST";
                SizeF ts = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, titleBrush, (W - ts.Width) / 2, 268);
            }
            // Subtitle
            using (Font subFont = new Font("Georgia", 18, FontStyle.Italic))
            using (SolidBrush subBrush = new SolidBrush(Color.FromArgb(200, 175, 130)))
            {
                string sub = "Nest Rescue";
                SizeF ss = g.MeasureString(sub, subFont);
                g.DrawString(sub, subFont, subBrush, (W - ss.Width) / 2, 320);
            }
        }

        // ── EVENT HANDLERS ────────────────────────────────
        private void PlayButtonSound()
        {
            // Simple inline button click sound — no SoundManager needed here
            try
            {
                var sp = new System.Media.SoundPlayer(EagleQuest.Properties.Resources.bottom_click);
                sp.Play();
            }
            catch { }
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            animTimer.Stop();
            GameForm gameForm = new GameForm();
            gameForm.Show();
            this.Hide();
            gameForm.FormClosed += (s, args) => this.Close();
        }

        private void BtnHowToPlay_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            HowToPlayForm htpForm = new HowToPlayForm();
            htpForm.ShowDialog(this);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            PlayButtonSound();
            Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { animTimer?.Stop(); animTimer?.Dispose(); }
            base.Dispose(disposing);
        }
    }
}

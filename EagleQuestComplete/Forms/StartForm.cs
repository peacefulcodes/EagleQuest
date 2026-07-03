using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EagleQuest.Forms
{
    // The first screen the player sees.
    // Warm amber sunrise sky, animated clouds, mountain silhouettes,
    // and a golden adventure-style title.

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
            this.BackColor       = Color.FromArgb(30, 30, 20); // dark fallback behind image
            this.DoubleBuffered  = true;

            // Load the startup artwork from Properties.Resources — same 3:2 ratio as form
            this.BackgroundImage       = EagleQuest.Properties.Resources.startup_eagle_background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void InitializeControls()
        {
            int formW   = 900;
            int btnW    = 240;
            int centerX = (formW - btnW) / 2;  // centers button

            // ── PLAY BUTTON (large, prominent, golden) ────────
            btnPlay = new Button();
            btnPlay.Text      = "▶   PLAY NOW";
            btnPlay.Size      = new Size(btnW, 56);
            btnPlay.Location  = new Point(centerX, 390);
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.FlatAppearance.BorderColor = Color.FromArgb(255, 220, 80);
            btnPlay.FlatAppearance.BorderSize  = 2;
            btnPlay.BackColor  = Color.FromArgb(210, 140, 20);
            btnPlay.ForeColor  = Color.FromArgb(255, 255, 220);
            btnPlay.Font       = new Font("Georgia", 16, FontStyle.Bold);
            btnPlay.Cursor     = Cursors.Hand;
            btnPlay.Click     += BtnPlay_Click;

            // Hover effects
            btnPlay.MouseEnter += (s, e) => btnPlay.BackColor = Color.FromArgb(240, 170, 30);
            btnPlay.MouseLeave += (s, e) => btnPlay.BackColor = Color.FromArgb(210, 140, 20);

            // ── HOW TO PLAY & EXIT (smaller, aligned side by side) ──
            int smallW = 110;
            int gap    = 14;
            int totalSmall = smallW * 2 + gap;
            int smallStartX = (formW - totalSmall) / 2;

            btnHowToPlay = new Button();
            btnHowToPlay.Text      = "How To Play";
            btnHowToPlay.Size      = new Size(smallW, 38);
            btnHowToPlay.Location  = new Point(smallStartX, 462);
            btnHowToPlay.FlatStyle = FlatStyle.Flat;
            btnHowToPlay.FlatAppearance.BorderColor = Color.FromArgb(190, 145, 50);
            btnHowToPlay.FlatAppearance.BorderSize  = 1;
            btnHowToPlay.BackColor = Color.FromArgb(100, 70, 10);
            btnHowToPlay.ForeColor = Color.FromArgb(255, 235, 170);
            btnHowToPlay.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
            btnHowToPlay.Cursor    = Cursors.Hand;
            btnHowToPlay.Click    += BtnHowToPlay_Click;

            btnHowToPlay.MouseEnter += (s, e) => btnHowToPlay.BackColor = Color.FromArgb(130, 95, 20);
            btnHowToPlay.MouseLeave += (s, e) => btnHowToPlay.BackColor = Color.FromArgb(100, 70, 10);

            btnExit = new Button();
            btnExit.Text      = "Exit";
            btnExit.Size      = new Size(smallW, 38);
            btnExit.Location  = new Point(smallStartX + smallW + gap, 462);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(160, 80, 50);
            btnExit.FlatAppearance.BorderSize  = 1;
            btnExit.BackColor = Color.FromArgb(90, 45, 15);
            btnExit.ForeColor = Color.FromArgb(255, 200, 160);
            btnExit.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
            btnExit.Cursor    = Cursors.Hand;
            btnExit.Click    += BtnExit_Click;

            btnExit.MouseEnter += (s, e) => btnExit.BackColor = Color.FromArgb(120, 60, 20);
            btnExit.MouseLeave += (s, e) => btnExit.BackColor = Color.FromArgb(90, 45, 15);

            this.Controls.AddRange(new Control[] { btnPlay, btnHowToPlay, btnExit });
        }

        private void InitializeAnimation()
        {
            // Cloud animation no longer needed — background is now a resource image.
            animTimer          = new System.Windows.Forms.Timer();
            animTimer.Interval = 50;
            animTimer.Tick    += AnimTimer_Tick;
            // Not started — static image background needs no animation timer
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            // No longer used — kept for compile safety
        }

        // OnPaint draws foreground overlays on top of the resource image background.
        // The background image (startup_eagle_background) is set as this.BackgroundImage
        // and is stretched to fill the form automatically — no GDI+ sky/mountain painting needed.
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int W = this.ClientSize.Width;
            int H = this.ClientSize.Height;

            // ── TITLE TEXT (drawn over the image) ─────────
            DrawTitle(g, W);

            // ── TAGLINE ───────────────────────────────────
            using (Font subFont = new Font("Georgia", 13, FontStyle.Italic))
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
            using (SolidBrush subBrush    = new SolidBrush(Color.FromArgb(255, 220, 150, 20)))
            {
                string sub = "\"Fly. Collect. Return. Save the nest.\"";
                SizeF subSize = g.MeasureString(sub, subFont);
                float sx = (W - subSize.Width) / 2;
                // Subtle shadow for readability over the image
                g.DrawString(sub, subFont, shadowBrush, sx + 2, 357);
                g.DrawString(sub, subFont, subBrush,    sx,     355);
            }

            // ── TRANSLUCENT PANEL BEHIND BUTTONS ─────────
            using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(120, 10, 6, 0)))
            {
                g.FillRoundedRect(panelBrush, (W - 280) / 2 - 20, 378, 320, 132, 14);
            }
        }

        private void DrawSun(Graphics g, int cx, int cy)
        {
            // Outer glow
            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(50, 255, 230, 0)))
                g.FillEllipse(glowBrush, cx - 50, cy - 50, 160, 160);
            // Main sun disc
            using (SolidBrush sunBrush = new SolidBrush(Color.FromArgb(255, 225, 20)))
                g.FillEllipse(sunBrush, cx - 30, cy - 30, 110, 110);
        }

        private void DrawStartClouds(Graphics g, int W)
        {
            // Three warm-tinted cloud layers drifting at different speeds
            DrawCloud(g, (int)(80  + cloudOffset * 0.4f), 70,  190, 52, Color.FromArgb(160, 255, 250, 230));
            DrawCloud(g, (int)(380 + cloudOffset * 0.6f), 48,  150, 40, Color.FromArgb(140, 255, 245, 210));
            DrawCloud(g, (int)(650 + cloudOffset * 0.25f),100, 110, 32, Color.FromArgb(120, 255, 248, 220));
        }

        private void DrawCloud(Graphics g, int x, int y, int w, int h, Color color)
        {
            using (SolidBrush b = new SolidBrush(color))
            {
                g.FillEllipse(b, x, y, w, h);
                g.FillEllipse(b, x + w / 4, y - h / 3, w * 2 / 3, h);
                g.FillEllipse(b, x + w / 2, y + h / 6, w / 2, h * 2 / 3);
            }
        }

        private void DrawMountainSilhouettes(Graphics g, int W, int H)
        {
            // Deep green mountain silhouettes — warm adventure feel
            Color mtnColor = Color.FromArgb(45, 95, 40);
            Color mtnDark  = Color.FromArgb(30, 72, 28);
            Point[][] mountains =
            {
                new Point[]{ new Point(-10,H), new Point(95, 248), new Point(200,H) },
                new Point[]{ new Point(65, H), new Point(215,225), new Point(365,H) },
                new Point[]{ new Point(210,H), new Point(352,195), new Point(494,H) },
                new Point[]{ new Point(415,H), new Point(548,192), new Point(681,H) },
                new Point[]{ new Point(600,H), new Point(735,205), new Point(870,H) },
                new Point[]{ new Point(790,H), new Point(928,202), new Point(W+10,H) },
            };
            Color[] colors = { mtnColor, mtnDark, mtnColor, mtnDark, mtnColor, mtnDark };
            for (int i = 0; i < mountains.Length; i++)
            {
                using (SolidBrush mtnBrush = new SolidBrush(colors[i]))
                    g.FillPolygon(mtnBrush, mountains[i]);
            }
        }

        private void DrawTreeSilhouettes(Graphics g, int W, int H)
        {
            Color darkTree = Color.FromArgb(25, 60, 15);
            int[] treeX = { 12, 48, 830, 868 };
            foreach (int tx in treeX)
            {
                DrawTree(g, tx, H - 72, darkTree);
            }
        }

        private void DrawTree(Graphics g, int x, int baseY, Color color)
        {
            using (SolidBrush b = new SolidBrush(color))
            {
                // Trunk
                g.FillRectangle(b, x + 4, baseY - 50, 10, 50);
                // Foliage layers
                g.FillPolygon(b, new[] {
                    new Point(x,    baseY - 50),
                    new Point(x+9,  baseY - 90),
                    new Point(x+18, baseY - 50) });
                g.FillPolygon(b, new[] {
                    new Point(x-5,  baseY - 72),
                    new Point(x+9,  baseY - 145),
                    new Point(x+23, baseY - 72) });
            }
        }

        private void DrawTitle(Graphics g, int W)
        {
            // Drop shadow
            using (Font titleFont = new Font("Georgia", 42, FontStyle.Bold))
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, 60, 30, 0)))
            {
                string title = "EAGLE QUEST";
                SizeF ts = g.MeasureString(title, titleFont);
                float tx = (W - ts.Width) / 2;
                g.DrawString(title, titleFont, shadowBrush, tx + 3, 253);
            }

            // Main title — deep warm brown with golden outline feel
            using (Font titleFont = new Font("Georgia", 42, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(215, 145, 20)))
            {
                string title = "EAGLE QUEST";
                SizeF ts = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, titleBrush, (W - ts.Width) / 2, 250);
            }

            // Subtitle — "Nest Rescue"
            using (Font subFont = new Font("Georgia", 20, FontStyle.Italic))
            using (SolidBrush subBrush = new SolidBrush(Color.FromArgb(180, 100, 45, 5)))
            {
                string sub = "Nest Rescue";
                SizeF ss = g.MeasureString(sub, subFont);
                g.DrawString(sub, subFont, subBrush, (W - ss.Width) / 2, 305);
            }
        }

        // ── EVENT HANDLERS ────────────────────────────────
        private void PlayButtonSound()
        {
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

    // ── GRAPHICS EXTENSION HELPER ────────────────────────
    // Allows drawing rounded rectangles for the button panel background
    internal static class GraphicsExtensions
    {
        public static void FillRoundedRect(this Graphics g, Brush brush,
            float x, float y, float w, float h, float radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                float d = radius * 2;
                path.AddArc(x, y, d, d, 180, 90);
                path.AddArc(x + w - d, y, d, d, 270, 90);
                path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
                path.AddArc(x, y + h - d, d, d, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }
    }
}

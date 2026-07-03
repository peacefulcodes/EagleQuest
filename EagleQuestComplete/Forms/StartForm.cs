using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using R = EagleQuest.Properties.Resources;

namespace EagleQuest.Forms
{
    // START FORM — Visual Redesign
    // Background: poster image from Properties.Resources.startup_eagle_background
    // Buttons: custom PNG assets with hover swap, using PictureBox
    // Title: already in poster image — NOT redrawn here
    // Tagline: drawn over image with strong contrast

    public class StartForm : Form
    {
        private PictureBox pbPlay;
        private PictureBox pbHowTo;
        private PictureBox pbExit;

        public StartForm()
        {
            InitializeForm();
            BuildButtons();
        }

        private void InitializeForm()
        {
            this.Text            = "Eagle Quest: Nest Rescue";
            this.Size            = new Size(900, 600);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = Color.FromArgb(18, 12, 5);
            this.DoubleBuffered  = true;
            // Poster — already contains "EAGLE QUEST: NEST RESCUE" title art
            this.BackgroundImage       = EagleQuest.Properties.Resources.startup_eagle_background;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        // ── IMAGE BUTTON FACTORY ─────────────────────────────
        private PictureBox MakeImgBtn(Image normal, Image hover, int x, int y, EventHandler onClick)
        {
            var pb = new PictureBox();
            pb.Image    = normal;
            pb.Size     = new Size(normal.Width, normal.Height);
            pb.Location = new Point(x, y);
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.BackColor= Color.Transparent;
            pb.Cursor   = Cursors.Hand;
            pb.Click   += (s, e) => { PlayClick(); onClick(s, e); };
            pb.MouseEnter += (s, e) => pb.Image = hover;
            pb.MouseLeave += (s, e) => pb.Image = normal;
            return pb;
        }

        private void BuildButtons()
        {
            int formW   = 900;
            int btnPlayW = R.btn_play_n.Width;          // 260
            int playX   = (formW - btnPlayW) / 2;       // centred

            pbPlay = MakeImgBtn(R.btn_play_n, R.btn_play_h, playX, 398, BtnPlay_Click);

            // Secondary buttons — side by side
            int smW   = R.btn_sm_n.Width;               // 130
            int gap   = 16;
            int total = smW * 2 + gap;
            int smX   = (formW - total) / 2;

            pbHowTo = MakeImgBtn(R.btn_sm_n,   R.btn_sm_h,   smX,         472, BtnHowTo_Click);
            pbExit  = MakeImgBtn(R.btn_exit_n, R.btn_exit_h, smX+smW+gap, 472, BtnExit_Click);

            this.Controls.AddRange(new Control[] { pbPlay, pbHowTo, pbExit });
        }

        // ── PAINT: tagline + button panel shadow ─────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = this.ClientSize.Width;

            // Dark vignette panel behind buttons
            using (var brush = new SolidBrush(Color.FromArgb(148, 6, 3, 0)))
                FillRoundedRect(g, brush, (W - 308) / 2f - 14, 382, 336, 152, 14);

            // Tagline — warm ivory-gold, drop shadow for contrast over poster
            using (var fnt  = new Font("Georgia", 13, FontStyle.Italic))
            using (var shdw = new SolidBrush(Color.FromArgb(210, 0, 0, 0)))
            using (var fill = new SolidBrush(Color.FromArgb(255, 240, 210, 118)))
            {
                string txt = "\"Fly. Collect. Return. Save the nest.\"";
                SizeF  sz  = g.MeasureString(txt, fnt);
                float  tx  = (W - sz.Width) / 2;
                g.DrawString(txt, fnt, shdw, tx + 2, 352f);
                g.DrawString(txt, fnt, fill, tx,     350f);
            }
        }

        private static void FillRoundedRect(Graphics g, Brush b, float x, float y, float w, float h, float r)
        {
            using (var path = new GraphicsPath())
            {
                float d = r * 2;
                path.AddArc(x, y, d, d, 180, 90);
                path.AddArc(x+w-d, y, d, d, 270, 90);
                path.AddArc(x+w-d, y+h-d, d, d, 0, 90);
                path.AddArc(x, y+h-d, d, d, 90, 90);
                path.CloseFigure();
                g.FillPath(b, path);
            }
        }

        private void PlayClick()
        {
            try { new System.Media.SoundPlayer(EagleQuest.Properties.Resources.bottom_click).Play(); }
            catch { }
        }

        private void BtnPlay_Click(object s, EventArgs e)
        {
            var gf = new GameForm();
            gf.Show();
            this.Hide();
            gf.FormClosed += (_, __) => this.Close();
        }

        private void BtnHowTo_Click(object s, EventArgs e)
        {
            new HowToPlayForm().ShowDialog(this);
        }

        private void BtnExit_Click(object s, EventArgs e)
        {
            Application.Exit();
        }
    }

    // ── ROUNDED RECT HELPER (kept for OtherForms compatibility) ──
    internal static class GraphicsExtensions
    {
        public static void FillRoundedRect(this Graphics g, Brush brush,
            float x, float y, float w, float h, float radius)
        {
            using (var path = new GraphicsPath())
            {
                float d = radius * 2;
                path.AddArc(x, y, d, d, 180, 90);
                path.AddArc(x+w-d, y, d, d, 270, 90);
                path.AddArc(x+w-d, y+h-d, d, d, 0, 90);
                path.AddArc(x, y+h-d, d, d, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using R = EagleQuest.Properties.Resources;

namespace EagleQuest.Forms
{
    // ════════════════════════════════════════════════════════════
    // LEVEL COMPLETE FORM
    // ════════════════════════════════════════════════════════════
    public class LevelCompleteForm : Form
    {
        public bool PlayerChoseNext { get; private set; }

        private int completedLevel;
        private int currentScore;

        public LevelCompleteForm(int level, int score)
        {
            completedLevel = level;
            currentScore   = score;
            PlayerChoseNext = false;
            InitializeForm();
        }

        private static Label MakeLabel(string t, Font f, Color c, int x, int y, int w, int h) => FormHelpers.MakeLabel(t,f,c,x,y,w,h);
        private static PictureBox MakeImgBtn(Image n, Image h2, int x, int y, EventHandler e2) => FormHelpers.MakeImgBtn(n,h2,x,y,e2);

        private void InitializeForm()
        {
            this.Text            = "Level Complete!";
            this.Size            = new Size(480, 320);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.DoubleBuffered  = true;
            // Background image
            this.BackgroundImage       = EagleQuest.Properties.Resources.lvlcomp_bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;


            Label lblTitle = MakeLabel("✓  Level " + completedLevel + " Complete!",
                new Font("Georgia", 19, FontStyle.Bold), Color.Gold, 20, 18, 440, 44);

            Label lblScore = MakeLabel("Score:  " + currentScore.ToString("N0"),
                new Font("Segoe UI", 13, FontStyle.Bold), Color.White, 20, 75, 440, 32);

            Label lblNext = MakeLabel("Get ready for the next challenge!",
                new Font("Segoe UI", 10, FontStyle.Italic), Color.FromArgb(180, 220, 180), 20, 112, 440, 26);

            // Image buttons
            PictureBox pbNext = MakeImgBtn(R.btn_next_n, R.btn_next_h, (480-R.btn_next_n.Width)/2, 210,
                (s,e) => { PlayerChoseNext = true; this.Close(); });
            PictureBox pbQuit = MakeImgBtn(R.btn_quit_n, R.btn_quit_h, (480-R.btn_quit_n.Width)/2 + 8, 215,
                (s,e) => { this.Close(); });

            // Position side by side
            int totalW = R.btn_next_n.Width + 16 + R.btn_quit_n.Width;
            int startX = (480 - totalW) / 2;
            pbNext.Location = new Point(startX, 222);
            pbQuit.Location = new Point(startX + R.btn_next_n.Width + 16, 222);

            this.Controls.AddRange(new Control[] { lblTitle, lblScore, lblNext, pbNext, pbQuit });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Gradient overlay on top third
            using (var bg = new LinearGradientBrush(
                new Point(0,0), new Point(0, this.Height),
                Color.FromArgb(160, 8, 16, 30), Color.FromArgb(0, 0, 0, 0)))
                e.Graphics.FillRectangle(bg, 0, 0, this.Width, this.Height/2);
        }
    }


    // ════════════════════════════════════════════════════════════
    // GAME OVER FORM
    // ════════════════════════════════════════════════════════════
    public class GameOverForm : Form
    {
        private static Label MakeLabel(string t, Font f, Color c, int x, int y, int w, int h) => FormHelpers.MakeLabel(t,f,c,x,y,w,h);
        private static PictureBox MakeImgBtn(Image n, Image h2, int x, int y, EventHandler e2) => FormHelpers.MakeImgBtn(n,h2,x,y,e2);
        public bool PlayerChoseRestart { get; private set; }

        private int finalScore;
        private int highScore;

        public GameOverForm(int score, int high)
        {
            finalScore = score;
            highScore  = high;
            PlayerChoseRestart = false;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text            = "Eagle Quest — Game Over";
            this.Size            = new Size(520, 380);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.DoubleBuffered  = true;
            // Stormy background
            this.BackgroundImage       = EagleQuest.Properties.Resources.gameover_bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;


            Label lblTitle = MakeLabel("The Baby is Hungry...",
                new Font("Georgia", 19, FontStyle.Bold), Color.FromArgb(220, 60, 60), 20, 22, 480, 44);
            Label lblSub = MakeLabel("The eagle could not make it in time.",
                new Font("Segoe UI", 10, FontStyle.Italic), Color.FromArgb(195, 160, 160), 20, 72, 480, 26);
            Label lblScore = MakeLabel("Score:  " + finalScore.ToString("N0"),
                new Font("Segoe UI", 14, FontStyle.Bold), Color.White, 20, 115, 480, 34);
            Label lblHigh = MakeLabel("Best:  " + highScore.ToString("N0"),
                new Font("Segoe UI", 11), Color.Gold, 20, 152, 480, 28);

            int totalW = R.btn_retry_n.Width + 16 + R.btn_quit_n.Width;
            int bx = (520 - totalW) / 2;

            PictureBox pbRetry = MakeImgBtn(R.btn_retry_n, R.btn_retry_h, bx, 265,
                (s,e) => { PlayerChoseRestart = true; this.Close(); });
            PictureBox pbQuit  = MakeImgBtn(R.btn_quit_n,  R.btn_quit_h,  bx + R.btn_retry_n.Width + 16, 265,
                (s,e) => { this.Close(); });

            this.Controls.AddRange(new Control[] { lblTitle, lblSub, lblScore, lblHigh, pbRetry, pbQuit });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var ov = new LinearGradientBrush(
                new Point(0,0), new Point(0, 200),
                Color.FromArgb(170, 4, 2, 8), Color.FromArgb(0, 0, 0, 0)))
                e.Graphics.FillRectangle(ov, 0, 0, this.Width, 200);
        }
    }


    // ════════════════════════════════════════════════════════════
    // WIN FORM
    // ════════════════════════════════════════════════════════════
    public class WinForm : Form
    {
        private static Label MakeLabel(string t, Font f, Color c, int x, int y, int w, int h) => FormHelpers.MakeLabel(t,f,c,x,y,w,h);
        private static PictureBox MakeImgBtn(Image n, Image h2, int x, int y, EventHandler e2) => FormHelpers.MakeImgBtn(n,h2,x,y,e2);
        private int finalScore;
        private int highScore;

        public WinForm(int score, int high)
        {
            finalScore = score;
            highScore  = high;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text            = "Eagle Quest — Victory!";
            this.Size            = new Size(540, 400);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.DoubleBuffered  = true;
            // Warm sunrise background
            this.BackgroundImage       = EagleQuest.Properties.Resources.win_bg;
            this.BackgroundImageLayout = ImageLayout.Stretch;


            Label lblTitle = MakeLabel("🏆  The Baby is Fed!",
                new Font("Georgia", 22, FontStyle.Bold), Color.Gold, 20, 22, 500, 50);
            Label lblSub = MakeLabel("The eagle survived all three levels. The nest is safe.",
                new Font("Segoe UI", 11, FontStyle.Italic), Color.FromArgb(180, 230, 180), 20, 82, 500, 52);
            Label lblScore = MakeLabel("Final Score:  " + finalScore.ToString("N0"),
                new Font("Segoe UI", 15, FontStyle.Bold), Color.White, 20, 148, 500, 36);
            Label lblHigh = MakeLabel("Best:  " + highScore.ToString("N0"),
                new Font("Segoe UI", 12), Color.Gold, 20, 190, 500, 30);

            PictureBox pbClose = MakeImgBtn(R.btn_close_n, R.btn_close_h,
                (540 - R.btn_close_n.Width) / 2, 282, (s,e) => this.Close());

            this.Controls.AddRange(new Control[] { lblTitle, lblSub, lblScore, lblHigh, pbClose });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var ov = new LinearGradientBrush(
                new Point(0,0), new Point(0, 230),
                Color.FromArgb(150, 10, 20, 8), Color.FromArgb(0, 0, 0, 0)))
                e.Graphics.FillRectangle(ov, 0, 0, this.Width, 230);
        }
    }


    // ════════════════════════════════════════════════════════════
    // HOW TO PLAY FORM — Full Two-Column Redesign
    // ════════════════════════════════════════════════════════════
    public class HowToPlayForm : Form
    {
        private static PictureBox MakeImgBtn(Image n, Image h2, int x, int y, EventHandler e2)
            => FormHelpers.MakeImgBtn(n, h2, x, y, e2);

        private Panel scrollPanel;
        private Panel contentPanel;
        private Panel footerPanel;
        private PictureBox pbClose;

        public HowToPlayForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text            = "How To Play — Eagle Quest";
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = Color.FromArgb(32, 18, 6);
            this.DoubleBuffered  = true;

            Rectangle working = Screen.PrimaryScreen.WorkingArea;
            int safeWidth  = Math.Min(1180, Math.Max(900, working.Width  - 80));
            int safeHeight = Math.Min(800,  Math.Max(600, working.Height - 100));
            this.ClientSize = new Size(safeWidth, safeHeight);

            var pbHeader = new PictureBox();
            pbHeader.Image     = R.howto_header;
            pbHeader.Dock      = DockStyle.Top;
            pbHeader.Height    = 125;
            pbHeader.SizeMode  = PictureBoxSizeMode.StretchImage;
            pbHeader.BackColor = Color.FromArgb(35, 18, 5);

            footerPanel = new Panel();
            footerPanel.Dock      = DockStyle.Bottom;
            footerPanel.Height    = 72;
            footerPanel.BackColor = Color.FromArgb(26, 13, 4);
            footerPanel.Paint    += PaintFooter;

            pbClose = MakeImgBtn(R.btn_back_n, R.btn_back_h, 0, 10, (s, e) => this.Close());
            footerPanel.Controls.Add(pbClose);
            footerPanel.Resize += (s, e) =>
            {
                pbClose.Left = (footerPanel.ClientSize.Width - pbClose.Width) / 2;
                pbClose.Top  = (footerPanel.ClientSize.Height - pbClose.Height) / 2;
            };

            scrollPanel = new Panel();
            scrollPanel.Dock       = DockStyle.Fill;
            scrollPanel.AutoScroll = true;
            scrollPanel.BackColor  = Color.FromArgb(32, 18, 6);
            scrollPanel.Padding    = new Padding(14);

            int contentWidth = safeWidth - 55;
            int gap = 14;
            int columnWidth = (contentWidth - gap) / 2;
            int columnHeight = 735;

            contentPanel = new Panel();
            contentPanel.Location  = new Point(0, 0);
            contentPanel.Size      = new Size(contentWidth, columnHeight + 24);
            contentPanel.BackColor = Color.FromArgb(32, 18, 6);

            var leftPanel = new Panel();
            leftPanel.Location  = new Point(0, 10);
            leftPanel.Size      = new Size(columnWidth, columnHeight);
            leftPanel.BackColor = Color.FromArgb(44, 26, 8);
            leftPanel.Paint    += PaintPanel;
            BuildLeftColumn(leftPanel);

            var rightPanel = new Panel();
            rightPanel.Location  = new Point(columnWidth + gap, 10);
            rightPanel.Size      = new Size(columnWidth, columnHeight);
            rightPanel.BackColor = Color.FromArgb(44, 26, 8);
            rightPanel.Paint    += PaintPanel;
            BuildRightColumn(rightPanel);

            contentPanel.Controls.Add(leftPanel);
            contentPanel.Controls.Add(rightPanel);
            scrollPanel.Controls.Add(contentPanel);
            scrollPanel.AutoScrollMinSize = new Size(contentWidth, columnHeight + 45);

            this.Controls.Add(scrollPanel);
            this.Controls.Add(footerPanel);
            this.Controls.Add(pbHeader);
        }

        private void PaintFooter(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(155, 112, 25), 1))
                e.Graphics.DrawLine(pen, 0, 0, footerPanel.Width, 0);
        }

        private void PaintPanel(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            using (var pen = new Pen(Color.FromArgb(175, 130, 30), 1))
                e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
        }

        private void BuildLeftColumn(Panel p)
        {
            int y = 12;

            AddSectionHead(p, "CONTROLS", ref y);
            AddIconRow(p, R.key_arrows, "Arrow Keys — Move the eagle", ref y, 62, 36);
            AddIconRow(p, R.key_space, "SPACE — Fire feather (Level 3 only)", ref y, 80, 28);
            AddSmallNote(p, "Short cooldown — continuous rapid fire is prevented", ref y);

            y += 10;
            AddSectionHead(p, "OBJECTIVE", ref y);
            AddIconRow(p, R.icon_food, "Collect all required prey", ref y);
            AddIconRow(p, R.icon_nest, "Return to the baby nest", ref y);
            AddIconRow(p, R.icon_timer, "Beat the hunger timer", ref y);

            y += 10;
            AddSectionHead(p, "LEVELS", ref y);
            AddTextLine(p, "Level 1 — 3 prey   |   60 seconds", ref y, Color.FromArgb(220, 190, 130));
            AddTextLine(p, "Level 2 — 5 prey   |   50 seconds", ref y, Color.FromArgb(220, 190, 130));
            AddTextLine(p, "Level 3 — 7 prey   |   45 seconds", ref y, Color.FromArgb(220, 190, 130));

            y += 10;
            AddSectionHead(p, "POWER-UPS", ref y);
            AddIconRow(p, R.icon_shield, "Shield — Absorbs one hit", ref y);
            AddIconRow(p, R.icon_speed, "Speed Boost — Move faster", ref y);
        }

        private void BuildRightColumn(Panel p)
        {
            int y = 12;

            AddSectionHead(p, "ENEMIES & HAZARDS", ref y);
            AddIconRow(p, R.icon_crow, "Crow — Patrols left and right", ref y);
            AddIconRow(p, R.icon_hawk, "Hawk — Dives toward the eagle", ref y);
            AddIconRow(p, R.icon_plane, "Plane — Fires bullets downward", ref y);
            AddSmallNote(p, "Rocks, mountain peaks and storm clouds are hazards", ref y);

            y += 10;
            AddSectionHead(p, "FEATHER ATTACK — LEVEL 3", ref y);
            AddIconRow(p, R.icon_feather, "Press SPACE to fire a feather", ref y);
            AddSmallNote(p, "Each feather hits one enemy and then disappears", ref y);

            y += 6;
            AddIconRow(p, R.icon_crow, "Crow — 1 feather hit", ref y);
            AddIconRow(p, R.icon_hawk, "Hawk — 2 feather hits", ref y);
            AddIconRow(p, R.icon_plane, "Plane — 3 feather hits", ref y);

            y += 10;
            AddSectionHead(p, "TIPS", ref y);
            AddTextLine(p, "• Grab the shield before approaching enemies", ref y, Color.FromArgb(215, 180, 120));
            AddTextLine(p, "• Fly carefully through mountain gaps in Level 1", ref y, Color.FromArgb(215, 180, 120));
            AddTextLine(p, "• When the timer turns red, return to the nest quickly", ref y, Color.FromArgb(215, 180, 120));
            AddTextLine(p, "• Your score carries across all levels", ref y, Color.FromArgb(215, 180, 120));
        }

        private void AddSectionHead(Panel p, string text, ref int y)
        {
            var lbl = new Label();
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(255, 210, 70);
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize  = false;
            lbl.Size      = new Size(p.Width - 18, 24);
            lbl.Location  = new Point(9, y);
            p.Controls.Add(lbl);

            var underline = new Panel();
            underline.BackColor = Color.FromArgb(160, 115, 25);
            underline.Size      = new Size(p.Width - 18, 1);
            underline.Location  = new Point(9, y + 22);
            p.Controls.Add(underline);
            y += 31;
        }

        private void AddIconRow(Panel p, Image icon, string text, ref int y, int iW = 36, int iH = 36)
        {
            int rowH = Math.Max(iH, 24) + 6;

            var pb = new PictureBox();
            pb.Image     = icon;
            pb.Size      = new Size(iW, iH);
            pb.Location  = new Point(10, y + (rowH - iH) / 2);
            pb.SizeMode  = PictureBoxSizeMode.Zoom;
            pb.BackColor = Color.Transparent;
            p.Controls.Add(pb);

            var lbl = new Label();
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 9.5f);
            lbl.ForeColor = Color.FromArgb(238, 218, 182);
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize  = false;
            lbl.Size      = new Size(p.Width - iW - 28, rowH);
            lbl.Location  = new Point(iW + 18, y);
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            p.Controls.Add(lbl);

            y += rowH + 3;
        }

        private void AddSmallNote(Panel p, string text, ref int y)
        {
            var lbl = new Label();
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 8.5f, FontStyle.Italic);
            lbl.ForeColor = Color.FromArgb(175, 153, 114);
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize  = false;
            lbl.Size      = new Size(p.Width - 20, 34);
            lbl.Location  = new Point(10, y);
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            p.Controls.Add(lbl);
            y += 36;
        }

        private void AddTextLine(Panel p, string text, ref int y, Color col)
        {
            var lbl = new Label();
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 9.4f);
            lbl.ForeColor = col;
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize  = false;
            lbl.Size      = new Size(p.Width - 20, 28);
            lbl.Location  = new Point(10, y);
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            p.Controls.Add(lbl);
            y += 30;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var bg = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, this.Height),
                Color.FromArgb(42, 22, 7),
                Color.FromArgb(22, 10, 3)))
            {
                e.Graphics.FillRectangle(bg, this.ClientRectangle);
            }
        }
    }


    // ════════════════════════════════════════════════════════════
    // SHARED HELPERS
    // ════════════════════════════════════════════════════════════
    static class FormHelpers
    {
        public static Label MakeLabel(string text, Font font, Color col, int x, int y, int w, int h)
        {
            var lbl = new Label();
            lbl.Text      = text;
            lbl.Font      = font;
            lbl.ForeColor = col;
            lbl.BackColor = Color.Transparent;
            lbl.Size      = new Size(w, h);
            lbl.Location  = new Point(x, y);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            return lbl;
        }

        public static PictureBox MakeImgBtn(Image normal, Image hover, int x, int y, EventHandler onClick)
        {
            var pb = new PictureBox();
            pb.Image    = normal;
            pb.Size     = new Size(normal.Width, normal.Height);
            pb.Location = new Point(x, y);
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.BackColor= Color.Transparent;
            pb.Cursor   = Cursors.Hand;
            pb.Click   += onClick;
            pb.MouseEnter += (s, e) => pb.Image = hover;
            pb.MouseLeave += (s, e) => pb.Image = normal;
            return pb;
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EagleQuest.Forms
{
    // ════════════════════════════════════════════════════
    // LEVEL COMPLETE FORM
    // ════════════════════════════════════════════════════
    public class LevelCompleteForm : Form
    {
        public bool PlayerChoseNext { get; private set; }
        private int completedLevel;
        private int currentScore;

        public LevelCompleteForm(int level, int score)
        {
            completedLevel   = level;
            currentScore     = score;
            PlayerChoseNext  = false;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text            = "Level Complete!";
            this.Size            = new Size(480, 320);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Color.FromArgb(10, 20, 45);
            this.DoubleBuffered  = true;

            // Title label
            Label lblTitle = new Label();
            lblTitle.Text      = "✓  Level " + completedLevel + " Complete!";
            lblTitle.Font      = new Font("Georgia", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.Size      = new Size(440, 45);
            lblTitle.Location  = new Point(20, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Score label
            Label lblScore = new Label();
            lblScore.Text      = "Score: " + currentScore.ToString("N0");
            lblScore.Font      = new Font("Segoe UI", 14);
            lblScore.ForeColor = Color.White;
            lblScore.Size      = new Size(440, 35);
            lblScore.Location  = new Point(20, 90);
            lblScore.TextAlign = ContentAlignment.MiddleCenter;

            // Next level info
            Label lblNext = new Label();
            lblNext.Text      = "Level " + (completedLevel + 1) + " awaits...";
            lblNext.Font      = new Font("Segoe UI", 11, FontStyle.Italic);
            lblNext.ForeColor = Color.FromArgb(180, 200, 255);
            lblNext.Size      = new Size(440, 30);
            lblNext.Location  = new Point(20, 135);
            lblNext.TextAlign = ContentAlignment.MiddleCenter;

            // Next Level button
            Button btnNext = MakeButton("Next Level  →", 110, 195, Color.FromArgb(40, 120, 200));
            btnNext.Click += (s, e) => { PlayerChoseNext = true; this.Close(); };

            // Quit button
            Button btnQuit = MakeButton("Quit", 260, 195, Color.FromArgb(80, 40, 40));
            btnQuit.Click += (s, e) => { PlayerChoseNext = false; this.Close(); };

            this.Controls.AddRange(new Control[] {
                lblTitle, lblScore, lblNext, btnNext, btnQuit
            });
        }

        private Button MakeButton(string text, int x, int y, Color backColor)
        {
            Button btn = new Button();
            btn.Text      = text;
            btn.Size      = new Size(130, 42);
            btn.Location  = new Point(x, y);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 150);
            btn.FlatAppearance.BorderSize  = 1;
            btn.BackColor  = backColor;
            btn.ForeColor  = Color.White;
            btn.Font       = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.Cursor     = Cursors.Hand;
            return btn;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (LinearGradientBrush bg = new LinearGradientBrush(
                new Point(0, 0), new Point(0, this.Height),
                Color.FromArgb(10, 20, 45), Color.FromArgb(20, 40, 80)))
                e.Graphics.FillRectangle(bg, this.ClientRectangle);
        }
    }


    // ════════════════════════════════════════════════════
    // GAME OVER FORM
    // ════════════════════════════════════════════════════
    public class GameOverForm : Form
    {
        public bool PlayerChoseRestart { get; private set; }
        private int finalScore;
        private int highScore;

        public GameOverForm(int score, int high)
        {
            finalScore           = score;
            highScore            = high;
            PlayerChoseRestart   = false;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text            = "Game Over";
            this.Size            = new Size(480, 340);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Color.FromArgb(25, 8, 8);
            this.DoubleBuffered  = true;

            Label lblTitle = new Label();
            lblTitle.Text      = "The Baby is Hungry...";
            lblTitle.Font      = new Font("Georgia", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(220, 60, 60);
            lblTitle.Size      = new Size(440, 45);
            lblTitle.Location  = new Point(20, 25);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            Label lblSub = new Label();
            lblSub.Text      = "The eagle did not return in time.";
            lblSub.Font      = new Font("Segoe UI", 11, FontStyle.Italic);
            lblSub.ForeColor = Color.FromArgb(200, 160, 160);
            lblSub.Size      = new Size(440, 28);
            lblSub.Location  = new Point(20, 78);
            lblSub.TextAlign = ContentAlignment.MiddleCenter;

            Label lblScore = new Label();
            lblScore.Text      = "Final Score: " + finalScore.ToString("N0");
            lblScore.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            lblScore.ForeColor = Color.White;
            lblScore.Size      = new Size(440, 30);
            lblScore.Location  = new Point(20, 118);
            lblScore.TextAlign = ContentAlignment.MiddleCenter;

            Label lblHigh = new Label();
            lblHigh.Text      = "Best Score: " + highScore.ToString("N0");
            lblHigh.Font      = new Font("Segoe UI", 11);
            lblHigh.ForeColor = Color.Gold;
            lblHigh.Size      = new Size(440, 28);
            lblHigh.Location  = new Point(20, 150);
            lblHigh.TextAlign = ContentAlignment.MiddleCenter;

            Button btnRestart = MakeButton("Try Again", 100, 215, Color.FromArgb(150, 60, 20));
            btnRestart.Click += (s, e) => { PlayerChoseRestart = true; this.Close(); };

            Button btnQuit = MakeButton("Quit", 260, 215, Color.FromArgb(60, 20, 20));
            btnQuit.Click += (s, e) => { PlayerChoseRestart = false; this.Close(); };

            this.Controls.AddRange(new Control[] {
                lblTitle, lblSub, lblScore, lblHigh, btnRestart, btnQuit
            });
        }

        private Button MakeButton(string text, int x, int y, Color backColor)
        {
            Button btn = new Button();
            btn.Text      = text;
            btn.Size      = new Size(130, 42);
            btn.Location  = new Point(x, y);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(120, 80, 80);
            btn.FlatAppearance.BorderSize  = 1;
            btn.BackColor  = backColor;
            btn.ForeColor  = Color.White;
            btn.Font       = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.Cursor     = Cursors.Hand;
            return btn;
        }
    }


    // ════════════════════════════════════════════════════
    // WIN FORM
    // ════════════════════════════════════════════════════
    public class WinForm : Form
    {
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
            this.Text            = "Victory!";
            this.Size            = new Size(520, 380);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Color.FromArgb(8, 20, 10);
            this.DoubleBuffered  = true;

            Label lblTitle = new Label();
            lblTitle.Text      = "🏆  The Baby is Fed!";
            lblTitle.Font      = new Font("Georgia", 22, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.Size      = new Size(480, 50);
            lblTitle.Location  = new Point(20, 25);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            Label lblSub = new Label();
            lblSub.Text      = "The eagle survived all three levels.\nThe nest is safe.";
            lblSub.Font      = new Font("Segoe UI", 12, FontStyle.Italic);
            lblSub.ForeColor = Color.FromArgb(180, 230, 180);
            lblSub.Size      = new Size(480, 55);
            lblSub.Location  = new Point(20, 85);
            lblSub.TextAlign = ContentAlignment.MiddleCenter;

            Label lblScore = new Label();
            lblScore.Text      = "Final Score: " + finalScore.ToString("N0");
            lblScore.Font      = new Font("Segoe UI", 15, FontStyle.Bold);
            lblScore.ForeColor = Color.White;
            lblScore.Size      = new Size(480, 35);
            lblScore.Location  = new Point(20, 150);
            lblScore.TextAlign = ContentAlignment.MiddleCenter;

            Label lblHigh = new Label();
            lblHigh.Text      = "High Score: " + highScore.ToString("N0");
            lblHigh.Font      = new Font("Segoe UI", 12);
            lblHigh.ForeColor = Color.Gold;
            lblHigh.Size      = new Size(480, 30);
            lblHigh.Location  = new Point(20, 190);
            lblHigh.TextAlign = ContentAlignment.MiddleCenter;

            Button btnClose = new Button();
            btnClose.Text      = "Close";
            btnClose.Size      = new Size(160, 45);
            btnClose.Location  = new Point(180, 270);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderColor = Color.Gold;
            btnClose.FlatAppearance.BorderSize  = 1;
            btnClose.BackColor  = Color.FromArgb(60, 100, 30);
            btnClose.ForeColor  = Color.White;
            btnClose.Font       = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClose.Cursor     = Cursors.Hand;
            btnClose.Click     += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblTitle, lblSub, lblScore, lblHigh, btnClose
            });
        }
    }


    // ════════════════════════════════════════════════════
    // HOW TO PLAY FORM
    // ════════════════════════════════════════════════════
    public class HowToPlayForm : Form
    {
        public HowToPlayForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text            = "How To Play";
            this.Size            = new Size(520, 480);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Color.FromArgb(10, 15, 35);
            this.DoubleBuffered  = true;

            Label lblTitle = new Label();
            lblTitle.Text      = "How To Play";
            lblTitle.Font      = new Font("Georgia", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.Size      = new Size(480, 40);
            lblTitle.Location  = new Point(20, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            RichTextBox rtbInstructions = new RichTextBox();
            rtbInstructions.Size      = new Size(460, 340);
            rtbInstructions.Location  = new Point(25, 65);
            rtbInstructions.BackColor = Color.FromArgb(15, 22, 48);
            rtbInstructions.ForeColor = Color.FromArgb(220, 220, 220);
            rtbInstructions.Font      = new Font("Segoe UI", 11);
            rtbInstructions.ReadOnly  = true;
            rtbInstructions.BorderStyle = BorderStyle.None;
            rtbInstructions.Text =
                "STORY\r\n" +
                "Your baby eagle is hungry in the nest.\r\n" +
                "Fly, collect food, and return before the hunger\r\n" +
                "timer runs out — feed your baby!\r\n\r\n" +
                "CONTROLS\r\n" +
                "  ↑  ↓  ←  →    Move the eagle\r\n" +
                "  SPACE          Fire feather (Level 3 only)\r\n\r\n" +
                "OBJECTIVE\r\n" +
                "  Level 1 — Collect 5 prey, return to nest  (60s)\r\n" +
                "  Level 2 — Collect 5 prey, return to nest  (50s)\r\n" +
                "  Level 3 — Collect 7 prey, return to nest  (45s)\r\n\r\n" +
                "HOW TO WIN A LEVEL\r\n" +
                "  Collect all required food items first.\r\n" +
                "  Then fly to the nest on the right side!\r\n\r\n" +
                "ENEMIES\r\n" +
                "  Crow         — Flies left and right\r\n" +
                "  Hawk         — Dives toward you\r\n" +
                "  Military Plane — Fires bullets! (Level 3)\r\n\r\n" +
                "TIPS\r\n" +
                "  Blue shield powerup = brief invincibility\r\n" +
                "  Yellow bolt powerup = faster movement\r\n" +
                "  Avoid storm clouds — they slow you down\r\n" +
                "  Timer flashing red = danger zone!";

            Button btnClose = new Button();
            btnClose.Text      = "Got it!";
            btnClose.Size      = new Size(140, 40);
            btnClose.Location  = new Point(185, 420);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderColor = Color.Gold;
            btnClose.FlatAppearance.BorderSize  = 1;
            btnClose.BackColor  = Color.FromArgb(80, 65, 10);
            btnClose.ForeColor  = Color.White;
            btnClose.Font       = new Font("Segoe UI", 11, FontStyle.Bold);
            btnClose.Cursor     = Cursors.Hand;
            btnClose.Click     += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTitle, rtbInstructions, btnClose });
        }
    }
}

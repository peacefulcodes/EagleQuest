using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EagleQuest.Enums;
using EagleQuest.Game;

namespace EagleQuest.Forms
{
    // VIVA: GameForm is the main game window.
    // It does NOT contain game logic — it just:
    //   1. Holds the Timer (100ms game loop)
    //   2. Passes keyboard input to game.KeyDown/KeyUp
    //   3. Calls game.Update() every tick
    //   4. Draws the animated background
    //   5. Opens correct form when game state changes

    public class GameForm : Form
    {
        private System.Windows.Forms.Timer gameLoop;
        private EagleQuest.Game.Game game;

        // HUD controls
        private Label lblLives;
        private Label lblScore;
        private Label lblLevel;
        private Label lblTimer;
        private Label lblNestHint;
        private ProgressBar pbFood;
        private ProgressBar pbHunger;

        // Animated cloud positions
        private float cx1 = 0f, cx2 = 320f, cx3 = 640f;

        public GameForm()
        {
            InitializeForm();
            InitializeHUD();
            InitializeGame();
            InitializeGameLoop();
        }

        private void InitializeForm()
        {
            this.Text            = "Eagle Quest: Nest Rescue";
            this.ClientSize      = new Size(1000, 650);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = Color.Black;
            this.DoubleBuffered  = true;
            this.KeyPreview      = true;
        }

        private void InitializeHUD()
        {
            int W = 1000;

            // ── TOP HUD (y 0–55) ──────────────────────────
            lblLives = MakeLbl("♥ ♥ ♥", 12, 8, 170, 32,
                Color.FromArgb(220, 60, 60), new Font("Segoe UI", 16, FontStyle.Bold));

            Label cScore = MakeLbl("SCORE", 260, 4, 90, 16,
                Color.FromArgb(160,160,160), new Font("Segoe UI", 8));
            lblScore = MakeLbl("0", 260, 20, 120, 28,
                Color.White, new Font("Segoe UI", 14, FontStyle.Bold));

            Label cLevel = MakeLbl("LEVEL", 420, 4, 80, 16,
                Color.FromArgb(160,160,160), new Font("Segoe UI", 8));
            lblLevel = MakeLbl("1 / 3", 420, 20, 80, 28,
                Color.Gold, new Font("Segoe UI", 14, FontStyle.Bold));

            Label cTimer = MakeLbl("TIME", 870, 4, 80, 16,
                Color.FromArgb(160,160,160), new Font("Segoe UI", 8));
            lblTimer = MakeLbl("1:00", 858, 20, 110, 28,
                Color.LimeGreen, new Font("Segoe UI", 14, FontStyle.Bold));

            // ── BOTTOM HUD (y 602–650) ────────────────────
            Label cFood = MakeLbl("PREY", 10, 608, 50, 16,
                Color.FromArgb(200,200,200), new Font("Segoe UI", 8));
            pbFood          = new ProgressBar();
            pbFood.Location = new Point(62, 608);
            pbFood.Size     = new Size(170, 16);
            pbFood.Minimum  = 0; pbFood.Maximum = 3; pbFood.Value = 0;
            pbFood.Style    = ProgressBarStyle.Continuous;

            Label cHunger = MakeLbl("HUNGER", 255, 608, 65, 16,
                Color.FromArgb(200,200,200), new Font("Segoe UI", 8));
            pbHunger          = new ProgressBar();
            pbHunger.Location = new Point(322, 608);
            pbHunger.Size     = new Size(170, 16);
            pbHunger.Minimum  = 0; pbHunger.Maximum = 100; pbHunger.Value = 100;
            pbHunger.Style    = ProgressBarStyle.Continuous;

            // Nest hint — right side of bottom bar
            lblNestHint = MakeLbl("Collect prey, then return to nest!", 510, 604, 480, 36,
                Color.FromArgb(255, 220, 80), new Font("Segoe UI", 9, FontStyle.Bold));

            this.Controls.AddRange(new Control[] {
                lblLives, cScore, lblScore, cLevel, lblLevel, cTimer, lblTimer,
                cFood, pbFood, cHunger, pbHunger, lblNestHint
            });
        }

        private Label MakeLbl(string text, int x, int y, int w, int h,
                               Color color, Font font)
        {
            Label l = new Label();
            l.Text = text; l.Location = new Point(x,y);
            l.Size = new Size(w,h); l.ForeColor = color;
            l.BackColor = Color.Transparent; l.Font = font;
            return l;
        }

        private void InitializeGame()
        {
            game = new EagleQuest.Game.Game(
                this, lblLives, lblScore, lblLevel,
                lblTimer, lblNestHint, pbFood, pbHunger);

            // ── LOAD IMAGES FROM Properties.Resources ────────
            // VIVA: "Images embedded in Properties.Resources are loaded here
            // and passed to Game class. GameForm handles resources;
            // Game class handles game logic — single responsibility."

            // Eagle direction fix:
            // The original sprite faces LEFT (head on left side).
            // eagle_right_frame = original = faces LEFT
            // eagle_left_frame  = flipped  = faces RIGHT
            // So to get correct direction: swap the assignments.
            game.EagleRightFrame0 = EagleQuest.Properties.Resources.eagle_left_frame0;
            game.EagleRightFrame1 = EagleQuest.Properties.Resources.eagle_left_frame1;
            game.EagleRightFrame2 = EagleQuest.Properties.Resources.eagle_left_frame2;
            game.EagleLeftFrame0  = EagleQuest.Properties.Resources.eagle_right_frame0;
            game.EagleLeftFrame1  = EagleQuest.Properties.Resources.eagle_right_frame1;
            game.EagleLeftFrame2  = EagleQuest.Properties.Resources.eagle_right_frame2;

            // Crow animation frames
            game.CrowLeftFrame0  = EagleQuest.Properties.Resources.crow_1;
            game.CrowLeftFrame1  = EagleQuest.Properties.Resources.crow_2;
            game.CrowRightFrame0 = EagleQuest.Properties.Resources.crow_right_1;
            game.CrowRightFrame1 = EagleQuest.Properties.Resources.crow_right_2;

            // Hawk animation frames
            game.HawkLeft1  = EagleQuest.Properties.Resources.hawk_1;
            game.HawkLeft2  = EagleQuest.Properties.Resources.hawk_2;
            game.HawkRight1 = EagleQuest.Properties.Resources.hawk_right_1;
            game.HawkRight2 = EagleQuest.Properties.Resources.hawk_right_2;
            game.HawkImage  = EagleQuest.Properties.Resources.hawk_fixed;
            game.PreyImage      = EagleQuest.Properties.Resources.prey_icon;
            game.RockImage      = EagleQuest.Properties.Resources.rock;
            game.CloudDarkImage = EagleQuest.Properties.Resources.dark_cloud_sprite;
            game.PlaneImage     = EagleQuest.Properties.Resources.plane;
            game.FeatherImage   = EagleQuest.Properties.Resources.feather;
            game.ShieldImage    = EagleQuest.Properties.Resources.shield;
            game.SpeedImage     = EagleQuest.Properties.Resources.speed;

            game.Start();
            BringHUDToFront();
        }

        private void BringHUDToFront()
        {
            foreach (Control c in this.Controls)
                if (c is Label || c is ProgressBar)
                    c.BringToFront();
        }

        private void InitializeGameLoop()
        {
            gameLoop          = new System.Windows.Forms.Timer();
            gameLoop.Interval = 100; // 100ms = teacher's style game loop
            gameLoop.Tick    += GameLoop_Tick;
            gameLoop.Start();
        }

        // ── GAME LOOP TICK ────────────────────────────────
        private void GameLoop_Tick(object sender, EventArgs e)
        {
            game.Update();
            AnimateClouds();
            this.Invalidate(); // repaint background
            BringHUDToFront();

            // Handle state transitions
            GameState state = game.State;

            if (state == GameState.LevelComplete)
            {
                gameLoop.Stop(); // pause timer during dialog
                LevelCompleteForm lc = new LevelCompleteForm(
                    game.LevelManager.CurrentLevel,
                    game.ScoreManager.Score);
                lc.ShowDialog(this);
                if (lc.PlayerChoseNext)
                {
                    game.StartNextLevel();
                    gameLoop.Start();
                }
                else this.Close();
            }
            else if (state == GameState.GameOver)
            {
                gameLoop.Stop();
                GameOverForm go = new GameOverForm(
                    game.ScoreManager.Score,
                    game.ScoreManager.HighScore);
                go.ShowDialog(this);
                if (go.PlayerChoseRestart)
                {
                    game.Restart();
                    gameLoop.Start();
                }
                else this.Close();
            }
            else if (state == GameState.Win)
            {
                gameLoop.Stop();
                WinForm win = new WinForm(
                    game.ScoreManager.Score,
                    game.ScoreManager.HighScore);
                win.ShowDialog(this);
                this.Close();
            }
        }

        private void AnimateClouds()
        {
            int W = ClientSize.Width;
            cx1 -= 0.9f; if (cx1 < -220) cx1 = W + 60;
            cx2 -= 0.6f; if (cx2 < -220) cx2 = W + 60;
            cx3 -= 1.1f; if (cx3 < -220) cx3 = W + 60;
        }

        // ── BACKGROUND PAINTING ───────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int lv = (game != null) ? game.LevelManager.CurrentLevel : 1;
            if      (lv == 1) PaintLevel1(g);
            else if (lv == 2) PaintLevel2(g);
            else              PaintLevel3(g);
        }

        // Level 1 — warm amber sunrise
        private void PaintLevel1(Graphics g)
        {
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var sky = new LinearGradientBrush(Pt(0,0), Pt(0,H),
                Clr(255,154,60), Clr(135,206,235))) g.FillRectangle(sky,0,0,W,H);

            using (var sg = new SolidBrush(Clr(40,255,215,0))) g.FillEllipse(sg, W-148,26,132,132);
            using (var sun = new SolidBrush(Clr(255,215,0)))   g.FillEllipse(sun, W-130,43, 94, 94);

            Cloud(g,(int)cx1,       82, 170,48, Clr(200,255,255,255));
            Cloud(g,(int)cx2,       60, 130,36, Clr(180,255,248,220));
            Cloud(g,(int)cx3 - 180,108,  96,28, Clr(160,255,250,230));

            Mtn(g,-10,H, 95,198,Clr(120,160,85));
            Mtn(g, 65,H,215,172,Clr(100,140,70));
            Mtn(g,210,H,352,150,Clr(85,120,55));
            Mtn(g,415,H,548,145,Clr(75,110,48));
            Mtn(g,600,H,735,160,Clr(90,130,60));
            Mtn(g,790,H,928,156,Clr(80,118,52));

            using (var gr = new LinearGradientBrush(Pt(0,H-88),Pt(0,H),
                Clr(74,140,55),Clr(45,90,30))) g.FillRectangle(gr,0,H-88,W,88);
            using (var gp = new SolidBrush(Clr(90,170,60))) g.FillRectangle(gp,0,H-88,W,7);

            Tree(g, 30,H-70,Clr(93,58,26),Clr(45,106,30));
            Tree(g,120,H-70,Clr(93,58,26),Clr(38,92,24));
            Tree(g,870,H-70,Clr(93,58,26),Clr(45,106,30));
            Tree(g,960,H-70,Clr(93,58,26),Clr(38,92,24));
            HUDBars(g,W,H);
        }

        // Level 2 — moonlit night
        private void PaintLevel2(Graphics g)
        {
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var sky = new LinearGradientBrush(Pt(0,0),Pt(0,H),
                Clr(10,22,40),Clr(26,62,130))) g.FillRectangle(sky,0,0,W,H);

            var rng = new Random(99);
            for (int i=0;i<120;i++){
                int sx=rng.Next(0,W),sy=rng.Next(0,H/2),a=rng.Next(100,240);
                using (var sb=new SolidBrush(Clr(a,255,255,255))) g.FillEllipse(sb,sx,sy,2,2);
            }
            using (var mg=new SolidBrush(Clr(30,212,200,160))) g.FillEllipse(mg,598,20,156,156);
            using (var m=new SolidBrush(Clr(212,200,168)))     g.FillEllipse(m, 618,36,110,110);
            using (var ms=new SolidBrush(Clr(26,62,130)))      g.FillEllipse(ms,648,24,110,110);

            Cloud(g,(int)cx1,     112,160,57,Clr(130,50,50,70));
            Cloud(g,(int)cx2+110,  82,190,67,Clr(150,42,42,62));

            SnowMtn(g,-10,H,105,173,Clr(70,90,110),Color.White);
            SnowMtn(g, 85,H,238,130,Clr(60,80,100),Color.White);
            SnowMtn(g,265,H,415,108,Clr(50,70,95), Color.White);
            SnowMtn(g,450,H,558,125,Clr(55,75,100),Color.White);
            SnowMtn(g,630,H,762,140,Clr(60,80,105),Color.White);
            SnowMtn(g,820,H,958,158,Clr(65,85,108),Color.White);
            HUDBars(g,W,H);
        }

        // Level 3 — storm
        private void PaintLevel3(Graphics g)
        {
            int W = ClientSize.Width, H = ClientSize.Height;
            using (var sky = new LinearGradientBrush(Pt(0,0),Pt(0,H),
                Clr(13,0,32),Clr(45,16,64))) g.FillRectangle(sky,0,0,W,H);

            Cloud(g,  0, 52,260, 82,Clr(200,42,32,56));
            Cloud(g,230, 43,290, 92,Clr(220,50,38,68));
            Cloud(g,620, 58,230, 76,Clr(200,42,32,56));
            Cloud(g,(int)cx1+100,70,210,72,Clr(180,38,28,52));

            using (var rp=new Pen(Clr(55,136,170,204),1)){
                var rng=new Random(77);
                for(int i=0;i<50;i++){
                    int rx=rng.Next(0,W);
                    int ry=(int)((rng.Next(0,H)+cx1*3)%H);
                    if(ry<0)ry+=H;
                    g.DrawLine(rp,rx,ry,rx-5,ry+28);
                }
            }
            if((int)(Math.Abs(cx1)/10)%15==0)
            {
                using(var lp=new Pen(Clr(180,200,160,255),3))
                { g.DrawLine(lp,228,94,218,138); g.DrawLine(lp,218,138,228,138); g.DrawLine(lp,228,138,212,184); }
            }
            Mtn(g,-10,H,125,182,Clr(15,10,28)); Mtn(g,105,H,270,155,Clr(12,8,24));
            Mtn(g,290,H,450,148,Clr(14,9,26));  Mtn(g,530,H,700,165,Clr(13,8,25));
            Mtn(g,760,H,935,172,Clr(15,10,28));
            HUDBars(g,W,H);
        }

        // ── DRAWING HELPERS ───────────────────────────────
        private void Cloud(Graphics g,int x,int y,int w,int h,Color c)
        {
            using(var b=new SolidBrush(c)){
                g.FillEllipse(b,x,y,w,h);
                g.FillEllipse(b,x+w/4,y-h/3,w*2/3,h);
                g.FillEllipse(b,x+w/2,y+h/6,w/2,h*2/3);
            }
        }
        private void Mtn(Graphics g,int bL,int bY,int pX,int pY,Color c)
        {
            using(var b=new SolidBrush(c))
                g.FillPolygon(b,new[]{Pt(bL,bY),Pt(pX,pY),Pt(pX*2-bL,bY)});
        }
        private void SnowMtn(Graphics g,int bL,int bY,int pX,int pY,Color mc,Color sc)
        {
            Mtn(g,bL,bY,pX,pY,mc);
            int sl=pY+(bY-pY)/3, hw=sl-pY;
            using(var sb=new SolidBrush(Color.FromArgb(220,sc)))
                g.FillPolygon(sb,new[]{Pt(pX-hw,sl),Pt(pX,pY),Pt(pX+hw,sl)});
        }
        private void Tree(Graphics g,int x,int bY,Color tc,Color lc)
        {
            using(var t=new SolidBrush(tc)) g.FillRectangle(t,x+4,bY-52,10,52);
            using(var l=new SolidBrush(lc)){
                g.FillPolygon(l,new[]{Pt(x,bY-52),Pt(x+9,bY-95),Pt(x+18,bY-52)});
                g.FillPolygon(l,new[]{Pt(x-5,bY-74),Pt(x+9,bY-152),Pt(x+23,bY-74)});
            }
        }
        private void HUDBars(Graphics g,int W,int H)
        {
            using(var t=new SolidBrush(Color.FromArgb(180,0,0,0))) g.FillRectangle(t,0,0,W,56);
            using(var b=new SolidBrush(Color.FromArgb(180,0,0,0))) g.FillRectangle(b,0,H-46,W,46);
        }
        private static Color Clr(int a,int r,int g,int b) => Color.FromArgb(a,r,g,b);
        private static Color Clr(int r,int g,int b)       => Color.FromArgb(r,g,b);
        private static Point Pt(int x,int y)               => new Point(x,y);

        // ── KEYBOARD ─────────────────────────────────────
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (game != null) game.KeyDown(e.KeyCode);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (game != null) game.KeyUp(e.KeyCode);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys k)
        {
            // WM_KEYDOWN = 0x100, WM_SYSKEYDOWN = 0x104
            // WM_KEYUP   = 0x101, WM_SYSKEYUP   = 0x105
            if (k == Keys.Up || k == Keys.Down ||
                k == Keys.Left || k == Keys.Right || k == Keys.Space)
            {
                const int WM_KEYDOWN    = 0x100;
                const int WM_SYSKEYDOWN = 0x104;
                const int WM_KEYUP      = 0x101;
                const int WM_SYSKEYUP   = 0x105;

                if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
                {
                    if (game != null) game.KeyDown(k);
                }
                else if (msg.Msg == WM_KEYUP || msg.Msg == WM_SYSKEYUP)
                {
                    if (game != null) game.KeyUp(k);
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, k);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing){ gameLoop?.Stop(); gameLoop?.Dispose(); }
            base.Dispose(disposing);
        }
    }
}

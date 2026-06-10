using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EagleQuest.Enums;
using EagleQuest.GameObjects;
using EagleQuest.Managers;

namespace EagleQuest.Game
{
    // VIVA: Game is the central controller class.
    // The Form does NOT contain game logic — it just calls game.Update() every tick.
    // Game CONTAINS (HAS-A) all managers and game objects — this is COMPOSITION.
    // This is the most important class for your class diagram.

    public class Game
    {
        // COMPOSITION: Game has-a all of these
        private Player player;
        private BabyNest babyNest;
        private List<GameObject> gameObjects;
        private InputManager inputManager;
        private CollisionManager collisionManager;
        private ScoreManager scoreManager;
        private LevelManager levelManager;
        private SoundManager soundManager;

        private GameState gameState;
        private Form gameForm;
        private int formWidth;
        private int formHeight;

        // Play area: between top HUD and bottom HUD
        private const int PLAY_TOP    = 58;
        private int playBottom;

        // Hit flash timer (counts up during invincibility blink)
        private int hitFlashTimer = 0;

        // Shooting
        private int shootCooldown = 0;
        private const int SHOOT_MAX = 12;

        // HUD references
        private Label livesLabel;
        private Label scoreLabel;
        private Label levelLabel;
        private Label timerLabel;
        private Label nestHintLabel;
        private ProgressBar foodBar;
        private ProgressBar hungerBar;

        // ── IMAGE RESOURCES ────────────────────────────────
        // Set from GameForm using Properties.Resources
        // VIVA: GameForm loads images, Game uses them — separation of concerns
        public Image EagleRightFrame0 { get; set; }
        public Image EagleRightFrame1 { get; set; }
        public Image EagleRightFrame2 { get; set; }
        public Image EagleLeftFrame0  { get; set; }
        public Image EagleLeftFrame1  { get; set; }
        public Image EagleLeftFrame2  { get; set; }
        public Image CrowLeftFrame0   { get; set; }   // crow facing left, frame 0
        public Image CrowLeftFrame1   { get; set; }   // crow facing left, frame 1
        public Image CrowRightFrame0  { get; set; }   // crow facing right, frame 0
        public Image CrowRightFrame1  { get; set; }   // crow facing right, frame 1
        public Image HawkLeft1        { get; set; }   // hawk left wing frame 1
        public Image HawkLeft2        { get; set; }   // hawk left wing frame 2
        public Image HawkRight1       { get; set; }   // hawk right wing frame 1
        public Image HawkRight2       { get; set; }   // hawk right wing frame 2
        public Image HawkImage        { get; set; }
        public Image PreyImage        { get; set; }  // prey/food collectible
        public Image RockImage        { get; set; }
        public Image CloudDarkImage   { get; set; }
        public Image PlaneImage       { get; set; }
        public Image FeatherImage     { get; set; }
        public Image ShieldImage      { get; set; }
        public Image SpeedImage       { get; set; }

        public GameState State           { get { return gameState;    } }
        public ScoreManager ScoreManager { get { return scoreManager; } }
        public LevelManager LevelManager { get { return levelManager; } }
        public Player Player             { get { return player;       } }

        public Game(Form form, Label lives, Label score, Label level,
                    Label timer, Label nestHint, ProgressBar food, ProgressBar hunger)
        {
            gameForm      = form;
            formWidth     = form.ClientSize.Width;
            formHeight    = form.ClientSize.Height;
            playBottom    = formHeight - 48;

            livesLabel    = lives;
            scoreLabel    = score;
            levelLabel    = level;
            timerLabel    = timer;
            nestHintLabel = nestHint;
            foodBar       = food;
            hungerBar     = hunger;

            gameObjects      = new List<GameObject>();
            inputManager     = new InputManager();
            collisionManager = new CollisionManager();
            scoreManager     = new ScoreManager();
            levelManager     = new LevelManager();
            soundManager     = new SoundManager();

            gameState     = GameState.Playing;
        }

        // Called once when game starts — creates player, nest, level objects
        public void Start()
        {
            CreatePlayer();
            CreateNest();
            LoadLevelObjects(levelManager.CurrentLevel);
        }

        // ── MAIN GAME LOOP ─────────────────────────────────
        // Called every 100ms from GameForm.GameLoop_Tick
        // VIVA: Input → Update → Collisions → State check
        public void Update()
        {
            if (gameState != GameState.Playing) return;

            // Snapshot values BEFORE collisions to detect changes
            int foodBefore  = player.FoodCollected;
            int livesBefore = player.Lives;

            HandleInput();
            UpdateAllObjects();
            SpawnPlaneBullets();
            collisionManager.CheckAll(gameObjects);
            RemoveDeadObjects();
            HandleHitFlash();

            // Play sounds based on what changed this tick
            if (player.FoodCollected > foodBefore)
                soundManager.PlayCollect();   // food was just collected

            if (player.Lives < livesBefore)
                soundManager.PlayHit();       // life was just lost

            levelManager.Tick();    // decrement timer
            UpdateHUD();            // show updated timer
            CheckGameState();       // AFTER HUD update — so displayed time = actual time
        }

        // ── INPUT ──────────────────────────────────────────
        private void HandleInput()
        {
            if (inputManager.MoveUp())    player.MoveUp();
            if (inputManager.MoveDown())  player.MoveDown();
            if (inputManager.MoveLeft())  player.MoveLeft();
            if (inputManager.MoveRight()) player.MoveRight();

            // Shooting: Level 3 only
            if (levelManager.CurrentLevel == 3 && inputManager.Fire())
            {
                if (shootCooldown <= 0)
                {
                    FireFeather();
                    shootCooldown = SHOOT_MAX;
                }
            }
            if (shootCooldown > 0) shootCooldown--;
        }

        // ── OBJECT CREATION ────────────────────────────────
        private void CreatePlayer()
        {
            PictureBox pb = new PictureBox();
            pb.Size      = new Size(72, 56);
            pb.Left      = formWidth / 2 - 36;
            pb.Top       = (PLAY_TOP + playBottom) / 2 - 28;
            pb.BackColor = Color.Transparent;
            pb.SizeMode  = PictureBoxSizeMode.StretchImage;
            pb.Image     = EagleRightFrame0;
            gameForm.Controls.Add(pb);
            pb.BringToFront();

            player = new Player(pb, formWidth, formHeight, PLAY_TOP, playBottom);

            // VIVA: EagleAnimator shows COMPOSITION inside Player.
            // Player HAS-A EagleAnimator. 6 frames — 3 right, 3 left.
            EagleAnimator animator = new EagleAnimator(
                EagleRightFrame0, EagleRightFrame1, EagleRightFrame2,
                EagleLeftFrame0,  EagleLeftFrame1,  EagleLeftFrame2,
                3);
            player.SetAnimator(animator);
            gameObjects.Add(player);
        }

        private void CreateNest()
        {
            // Nest at far right of play area, vertically centered
            int nestX = formWidth - 100;
            int nestY = (PLAY_TOP + playBottom) / 2 - 35;

            PictureBox pb = new PictureBox();
            pb.Size      = new Size(80, 65);
            pb.Left      = nestX;
            pb.Top       = nestY;
            pb.BackColor = Color.Transparent;
            pb.SizeMode  = PictureBoxSizeMode.StretchImage;
            gameForm.Controls.Add(pb);
            pb.BringToFront();

            babyNest = new BabyNest(pb); // draws itself
            gameObjects.Add(babyNest);
        }

        private void LoadLevelObjects(int level)
        {
            ClearLevelObjects();
            if (babyNest != null) babyNest.Reset();

            // Safe spawn zone — away from HUD and nest
            int sL = 50;
            int sR = formWidth - 130; // stay away from nest on right
            int sT = PLAY_TOP + 25;
            int sB = playBottom - 80;

            if (level == 1)
            {
                CreateCrow(130, sT + 50);
                CreateCrow(440, sT + 90);
                CreateRock(300, sT + 130);
                CreateRock(540, sB - 50);
                // 5 prey items spread across play area — more active than before
                CreatePrey(130, sT + 100);
                CreatePrey(260, sT + 60);
                CreatePrey(390, sT + 150);
                CreatePrey(520, sT + 85);
                CreatePrey(650, sT + 120);
            }
            else if (level == 2)
            {
                CreateCrow(110, sT + 55);
                CreateCrow(370, sT + 105);
                CreateHawk(580, sT + 35);
                CreateRock(245, sT + 135);
                CreateRock(490, sB - 70);
                CreateStormCloud(330, sT + 65);
                // 5 prey items
                CreatePrey(150, sT + 85);
                CreatePrey(315, sT + 165);
                CreatePrey(475, sT + 75);
                CreatePrey(210, sB - 90);
                CreatePrey(540, sB - 110);
                CreatePowerUp(360, sT + 210, PowerUpType.Shield);
            }
            else if (level == 3)
            {
                CreateCrow(120, sT + 65);
                CreateCrow(410, sT + 55);
                CreateHawk(590, sT + 35);
                CreateMilitaryPlane(formWidth + 20, sT + 45);
                CreateStormCloud(215, sT + 75);
                CreateStormCloud(510, sT + 95);
                CreateRock(330, sB - 95);
                // 7 prey items
                int[] px = { 100, 210, 325, 435, 560, 175, 400 };
                int[] py = { sT+85, sT+165, sT+95, sB-125, sT+125, sB-95, sB-145 };
                for (int i = 0; i < 7; i++) CreatePrey(px[i], py[i]);
                CreatePowerUp(320, sT + 205, PowerUpType.SpeedBoost);
            }

            foodBar.Maximum = levelManager.FoodRequired;
            foodBar.Value   = 0;
            UpdateNestHint();
        }

        // ── FACTORY METHODS ────────────────────────────────
        private void CreateCrow(int x, int y)
        {
            PictureBox pb = MakePB(x, y, 55, 42, CrowLeftFrame0);
            CrowEnemy crow = new CrowEnemy(pb, formWidth, formHeight);
            crow.SetAnimationFrames(CrowLeftFrame0, CrowLeftFrame1,
                                    CrowRightFrame0, CrowRightFrame1);
            gameObjects.Add(crow);
        }

        private void CreateHawk(int x, int y)
        {
            PictureBox pb = MakePB(x, y, 70, 40, HawkLeft1 ?? HawkImage);
            HawkEnemy hawk = new HawkEnemy(pb, formWidth, formHeight, player);
            hawk.SetAnimationFrames(
                HawkLeft1 ?? HawkImage,  HawkLeft2 ?? HawkImage,
                HawkRight1 ?? HawkImage, HawkRight2 ?? HawkImage);
            gameObjects.Add(hawk);
        }

        private void CreateMilitaryPlane(int x, int y)
        {
            PictureBox pb = MakePB(x, y, 98, 50, PlaneImage);
            gameObjects.Add(new MilitaryPlane(pb, formWidth, formHeight, PlaneImage));
        }

        private void CreateRock(int x, int y)
        {
            PictureBox pb = MakePB(x, y, 55, 42, RockImage);
            gameObjects.Add(new RockObstacle(pb, formWidth, formHeight));
        }

        private void CreateStormCloud(int x, int y)
        {
            PictureBox pb = MakePB(x, y, 100, 62, CloudDarkImage);
            gameObjects.Add(new StormCloud(pb, formWidth, formHeight));
        }

        private void CreatePrey(int x, int y)
        {
            // Food uses MakePBBack (adds to back) so enemies and player appear on top naturally
            PictureBox pb = MakePBBack(x, y, 40, 40, PreyImage);
            gameObjects.Add(new FoodItem(pb, formWidth, formHeight));
        }

        private void CreatePowerUp(int x, int y, PowerUpType type)
        {
            Image img = (type == PowerUpType.Shield) ? ShieldImage : SpeedImage;
            PictureBox pb = MakePB(x, y, 40, 40, img);
            gameObjects.Add(new PowerUp(pb, formWidth, formHeight, type));
        }

        private void FireFeather()
        {
            PictureBox pb = new PictureBox();
            pb.Size      = new Size(14, 32);
            pb.Left      = player.X + player.Width / 2 - 7;
            pb.Top       = player.Y - 35;
            pb.BackColor = Color.Transparent;
            pb.SizeMode  = PictureBoxSizeMode.StretchImage;
            pb.Image     = FeatherImage;
            gameForm.Controls.Add(pb);
            pb.BringToFront();
            gameObjects.Add(new FeatherProjectile(pb));
            soundManager.PlayShoot();
        }

        private PictureBox MakePB(int x, int y, int w, int h, Image img)
        {
            PictureBox pb = new PictureBox();
            pb.Size      = new Size(w, h);
            pb.Left      = x;
            pb.Top       = y;
            pb.BackColor = Color.Transparent;
            pb.SizeMode  = PictureBoxSizeMode.StretchImage;
            pb.Image     = img;
            gameForm.Controls.Add(pb);
            pb.BringToFront();
            return pb;
        }

        // Same as MakePB but does NOT bring to front — used for food/collectibles
        // so they sit below enemies and player visually
        private PictureBox MakePBBack(int x, int y, int w, int h, Image img)
        {
            PictureBox pb = new PictureBox();
            pb.Size      = new Size(w, h);
            pb.Left      = x;
            pb.Top       = y;
            pb.BackColor = Color.Transparent;
            pb.SizeMode  = PictureBoxSizeMode.StretchImage;
            pb.Image     = img;
            gameForm.Controls.Add(pb);
            // Do NOT BringToFront — food sits behind enemies naturally
            return pb;
        }

        // ── UPDATE LOOP ────────────────────────────────────
        private void UpdateAllObjects()
        {
            // VIVA: POLYMORPHISM — same Update() call, each object behaves differently
            foreach (GameObject obj in new List<GameObject>(gameObjects))
                if (obj.IsAlive) obj.Update();
        }

        private void SpawnPlaneBullets()
        {
            foreach (GameObject obj in new List<GameObject>(gameObjects))
            {
                if (obj is MilitaryPlane && obj.IsAlive)
                {
                    MilitaryPlane plane = (MilitaryPlane)obj;
                    foreach (BulletObstacle b in plane.GetNewBullets())
                    {
                        gameForm.Controls.Add(b.Sprite);
                        b.Sprite.BringToFront();
                        gameObjects.Add(b);
                    }
                }
            }
        }

        private void RemoveDeadObjects()
        {
            List<GameObject> dead = new List<GameObject>();
            foreach (GameObject obj in gameObjects)
                if (!obj.IsAlive) dead.Add(obj);
            foreach (GameObject obj in dead)
            {
                gameForm.Controls.Remove(obj.Sprite);
                gameObjects.Remove(obj);
            }
        }

        // Blink effect during invincibility window after hit
        private void HandleHitFlash()
        {
            if (player.IsInvincible)
            {
                // Alternate between orange and transparent every 2 ticks
                if (hitFlashTimer % 2 == 0)
                    player.ShowHitFlash();
                else
                    player.ClearHitFlash();
                hitFlashTimer++;
            }
            else
            {
                // No longer invincible — clear flash
                if (hitFlashTimer > 0)
                {
                    player.ClearHitFlash();
                    hitFlashTimer = 0;
                }
            }
        }

        // ── HUD UPDATE ─────────────────────────────────────
        private void UpdateHUD()
        {
            // Lives
            string hearts = "";
            for (int i = 0; i < player.Lives; i++) hearts += "♥ ";
            for (int i = player.Lives; i < 3; i++) hearts += "♡ ";
            livesLabel.Text = hearts.Trim();

            scoreLabel.Text = scoreManager.Score.ToString("N0");
            levelLabel.Text = levelManager.CurrentLevel + " / " + levelManager.MaxLevel;

            int t = levelManager.TimeLeft;
            timerLabel.Text      = (t / 60) + ":" + (t % 60).ToString("D2");
            timerLabel.ForeColor = t <= 10 ? Color.Red : Color.LimeGreen;

            foodBar.Value = Math.Min(player.FoodCollected, levelManager.FoodRequired);

            int maxT     = levelManager.CurrentLevel == 1 ? 60
                         : levelManager.CurrentLevel == 2 ? 50 : 45;
            int hPct     = (int)((levelManager.TimeLeft / (float)maxT) * 100);
            hungerBar.Value = Math.Max(0, Math.Min(100, hPct));

            UpdateNestHint();
        }

        private void UpdateNestHint()
        {
            if (nestHintLabel == null) return;
            if (player != null && player.FoodCollected >= levelManager.FoodRequired)
                nestHintLabel.Text = "▶  All food collected! Fly to the NEST now!";
            else if (player != null)
                nestHintLabel.Text = "Prey: " + player.FoodCollected
                    + " / " + levelManager.FoodRequired
                    + "   —   Collect all prey, then return to nest";
        }

        // ── WIN / LOSS STATE CHECKS ────────────────────────
        private void CheckGameState()
        {
            // WIN: enough food collected AND eagle reached nest
            if (babyNest != null
                && babyNest.NestReached
                && player.FoodCollected >= levelManager.FoodRequired)
            {
                scoreManager.AddScore(500 + levelManager.TimeLeft * 10);

                if (levelManager.IsLastLevel())
                {
                    soundManager.PlayWin();          // final win sound
                    gameState = GameState.Win;
                }
                else
                {
                    soundManager.PlayLevelComplete(); // level complete sound
                    gameState = GameState.LevelComplete;
                }
                return;
            }

            // LOSE: time up
            if (levelManager.IsTimeUp)
            {
                soundManager.PlayGameOver();
                gameState = GameState.GameOver;
                return;
            }

            // LOSE: no lives
            if (player.Lives <= 0)
            {
                soundManager.PlayGameOver();
                gameState = GameState.GameOver;
                return;
            }
        }

        // ── TRANSITIONS ────────────────────────────────────
        public void StartNextLevel()
        {
            levelManager.LoadNextLevel();
            player.ResetForLevel();
            if (babyNest != null) babyNest.Reset();
            hitFlashTimer = 0;
            player.ClearHitFlash();
            inputManager.Reset();  // clear stuck keys between levels
            LoadLevelObjects(levelManager.CurrentLevel);
            gameState = GameState.Playing;
        }

        public void Restart()
        {
            ClearAllObjects();
            scoreManager.Reset();
            levelManager.LoadLevel(1);
            gameObjects.Clear();
            hitFlashTimer = 0;
            inputManager.Reset();  // clear stuck keys on restart
            gameState = GameState.Playing;
            Start();
        }

        public void KeyDown(Keys key) { inputManager.KeyDown(key); }
        public void KeyUp(Keys key)   { inputManager.KeyUp(key); }

        private void ClearLevelObjects()
        {
            List<GameObject> toRemove = new List<GameObject>();
            foreach (GameObject obj in gameObjects)
                if (!(obj is Player) && !(obj is BabyNest))
                    toRemove.Add(obj);
            foreach (GameObject obj in toRemove)
            {
                gameForm.Controls.Remove(obj.Sprite);
                gameObjects.Remove(obj);
            }
        }

        private void ClearAllObjects()
        {
            foreach (GameObject obj in gameObjects)
                gameForm.Controls.Remove(obj.Sprite);
            gameObjects.Clear();
        }
    }
}

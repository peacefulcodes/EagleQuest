using System;
using System.Media;

namespace EagleQuest.Managers
{
    // VIVA: SoundManager wraps all sound logic in one place.
    // Uses System.Media.SoundPlayer — built into .NET, no extra packages needed.
    // Sounds are loaded from Properties.Resources (embedded in the project).
    // This follows Single Responsibility Principle.

    public class SoundManager
    {
        private SoundPlayer collectSound;
        private SoundPlayer hitSound;
        private SoundPlayer levelCompleteSound;
        private SoundPlayer gameOverSound;
        private SoundPlayer winSound;
        private SoundPlayer buttonClickSound;

        private bool soundEnabled;

        public bool SoundEnabled
        {
            get { return soundEnabled; }
            set { soundEnabled = value; }
        }

        public SoundManager()
        {
            soundEnabled = true;
            LoadSounds();
        }

        private void LoadSounds()
        {
            // Load each sound from Properties.Resources using the UnmanagedMemoryStream
            // Properties.Resources returns a stream — SoundPlayer can play it with .Play()
            // If a resource is missing or fails, game continues without sound
            collectSound       = TryLoadFromResource(EagleQuest.Properties.Resources.collect);
            hitSound           = TryLoadFromResource(EagleQuest.Properties.Resources.hit);
            levelCompleteSound = TryLoadFromResource(EagleQuest.Properties.Resources.level_complete);
            gameOverSound      = TryLoadFromResource(EagleQuest.Properties.Resources.game_over);
            winSound           = TryLoadFromResource(EagleQuest.Properties.Resources.win);
            buttonClickSound   = TryLoadFromResource(EagleQuest.Properties.Resources.bottom_click);
        }

        private SoundPlayer TryLoadFromResource(System.IO.UnmanagedMemoryStream stream)
        {
            try
            {
                if (stream != null)
                {
                    SoundPlayer sp = new SoundPlayer(stream);
                    sp.Load(); // pre-load so Play() is instant
                    return sp;
                }
            }
            catch { }
            return null;
        }

        private void Play(SoundPlayer player)
        {
            if (!soundEnabled || player == null) return;
            try
            {
                // Use Play() not PlaySync() — non-blocking
                player.Play();
            }
            catch { }
        }

        // Called exactly when food/prey is collected by the eagle
        public void PlayCollect()      { Play(collectSound); }

        // Called exactly when eagle loses one life (not during invincibility)
        public void PlayHit()          { Play(hitSound); }

        // Called once when a level is completed
        public void PlayLevelComplete(){ Play(levelCompleteSound); }

        // Called once when game ends due to time or lives
        public void PlayGameOver()     { Play(gameOverSound); }

        // Called once on the final win (Level 3 complete)
        public void PlayWin()          { Play(winSound); }

        // Called on button clicks
        public void PlayButtonClick()  { Play(buttonClickSound); }

        // Kept for backward compatibility with existing shoot calls
        public void PlayShoot()        { /* no shoot sound resource — silent */ }
    }
}

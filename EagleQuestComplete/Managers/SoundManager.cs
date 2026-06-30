using System;
using System.Media;

namespace EagleQuest.Managers
{
    
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
            
            collectSound       = TryLoadFromResource(EagleQuest.Properties.Resources.collect);
            hitSound           = TryLoadFromResource(EagleQuest.Properties.Resources.hit);
            levelCompleteSound = TryLoadFromResource(EagleQuest.Properties.Resources.level_complete);
            gameOverSound      = TryLoadFromResource(EagleQuest.Properties.Resources.game_over);
            winSound           = TryLoadFromResource(EagleQuest.Properties.Resources.win2);
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
                
                player.Play();
            }
            catch { }
        }

      
        public void PlayCollect()      { Play(collectSound); }

       
        public void PlayHit()          { Play(hitSound); }

        // Called once when a level is completed
        public void PlayLevelComplete(){ Play(levelCompleteSound); }

        
        public void PlayGameOver()     { Play(gameOverSound); }

       
        public void PlayWin()          { Play(winSound); }

        
        public void PlayButtonClick()  { Play(buttonClickSound); }

        
        public void PlayShoot()        { }
    }
}

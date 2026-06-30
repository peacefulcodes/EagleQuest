using System.Collections.Generic;
using System.Windows.Forms;

namespace EagleQuest.Managers
{
    

    public class InputManager
    {
        
        private Dictionary<Keys, bool> pressedKeys;

        public InputManager()
        {
            pressedKeys = new Dictionary<Keys, bool>();
        }

        public void KeyDown(Keys key)
        {
            pressedKeys[key] = true;
        }

        
        public void KeyUp(Keys key)
        {
            pressedKeys[key] = false;
        }

        
        private bool IsKeyPressed(Keys key)
        {
            if (pressedKeys.ContainsKey(key))
                return pressedKeys[key];
            return false;
        }

       
        public bool MoveUp()
        {
            return IsKeyPressed(Keys.Up);
        }

        public bool MoveDown()
        {
            return IsKeyPressed(Keys.Down);
        }

        public bool MoveLeft()
        {
            return IsKeyPressed(Keys.Left);
        }

        public bool MoveRight()
        {
            return IsKeyPressed(Keys.Right);
        }

        public bool Fire()
        {
            return IsKeyPressed(Keys.Space);
        }

        public void Reset()
        {
            pressedKeys.Clear();
        }
    }
}

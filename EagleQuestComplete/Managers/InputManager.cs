using System.Collections.Generic;
using System.Windows.Forms;

namespace EagleQuest.Managers
{
    // VIVA: InputManager separates keyboard input from game logic.
    // The form just tells InputManager which keys are pressed.
    // Game class asks InputManager "is this key pressed?" — clean separation.
    // This follows Single Responsibility Principle.

    public class InputManager
    {
        // Dictionary stores which keys are currently held down
        private Dictionary<Keys, bool> pressedKeys;

        public InputManager()
        {
            pressedKeys = new Dictionary<Keys, bool>();
        }

        // Called from Form's KeyDown event
        public void KeyDown(Keys key)
        {
            pressedKeys[key] = true;
        }

        // Called from Form's KeyUp event
        public void KeyUp(Keys key)
        {
            pressedKeys[key] = false;
        }

        // Helper to safely check if a key is pressed
        private bool IsKeyPressed(Keys key)
        {
            if (pressedKeys.ContainsKey(key))
                return pressedKeys[key];
            return false;
        }

        // Clean readable methods that Game class calls
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace selfiekiller_beta
{
    public class InputSystem
    {

        #region Fields and Properties
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        //Mouse and GamePad states
        #endregion

        #region Properties
        public bool MenuUp
        {
            get { return IsNewPressedKey(Keys.Up); }
        }
        public bool MenuDown
        {
            get { return IsNewPressedKey(Keys.Down); }
        }
        public bool MenuSelect
        {
            get { return IsNewPressedKey(Keys.Enter); }
        }
        public bool MenuCancel
        {
            get { return IsNewPressedKey(Keys.Escape); }
        }
        public bool PauseGame
        {
            get { return IsNewPressedKey(Keys.Escape); }
        }
        public bool MoveUp
        {
            get { return IsPressedKey(Keys.Up); }
        }
        public bool MoveDown
        {
            get { return IsPressedKey(Keys.Down); }
        }
        public bool MenuLeft
        {
            get { return IsNewPressedKey(Keys.Left); }
        }
        public bool MenuRight
        {
            get { return IsNewPressedKey(Keys.Right); }
        }
        public bool Destroy
        {
            get { return IsPressedKey(Keys.D); }
        }
        public bool Avoid
        {
            get { return IsPressedKey(Keys.A); }
        }
        #endregion

        #region Input System Methods
        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }
        private bool IsNewPressedKey(Keys key)
        {
            return previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key);
        }
        private bool IsPressedKey(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }
        #endregion

    }
}

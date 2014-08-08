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
            get { return IsPressedKey(Keys.Up) || IsPressedKey(Keys.W); }
        }
        public bool MoveDown
        {
            get { return IsPressedKey(Keys.Down) || IsPressedKey(Keys.S); }
        }
        public bool MoveLeft
        {
            get { return IsPressedKey(Keys.Left) || IsPressedKey(Keys.A); }
        }
        public bool MoveRight
        {
            get { return IsPressedKey(Keys.Right) || IsPressedKey(Keys.D); }
        }
        public bool Reload
        {
            get { return IsPressedKey(Keys.R); }
        }
        public bool Fire
        {
            get { return IsPressedKey(Keys.Space); }
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
        /// <summary>
        /// Checks for a "menu decrement" input action, from any player,
        /// on keyboard
        /// </summary>
        public bool MenuLeft
        {
            get
            {
                return IsNewPressedKey(Keys.Left) ||
                       IsNewPressedKey(Keys.A);
            }
        }

        /// <summary>
        /// Checks for a "menu incremenet" input action, from any player,
        /// on keyboard
        /// </summary>
        public bool MenuRight
        {
            get
            {
                return IsNewPressedKey(Keys.Right) ||
                       IsNewPressedKey(Keys.D);
            }
        }
        #endregion

    }
}

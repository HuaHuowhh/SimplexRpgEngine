﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace SimplexCore
{
    public static partial class Sgml
    {
        public static MouseButtons LastButton = MouseButtons.mb_none;
        private static Vector2 _mouse = Vector2.Zero;
        private static Keys keyboardKey = Keys.None;
        private static Keys keyboardLastKey = Keys.None;
        public static string keyboard_string = "";

        public static Keys keyboard_key
        {
            get
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    keyboardKey = Keyboard.GetState().GetPressedKeys()[0];
                }
                else
                {
                    keyboardKey = Keys.None;
                }

                return keyboardKey;
            }
            set { keyboardKey = value; }
        }

        public static Keys keyboard_lastkey
        {
            get
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    if (Keyboard.GetState().GetPressedKeys()[0] != Keys.None)
                    {
                        keyboardLastKey = Keyboard.GetState().GetPressedKeys()[0];
                    }
                }

                return keyboardLastKey;
            }
            set { keyboardLastKey = value; }
        }

        public static char keyboard_lastchar
        {
            get { return (char) keyboardLastKey; }
            set { keyboardLastKey = (Keys) value; }
        }

        public static Vector2 mouse
        {
            get
            {
                _mouse = Input.MousePosition; return _mouse;
            }
            set
            {
                Input.MousePosition = value; _mouse = value;
            }
        }

        public enum MouseButtons : int
        {
            Left,
            mb_right,
            Middle,
            mb_none,
            mb_any,
            mb_x1,
            mb_x2
        }

        public static bool mouse_wheel_up()
        {
            return Input.WheelUp;
        }

        public static bool mouse_wheel_down()
        {
            return Input.WheelDown;
        }

        public static bool mouse_check_button_pressed(MouseButtons button)
        {
            return Input.PressedButtonsOnce[(int)button] == 1 && Input.ReleasedButtons[(int)button] == 0;
        }

        public static bool mouse_check_button(MouseButtons button)
        {
            return Input.PressedButtonsOnce[(int)button] == 1;
        }

        public static bool mouse_check_button_released(MouseButtons button)
        {
            return Input.ReleasedButtons[(int)button] == 1;
        }

        public static MouseButtons mouse_button()
        {
            if (Input.PressedButtons[0] == 1) { return MouseButtons.Left;}
            if (Input.PressedButtons[1] == 1) { return MouseButtons.mb_right; }
            if (Input.PressedButtons[2] == 1) { return MouseButtons.Middle; }
            if (Input.PressedButtons[3] == 1) { return MouseButtons.mb_none; }
            if (Input.PressedButtons[5] == 1) { return MouseButtons.mb_x1; }
            if (Input.PressedButtons[6] == 1) { return MouseButtons.mb_x2; }

            return MouseButtons.mb_none;
        }

        public static void mouse_clear(MouseButtons btn)
        {
            Input.PressedButtons[(int) btn] = 0;
            Input.ReleasedButtons[(int)btn] = 0;
            Input.PressedButtonsOnce[(int)btn] = 0;
        }

        public static Vector2 window_mouse_get()
        {
            return new Vector2(Cursor.Position.X, Cursor.Position.Y);
        }

        public static void window_mouse_set(Vector2 pos)
        {
            Cursor.Position = new System.Drawing.Point((int)pos.X, (int)pos.Y);
        }

        public static bool keyboard_check_pressed(Keys key)
        {
            return Input.KeyPressed(key);
        }

        public static bool keyboard_check_released(Keys key)
        {
            return Input.KeyReleased(key);
        }

        public static bool keyboard_check_direct(Keys key)
        {
            return Input.KeyboardLowLevel[(int) key] == 1;
        }

        public static void io_clear()
        {
            Input.Clear();
        }

        public static bool keyboard_check(Keys key)
        {
            return Input.KeyDown(key);
        }

        public static void keyboard_set_numlock(bool set)
        {
            if (Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock) != set)
            {
                NativeMethods.SimulateKeyPress(NativeMethods.VK_NUMLOCK);
            }
        }

        public static bool keyboard_get_numlock()
        {
            return Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock);
        }

        public static void keyboard_clear()
        {
            Input.KeyboardState = new KeyboardState();
            Input.KeyboardStatePrevious = new KeyboardState();
        }


        public static void keyboard_press(Keys key)
        {
           SendKeys.Send(Convert.ToChar((int)key).ToString());
        }

        public static void keyboard_release(Keys key)
        {
            SendKeys.Send(Convert.ToChar((int)key).ToString());
            SendKeys.Send(Convert.ToChar((int)key).ToString());
        }

    }
}

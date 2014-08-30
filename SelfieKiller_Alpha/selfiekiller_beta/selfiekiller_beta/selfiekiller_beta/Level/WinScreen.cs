using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using selfiekiller_beta;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
//using System.Windows.forms;

namespace selfiekiller_beta
{
    public class WinScreen : GameScreen
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        Texture2D winBG;
        public string stageStates;

        #region Buttonvars
        // Global variables
        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        const int NUMBER_OF_BUTTONS = 3,
            Menu_BUTTON_INDEX = 0,
            Retry_BUTTON_INDEX = 1,
            Continue_BUTTON_INDEX = 2,
            BUTTON_HEIGHT = 50,
            BUTTON_WIDTH = 150;
        Color background_color;
        Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        double[] button_timer = new double[NUMBER_OF_BUTTONS];
        //mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;
        //mouse location in window
        int mx, my;
        double frame_time;
        #endregion 

        public WinScreen(ContentManager content, string state)
        {
            this.content = content;
            state = stageStates;
        }
        

        public override void Initialize()
        {
            // starting x and y locations to stack buttons 
            // vertically in the middle of the screen
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.White;
                button_timer[i] = 0.0;
                button_rectangle[Menu_BUTTON_INDEX] = new Rectangle(80, 398, BUTTON_WIDTH, BUTTON_HEIGHT);
                button_rectangle[Retry_BUTTON_INDEX] = new Rectangle(329, 398, BUTTON_WIDTH, BUTTON_HEIGHT);
                button_rectangle[Continue_BUTTON_INDEX] = new Rectangle(570, 398, BUTTON_WIDTH, BUTTON_HEIGHT);
            }
            
            background_color = Color.CornflowerBlue;
        }

        public override void LoadContent()
        {
            spriteBatch = ScreenManager.SpriteBatch;
            winBG = content.Load<Texture2D>("Backgrounds/WinBg");

            button_texture[Menu_BUTTON_INDEX] =
                content.Load<Texture2D>("Buttons/MainMenuBlue");
            button_texture[Retry_BUTTON_INDEX] =
                content.Load<Texture2D>("Buttons/playagain");
            button_texture[Continue_BUTTON_INDEX] =
                content.Load<Texture2D>("Buttons/continue");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        // wrapper for hit_image_alpha taking Rectangle and Texture
        Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determine state and color of button
        void update_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {

                if (hit_image_alpha(
                    button_rectangle[i], button_texture[i], mx, my))
                {
                    button_timer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state[i] = BState.DOWN;
                        button_color[i] = Color.Blue;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state[i] == BState.DOWN)
                        {
                            // button i was just down
                            button_state[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        button_state[i] = BState.HOVER;
                        button_color[i] = Color.LightBlue;
                    }
                }
                else
                {
                    button_state[i] = BState.UP;
                    if (button_timer[i] > 0)
                    {
                        button_timer[i] = button_timer[i] - frame_time;
                    }
                    else
                    {
                        button_color[i] = Color.White;
                    }
                }

                if (button_state[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }
        }


        // Logic for each button click goes here
        void take_action_on_button(int i)
        {
            //take action corresponding to which button was clicked
            switch (i)
            {
                case Menu_BUTTON_INDEX:
                    ScreenManager.AddScreen(new MainMenu(content));
                    break;
                case Retry_BUTTON_INDEX:
                    switch (stageStates)
                    {
                        case "stateOne":
                            ScreenManager.AddScreen(new StageOne(content));
                            break;
                        case "stateTwo":
                            ScreenManager.AddScreen(new StageTwo(content));
                            break;
                        case "stateThree":
                            ScreenManager.AddScreen(new StageThree(content));
                            break;
                        default:
                            Trace.WriteLine("default");
                            break;
                    }
                    Trace.WriteLine(stageStates);
                    break;
                case Continue_BUTTON_INDEX:
                    switch (stageStates)
                    {
                        case "stateOne":
                            ScreenManager.AddScreen(new StageTwo(content));
                            break;
                        case "stateTwo":
                            ScreenManager.AddScreen(new StageThree(content));
                            break;
                        case "stateThree":
                            ScreenManager.AddScreen(new MainMenu(content));
                            break;
                        default:
                            Trace.WriteLine("default");
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            update_buttons();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(winBG, new Vector2(0, 0), Color.White);
            //GraphicsDevice.Clear(background_color);
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
            spriteBatch.End();
            
        }
    }
}

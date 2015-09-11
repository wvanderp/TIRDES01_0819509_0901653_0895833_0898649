using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ourGame {

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ship ship;
        List<LaserBeam> laserArray = new List<LaserBeam>();

        private Texture2D background;
        private Texture2D[] cylonRaider = new Texture2D[14];
        private Texture2D laserBeamTexture;
        private LaserBeam testBeam;

        private System.TimeSpan last;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            ship = new Ship(Content.Load<Texture2D>("ViperMK2.1s"), Content.Load<Texture2D>("engineFlame"));

            laserBeamTexture = Content.Load<Texture2D>("laserBeam");
            for (int i = 0; i < 7; i++) {
                cylonRaider[i] = Content.Load<Texture2D>("CylonRaider");
            }
            testBeam = new LaserBeam(new Vector2(ship.getX(), ship.getY()), laserBeamTexture);
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            ship.Update(keyboardState);


            if (keyboardState.IsKeyDown(Keys.Space)) {
                //TODO: add a new laser
                System.TimeSpan interval = gameTime.TotalGameTime;
                Debug.WriteLine(gameTime.TotalGameTime.ToString());
                if (interval > last + new System.TimeSpan(0, 0, 1)) {
                    laserArray.Add(new LaserBeam(new Vector2(ship.getX() + (ship.getWidth() / 2), ship.getY())));
                    last = interval;
                }
                
            }

            //game update stuff
            foreach (var laser in laserArray) {
                laser.Update();
             }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-200,-200), Color.White);
            ship.Draw(spriteBatch);

            for(int i = 0; i < 7; i++) {
                spriteBatch.Draw(cylonRaider[i], new Vector2((i * 100 + 50), 15), Color.White);
                spriteBatch.Draw(cylonRaider[i], new Vector2((i * 100 + 50), 150), Color.White);
            }
            
            //draw laser beams
            foreach (var laser in laserArray) {
                spriteBatch.Draw(laserBeamTexture, laser.location, Color.White);
            }
            testBeam.SetHeading(ship.getHeading());
            testBeam.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

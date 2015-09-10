using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ourGame {

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private KeyboardState keyboardState;
        Vector2 playerPosition;

        List<LaserBeam> laserArray = new List<LaserBeam>();

        private Texture2D viperTexture;
        private Texture2D laserBeamTexture;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            playerPosition = new Vector2(250, 250);
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            viperTexture = Content.Load<Texture2D>("ViperMK2.1s.png");
            laserBeamTexture = Content.Load<Texture2D>("laserBeam.png");
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {


            //keyboard stuff
        
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            var playerDelta = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                playerDelta -= new Vector2(1.0f, 0.0f);
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                playerDelta -= new Vector2(-1.0f, 0.0f);
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) {
                playerDelta -= new Vector2(0.0f, 1.0f);
            }

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) {
                playerDelta -= new Vector2(0.0f, -1.0f);
                
            }

            playerPosition += playerDelta * 2.5f;

            if (keyboardState.IsKeyDown(Keys.Space)) {
                //TODO: add a new laser
                laserArray.Add(new LaserBeam(new Vector2(playerPosition.X + (viperTexture.Width/2), playerPosition.Y)));
            }

            //game update stuff
            foreach (var laser in laserArray) {
                if (laser == null) {
                    continue;
                }
                laser.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(viperTexture, playerPosition, Color.White);

            //draw laser beams
            foreach (var laser in laserArray) {
                if(laser == null) {
                    continue;
                }
                spriteBatch.Draw(laserBeamTexture, laser.location, Color.White);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}

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

        Ship ship;

        private Texture2D background;
        private Texture2D[] cylonRaider = new Texture2D[14];

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            viperTexture = Content.Load<Texture2D>("ViperMK2.1s.png");
            laserBeamTexture = Content.Load<Texture2D>("laserBeam.png");

            background = Content.Load<Texture2D>("background.jpg");
            ship = new Ship(Content.Load<Texture2D>("ViperMK2.1s.png"));

            for (int i = 0; i < 7; i++) {
                cylonRaider[i] = Content.Load<Texture2D>("CylonRaider.png");
            }
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {

            //keyboard stuff
        
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            ship.Update(keyboardState);

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
                if (laser == null) {
                    continue;
                }
                spriteBatch.Draw(laserBeamTexture, laser.location, Color.White);
            }

            spriteBatch.Draw(background, new Vector2(-200,-200), Color.White);
            ship.Draw(spriteBatch);

            for(int i = 0; i < 7; i++) {
                spriteBatch.Draw(cylonRaider[i], new Vector2((i * 100 + 50), 15), Color.White);
                spriteBatch.Draw(cylonRaider[i], new Vector2((i * 100 + 50), 150), Color.White);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}

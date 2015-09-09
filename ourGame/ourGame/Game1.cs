using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ourGame {

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private KeyboardState keyboardState;
        Vector2 playerPosition;
        Vector2 laserBeamPosition;

        private Texture2D viper;
        private Texture2D laserBeam;

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
            viper = Content.Load<Texture2D>("ViperMK2.1s.png");
            laserBeam = Content.Load<Texture2D>("laserBeam.png");
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {

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
                //for (laserBeamPosition = (playerPosition + new Vector2(12.0f, 30.0f)); laserBeamPosition.Y < 900.0f; laserBeamPosition =  (laserBeamPosition + new Vector2(0.0f, 1.0f))) {
                //    System.Diagnostics.Debug.Write("HerpDerp prior to printstuff\n");
                //    System.Diagnostics.Debug.Write(laserBeamPosition);
                //    System.Diagnostics.Debug.Write("HerpDerp prior to spritBatch.Begin()\n");
                //    spriteBatch.Begin();
                //    System.Diagnostics.Debug.Write("HerpDerp post to spritBatch.Begin()\n");
                //    spriteBatch.Draw(laserBeam, laserBeamPosition, Color.White);
                //              offset of current player position: Vector2(12.0f, 30.0f)
                //      remove something if the beam gets too long (like 6 in a row is fine but when above line creates a 7th, the 1st is removed again)
                //    System.Diagnostics.Debug.Write("HerpDerp prior to spritBatch.End()\n");
                //    spriteBatch.End();
                //    System.Diagnostics.Debug.Write("HerpDerp post to spritBatch.End()\n");
                //}
            }
            //System.Diagnostics.Debug.Print("", playerPosition.X);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(viper, playerPosition, Color.White);
            spriteBatch.Draw(laserBeam, (playerPosition + new Vector2(12.0f, 30.0f)), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

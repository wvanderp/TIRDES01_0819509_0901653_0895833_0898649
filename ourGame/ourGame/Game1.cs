using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ourGame {

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D viper;
        Vector2 playerPosition;
        private KeyboardState keyboardState;

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
            viper = Content.Load<Texture2D>("ViperMK2.png");
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            var playerDelta = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.Left))
                playerDelta -= new Vector2(1.0f, 0.0f);
            if (keyboardState.IsKeyDown(Keys.Right))
                playerDelta -= new Vector2(-1.0f, 0.0f);
            if (keyboardState.IsKeyDown(Keys.Up))
                playerDelta -= new Vector2(0.0f, 1.0f);
            if (keyboardState.IsKeyDown(Keys.Down))
                playerDelta -= new Vector2(0.0f, -1.0f);

            playerPosition += playerDelta * 4.5f;

            base.Update(gameTime);
            }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(viper, playerPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

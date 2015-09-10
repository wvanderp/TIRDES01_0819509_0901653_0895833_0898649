using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ourGame {

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ship ship;


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
            ship = new Ship(Content.Load<Texture2D>("ViperMK2.1s.png"));

            for (int i = 0; i < 7; i++) {
                cylonRaider[i] = Content.Load<Texture2D>("CylonRaider.png");
            }
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            ship.Update(keyboardState);

            if (keyboardState.IsKeyDown(Keys.Space)) {
           
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Content.Load<Texture2D>("ViperMK2.1s.png"););

            spriteBatch.Begin();
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

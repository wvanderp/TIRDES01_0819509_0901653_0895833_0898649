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

        List<Projectile> laserArray = new List<Projectile>();

        private Texture2D background;
        CylonRaider[] cylonRaiders = new CylonRaider[45];
        private Texture2D projectileTexture;
        private Projectile testBeam;
        private int shotsLeftInBurst = 4;

        private TimeSpan lastBurst;
        private TimeSpan lastShot;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
        }

        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            ship = new Ship(Content.Load<Texture2D>("ViperMK2.1s"), Content.Load<Texture2D>("engineFlame"));
            projectileTexture = Content.Load<Texture2D>("projectile");
            Texture2D cylonTexture = Content.Load<Texture2D>("CylonRaider");
            for (int i = 0; i < 15; i++) {
                cylonRaiders[i] = new CylonRaider(cylonTexture, new Rectangle((i * 80 + 100), (25), 50, 75));
                cylonRaiders[i + 15] = new CylonRaider(cylonTexture, new Rectangle((i * 80 + 100), (125), 50, 75));
                cylonRaiders[i + 30] = new CylonRaider(cylonTexture, new Rectangle((i * 80 + 100), (225), 50, 75));
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
                //TODO: add a new laser
                TimeSpan interval = gameTime.TotalGameTime;
                if (interval > lastShot + new TimeSpan(0, 0, 0, 0, 100 )) {
                    if (shotsLeftInBurst-- > 0) {
                        int laser1y = ship.getY() - (int)(Math.Sin((double)ship.getHeading()) * 10);
                        int laser2y = ship.getY() + (int)(Math.Sin((double)ship.getHeading()) * 10);
                        int laser1x = ship.getX() - (int)(Math.Cos((double)ship.getHeading()) * 10);
                        int laser2x = ship.getX() + (int)(Math.Cos((double)ship.getHeading()) * 10);
                        Console.WriteLine("Lazor 1 y:{0}\nLazor 2 y:{1}", laser1y, laser2y);
                        laserArray.Add(new Projectile(new Vector2(laser1x, laser1y), projectileTexture, ship.getHeading(), ship.getProjectileSpeed()));
                        laserArray.Add(new Projectile(new Vector2(laser2x, laser2y), projectileTexture, ship.getHeading(), ship.getProjectileSpeed()));
                        lastShot = interval;
                        lastBurst = interval;
                    }
                }
                if (interval > lastBurst + new TimeSpan(0, 0, 1)) {
                    shotsLeftInBurst = 4;
                    lastBurst = interval;
                }
            }

            //game update stuff
            foreach (var laser in laserArray) {
                laser.Update();
             }

            foreach (CylonRaider raider in cylonRaiders) {
                raider.Update(ship.Position);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-200,-200), Color.White);

            for (int i = 0; i < 45; i++) {
                cylonRaiders[i].Draw(spriteBatch);
            }
            ship.Draw(spriteBatch);

            //draw laser beams
            foreach (var laser in laserArray) {
                laser.Draw(spriteBatch);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

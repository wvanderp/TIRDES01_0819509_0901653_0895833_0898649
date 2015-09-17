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

        private Texture2D cylonTexture;
        private Texture2D background;
        private Texture2D projectileTexture;

        List<Projectile> laserArray = new List<Projectile>();

        List<CylonRaider> cylonRaiders = new List<CylonRaider>();
        private int shotsLeftInBurst = 4;

        private TimeSpan lastBurst;
        private TimeSpan lastShot;

        private int programCounter = 0;
        private int initForLoopVM, amount;
        private float firstDelay, secondDelay;
        private Random random = new Random();

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
            cylonTexture = Content.Load<Texture2D>("CylonRaider");
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {

            KeyboardState keyboardState = Keyboard.GetState();
            float deltaTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Space)) {
                TimeSpan interval = gameTime.TotalGameTime;
                if (interval > lastShot + new TimeSpan(0, 0, 0, 0, 100)) {
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

            foreach (var laser in laserArray) {
                laser.Update();
            }

            ship.Update(keyboardState);

    
            foreach (CylonRaider raider in cylonRaiders)
            {
                raider.Update(ship.Position);
            }

            //This block should always come last!
            switch (programCounter)
            {
                case 0:
                    if (true)
                    {
                        programCounter = 1;
                        initForLoopVM = 0;
                        amount = random.Next(3, 9);
                    }
                    break;

                case 1:
                    if(initForLoopVM < amount)
                    {
                    cylonRaiders.Add(new CylonRaider(cylonTexture, new Rectangle(random.Next(10, 1300), random.Next(25, 30), 50, 75)));
                    initForLoopVM++;
                    }
                    else
                    {
                        programCounter = 2;
                        firstDelay = (float)(random.NextDouble() * 725.0 + 1500.0);
                    }
                    break;
                case 2:
                    firstDelay -= deltaTime;
                    if (firstDelay > 0.0f)
                    {
                        programCounter = 2;
                    }
                    else
                    {
                        programCounter = 3;
                    }
                    break;
                case 3:
                    if(cylonRaiders.Count > 25)
                    {
                        programCounter = 4;
                        secondDelay = 10000.0f;
                    }
                    else
                    {
                        programCounter = 0;
                    }
                    break;
                case 4:
                    secondDelay -= deltaTime;
                    if(secondDelay > 0.0f)
                    {
                        programCounter = 4;
                    }
                    else
                    {
                        programCounter = 3;
                    }
                    break;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-200,-200), Color.White);
            
            foreach (CylonRaider raider in cylonRaiders) {
                raider.Draw(spriteBatch);
            }

            ship.Draw(spriteBatch);

            foreach (var laser in laserArray) {
                laser.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

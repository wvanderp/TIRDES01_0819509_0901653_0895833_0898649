using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ourGame {

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Entity ship;
        private Texture2D cylonTexture;
        private Texture2D background;
        private Texture2D projectileTexture;

        List<Entity> lasers = new List<Entity>();
        List<Entity> cylonRaiders = new List<Entity>();

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
            ship = new Entity(new Vector2(400.0f, 300.0f), Content.Load<Texture2D>("ViperMK2.1s"), 0.0f, 25.0f);
            projectileTexture = Content.Load<Texture2D>("projectile");
            cylonTexture = Content.Load<Texture2D>("CylonRaider");

            int mul = 0;
            for (int j = 0; j < 3; j++) {
                for (int i = 0; i < 15; i++) {
                    cylonRaiders.Add(new Entity(new Vector2(350.0f * mul, 400.0f * mul), cylonTexture, 0.0f, 10.0f));
                }
                mul += 100;
            }
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {

            KeyboardState keyboardState = Keyboard.GetState();
            float deltaTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //game update stuff
            foreach (var laser in lasers) {
                laser.CreateNext(new Vector2(laser.X - laser.Speed, laser.Y - laser.Speed), laser.H);
            }

            foreach (Entity raider in cylonRaiders) {
                raider.CreateNext(new Vector2(raider.X - ship.X, raider.Y - ship.Y), ship.H);
            }

            ship.CreateNext(new Vector2(ship.X + ship.S, ship.Y + ship.S), ship.H);

            var newRaiderList =
              (from raider in cylonRaiders
               let raiderPos = new Vector2(raider.X, raider.Y)
               let colliders =
                  from laser in lasers
                  where Vector2.Distance(raiderPos, new Vector2(laser.X, laser.Y)) < 20.0f
                  select raiderPos
               where colliders.Count() == 0
               select raider
            ).ToList();

            var newlasers =
              (from laser in lasers
               let colliders =
                  from raider in cylonRaiders
                  let raiderPos = new Vector2(raider.X, raider.Y)
                  where Vector2.Distance(raiderPos, new Vector2(laser.X, laser.Y)) < 20.0f
                  select raiderPos
               where colliders.Count() == 0
               select laser
            ).ToList();

            if (keyboardState.IsKeyDown(Keys.Space)) {
                TimeSpan interval = gameTime.TotalGameTime;
                if (interval > lastShot + new TimeSpan(0, 0, 0, 0, 100)) {
                    if (shotsLeftInBurst-- > 0) {
                        //calculate the location of the projectiles
                        int projectile1x = (int)(ship.X - Math.Cos(ship.H) * 10);
                        int projectile1y = (int)(ship.Y - Math.Sin(ship.H) * 10);

                        int projectile2x = (int)(ship.X + Math.Cos(ship.H) * 10);
                        int projectile2y = (int)(ship.Y + Math.Sin(ship.H) * 10);

                        newlasers.Add(new Entity(new Vector2(projectile1x, projectile1y), projectileTexture, ship.H, ship.S));
                        newlasers.Add(new Entity(new Vector2(projectile2x, projectile2y), projectileTexture, ship.H, ship.S));

                        lastShot = interval;
                        lastBurst = interval;
                    }
                }
                if (interval > lastBurst + new TimeSpan(0, 0, 1)) {
                    shotsLeftInBurst = 4;
                    lastBurst = interval;
                }
            }

            lasers = newlasers;
            cylonRaiders = newRaiderList;


            //This block should always come last!
            switch (programCounter) {
                case 0:
                    if (true) {
                        programCounter = 1;
                        initForLoopVM = 0;
                        amount = random.Next(3, 9);
                    }
                    break;

                case 1:
                    if (initForLoopVM < amount) {
                        cylonRaiders.Add(new Entity(new Vector2(100.0f, 100.0f), cylonTexture, 0.0f, 10.0f));
                        initForLoopVM++;
                    }
                    else {
                        programCounter = 2;
                        firstDelay = (float)(random.NextDouble() * 725.0 + 1500.0);
                    }
                    break;
                case 2:
                    firstDelay -= deltaTime;
                    if (firstDelay > 0.0f) {
                        programCounter = 2;
                    }
                    else {
                        programCounter = 3;
                    }
                    break;
                case 3:
                    if (cylonRaiders.Count > 25) {
                        programCounter = 4;
                        secondDelay = 10000.0f;
                    }
                    else {
                        programCounter = 0;
                    }
                    break;
                case 4:
                    secondDelay -= deltaTime;
                    if (secondDelay > 0.0f) {
                        programCounter = 4;
                    }
                    else {
                        programCounter = 3;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-200, -200), Color.White);

            //draw raiders
            foreach (Entity raider in cylonRaiders) {
                spriteBatch.Draw(raider.Appearance, new Vector2(raider.X, raider.Y), Color.White);
            }

            //draw ship
            spriteBatch.Draw(ship.Appearance, new Vector2(ship.X, ship.Y), Color.White);

            //draw laser beams
            foreach (var laser in lasers) {
                spriteBatch.Draw(laser.Appearance, new Vector2(laser.X, laser.Y), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

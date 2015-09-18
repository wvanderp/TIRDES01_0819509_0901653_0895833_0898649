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
        Ship ship;

        private Texture2D cylonTexture;
        private Texture2D background;
        private Texture2D projectileTexture;

        List<Projectile> laserList = new List<Projectile>();
        List<CylonRaider> cylonRaiderList = new List<CylonRaider>();
       
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

            int muli = 0;
            for (int j = 0; j < 3; j++) {
                for (int i = 0; i < 15; i++) {
                    cylonRaiderList.Add(new CylonRaider(cylonTexture, new Rectangle((i * 80 + 100), (25 + muli), 50, 75)));
                }
                muli += 100;
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
            foreach (var laser in laserList) {
                laser.Update();
            }

            ship.Update(keyboardState);

    
            foreach (CylonRaider raider in cylonRaiderList){
                raider.Update(ship.Position);
            }
        
            ship.Update(keyboardState);



            var newRaiderList =
              (from raider in cylonRaiderList
               let raiderPos = new Vector2(raider.position.X, raider.position.Y)
               let colliders =
                  from laser in laserList
                  where Vector2.Distance(raiderPos, laser.location) < 20.0f
                  select raiderPos
               where colliders.Count() == 0
               select raider
            ).ToList();

            var newLaserList =
              (from laser in laserList
               let colliders =
                  from raider in cylonRaiderList
                  let raiderPos = new Vector2(raider.position.X, raider.position.Y)
                  where Vector2.Distance(raiderPos, laser.location) < 20.0f
                  select raiderPos
               where colliders.Count() == 0
               select laser
            ).ToList();

            if (keyboardState.IsKeyDown(Keys.Space)) {
                TimeSpan interval = gameTime.TotalGameTime;
                if (interval > lastShot + new TimeSpan(0, 0, 0, 0, 100)) {
                    if (shotsLeftInBurst-- > 0) {
                        //calculate the location of the lazors
                        int laser1x = ship.getX() - (int)(Math.Cos((double)ship.getHeading()) * 10);
                        int laser1y = ship.getY() - (int)(Math.Sin((double)ship.getHeading()) * 10);

                        int laser2x = ship.getX() + (int)(Math.Cos((double)ship.getHeading()) * 10);
                        int laser2y = ship.getY() + (int)(Math.Sin((double)ship.getHeading()) * 10);

                        Console.WriteLine("Lazor 1 y:{0}\nLazor 2 y:{1}", laser1y, laser2y);

                        newLaserList.Add(new Projectile(new Vector2(laser1x, laser1y), projectileTexture, ship.getHeading(), ship.getProjectileSpeed()));
                        newLaserList.Add(new Projectile(new Vector2(laser2x, laser2y), projectileTexture, ship.getHeading(), ship.getProjectileSpeed()));

                        lastShot = interval;
                        lastBurst = interval;
                    }
                }
                if (interval > lastBurst + new TimeSpan(0, 0, 1)) {
                    shotsLeftInBurst = 4;
                    lastBurst = interval;
                }
            }

            laserList = newLaserList;
            cylonRaiderList = newRaiderList;


            //This block should always come last!
            switch (programCounter)
            {
                case 0:
                    if (true){
                        programCounter = 1;
                        initForLoopVM = 0;
                        amount = random.Next(3, 9);
                    }
                    break;

                case 1:
                    if(initForLoopVM < amount){
                        cylonRaiderList.Add(new CylonRaider(cylonTexture, new Rectangle(random.Next(10, 1300), random.Next(25, 30), 50, 75)));
                        initForLoopVM++;
                    }else {
                        programCounter = 2;
                        firstDelay = (float)(random.NextDouble() * 725.0 + 1500.0);
                    }
                    break;
                case 2:
                    firstDelay -= deltaTime;
                    if (firstDelay > 0.0f){
                        programCounter = 2;
                    } else{
                        programCounter = 3;
                    }
                    break;
                case 3:
                    if(cylonRaiderList.Count > 25){
                        programCounter = 4;
                        secondDelay = 10000.0f;
                    } else {
                        programCounter = 0;
                    }
                    break;
                case 4:
                    secondDelay -= deltaTime;
                    if(secondDelay > 0.0f) {
                        programCounter = 4;
                    } else {
                        programCounter = 3;
                    }
                    break;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-200,-200), Color.White);

            //draw raiders
            foreach (CylonRaider raider in cylonRaiderList) {
                raider.Draw(spriteBatch);
            }

            //draw ship
            ship.Draw(spriteBatch);

            //draw laser beams
            foreach (var laser in laserList) {
                laser.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

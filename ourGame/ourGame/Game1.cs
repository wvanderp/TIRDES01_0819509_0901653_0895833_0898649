using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ourGame
{
    public class Projectile
    {
        public Vector2 location = new Vector2(0f, 0f);
        public float heading = 0f;
        public float speed = 10f;
        Texture2D laserBeamTexture;

        public Projectile(Vector2 location, Texture2D laserBeamTexture, float heading = 0f, float speed = 10f)
        {
            this.location = location;
            this.heading = heading - (float)(Math.PI / 2);
            this.laserBeamTexture = laserBeamTexture;
            this.speed = speed;
        }

        public void Update()
        {
            location += new Vector2((float)Math.Cos((double)heading), (float)Math.Sin((double)heading)) * speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(laserBeamTexture, location, null, null, new Vector2(0, 0), heading - (float)(Math.PI / 2), null, Color.White, new SpriteEffects(), 1.0f);
        }

        public void SetHeading(float heading)
        {
            this.heading = heading;
        }

        public float getHeading()
        {
            return heading;
        }
    }

    public class Ship
    {
        Texture2D texture;
        Texture2D engineTexture;
        float heading = 0.0f;
        float speed = 0.0f;
        Rectangle position = new Rectangle(300, 200, 50, 75);
        float x = 400.0f, y = 350.0f;
        float cannonSpeed = 10.0f;
        public Ship(Texture2D texture, Texture2D engineTexture)
        {
            this.texture = texture;
            this.engineTexture = engineTexture;
        }

        public void Update(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                heading -= MathHelper.Pi / 60;
                System.Console.WriteLine("Rotation: {0}", heading - MathHelper.Pi);
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                heading += MathHelper.Pi / 60;
                System.Console.WriteLine("Rotation: {0}", heading - MathHelper.Pi);
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                speed += .3f;
            }

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                speed -= .1f;
            }

            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                speed = 0.0f;
                x = 0.0f;
                y = 0.0f;
            }

            if (heading > MathHelper.Pi * 2)
            {
                heading -= MathHelper.Pi * 2;
            }

            else if (heading < 0)
            {
                heading += MathHelper.Pi * 2;
            }

            x += (float)System.Math.Sin((double)heading) * speed;
            y += (float)System.Math.Cos((double)heading) * speed * -1;

            position.Y = (int)y;
            position.X = (int)x;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle: null, color: Color.White, rotation: heading, origin: new Vector2(texture.Width / 2, texture.Height / 2), effects: SpriteEffects.None, layerDepth: 1.0f);
            // TODO: Add your drawing code here
        }

        //getters + setters
        public int getX()
        {
            return position.X;
        }

        public int getY()
        {
            return position.Y;
        }

        public int getWidth()
        {
            return position.Width;
        }

        public int getHeight()
        {
            return position.Height;
        }

        public float getHeading()
        {
            return heading;
        }

        public float getProjectileSpeed()
        {
            return speed + cannonSpeed;
        }

        public Rectangle Position
        {
            get { return position; }
        }
    }

    public class CylonRaider
    {
        static Random r;
        private Texture2D cylonGraphics;
        public Rectangle position;

        private float heading;
        private float speed;

        //more like temp vars
        float x, y;

        public CylonRaider(Texture2D cylonGraphics, Rectangle position)
        {
            if (r == null)
            {
                r = new Random();
            }
            this.cylonGraphics = cylonGraphics;
            this.position = position;
            heading = (float)r.NextDouble() * MathHelper.Pi * 2;
            speed = .3f;
            x = (float)position.X;
            y = (float)position.Y;
        }

        public void Update(Rectangle targetBounds)
        {
            x += ((float)Math.Cos((double)heading) * speed);
            y += ((float)Math.Sin((double)heading) * speed);

            float targetX = targetBounds.X + (targetBounds.Width / 2) - position.X - (position.Width / 2);
            float targetY = targetBounds.Y + (targetBounds.Height / 2) - position.Y - (position.Height / 2);

            Vector2 target = new Vector2(targetX, targetY);

            heading = (float)Math.Atan2((double)target.Y, (double)target.X);
            speed = target.Length() / 120;

            position.X = (int)x;
            position.Y = (int)y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cylonGraphics, position, sourceRectangle: null, color: Color.White, rotation: heading - (float)(Math.PI / 2), origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1.0f);
        }
    }


    public class Game1 : Game
    {

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            ship = new Ship(Content.Load<Texture2D>("ViperMK2.1s"), Content.Load<Texture2D>("engineFlame"));
            projectileTexture = Content.Load<Texture2D>("projectile");
            cylonTexture = Content.Load<Texture2D>("CylonRaider");

            int muli = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 15; i++)
                {
                    cylonRaiderList.Add(new CylonRaider(cylonTexture, new Rectangle((i * 80 + 100), (25 + muli), 50, 75)));
                }
                muli += 100;
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();
            float deltaTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //game update stuff
            foreach (var laser in laserList)
            {
                laser.Update();
            }

            ship.Update(keyboardState);


            foreach (CylonRaider raider in cylonRaiderList)
            {
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

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                TimeSpan interval = gameTime.TotalGameTime;
                if (interval > lastShot + new TimeSpan(0, 0, 0, 0, 100))
                {
                    if (shotsLeftInBurst-- > 0)
                    {
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
                if (interval > lastBurst + new TimeSpan(0, 0, 1))
                {
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
                    if (true)
                    {
                        programCounter = 1;
                        initForLoopVM = 0;
                        amount = random.Next(3, 9);
                    }
                    break;

                case 1:
                    if (initForLoopVM < amount)
                    {
                        cylonRaiderList.Add(new CylonRaider(cylonTexture, new Rectangle(random.Next(10, 1300), random.Next(25, 30), 50, 75)));
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
                    if (cylonRaiderList.Count > 25)
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
                    if (secondDelay > 0.0f)
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

        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-200, -200), Color.White);

            //draw raiders
            foreach (CylonRaider raider in cylonRaiderList)
            {
                raider.Draw(spriteBatch);
            }

            //draw ship
            ship.Draw(spriteBatch);

            //draw laser beams
            foreach (var laser in laserList)
            {
                laser.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

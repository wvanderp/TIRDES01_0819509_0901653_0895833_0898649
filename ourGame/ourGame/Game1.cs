using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using ourGame.Instructions;

namespace ourGame {
    enum InstructionResult{
        Done,
        DoneAndCreateCylon,
        Running,
        RunningAndCreateCylon
    }

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D cylonTexture;
        private Texture2D background;
        private Texture2D projectileTexture;
        GameInput input;

        private Entity ship;
        List<Entity> lasers = new List<Entity>();
        List<Entity> cylonRaiders = new List<Entity>();

        Weapon<Entity> currentWeapon;

        private int programCounter = 0;
        private int initForLoopVM, amount;
        private float firstDelay, secondDelay;

        private static Random rnd = new Random();

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
        }

        Instruction gameLogic =
        new Repeat(
          new For(0, 10, i =>
                new Wait(() => i * 0.1f) +
                new CreateCylon()) +
          new Wait(() => rnd.Next(1, 5)) +
          new For(0, 10, i =>
                new Wait(() => (float)rnd.NextDouble() * 1.0f + 0.2f) +
                new CreateCylon()) +
          new Wait(() => rnd.Next(2, 3)));

        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("background");
            projectileTexture = Content.Load<Texture2D>("projectile");
            cylonTexture = Content.Load<Texture2D>("CylonRaider");

            ship = new Entity(new Vector2(400.0f, 300.0f), Content.Load<Texture2D>("ViperMK2.1s"), 0.0f, 0.0f);
            
            input = new WASDKeyboardInputController();
			input += new CursorKeyboardInputController ();

            int mul = 0;
            for (int j = 0; j < 3; j++) {
                for (int i = 0; i < 15; i++) {
                    cylonRaiders.Add(new Entity(new Vector2(350.0f * mul, 400.0f * mul), cylonTexture, 0.0f, 10.0f));
                }
                mul += 100;
            }

            currentWeapon = new Blaster(projectileTexture);
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            input.Update();
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

            var untouchedRaiders =
              (from raider in cylonRaiders
               let raiderPos = new Vector2(raider.X, raider.Y)
               let colliders =
                  from laser in lasers
                  where Vector2.Distance(raiderPos, new Vector2(laser.X, laser.Y)) < 20.0f
                  select raiderPos
               where colliders.Count() == 0
               select raider
            ).ToList();

            var untouchedLasers =
              (from laser in lasers
               let colliders =
                  from raider in cylonRaiders
                  let raiderPos = new Vector2(raider.X, raider.Y)
                  where Vector2.Distance(raiderPos, new Vector2(laser.X, laser.Y)) < 20.0f
                  select raiderPos
               where colliders.Count() == 0
               select laser
            ).ToList();
            lasers = untouchedLasers;
            cylonRaiders = untouchedRaiders;

            #region shipUpdate
            float shipheading = ship.Heading;
            float shipSpeed = ship.Speed;
            if(input.CurrentRotationState == RotationState.CCW) {
            }
            if (input.CurrentRotationState == RotationState.CW)
                shipheading += MathHelper.Pi / 60;
            if (input.CurrentRotationState == RotationState.CCW)
                shipheading -= MathHelper.Pi / 60;

            if(input.ShouldIncreaseSpeed) {
                shipSpeed += .1f;
            }
            if(input.ShouldDecreaseSpeed) {
                shipSpeed -= .1f;
            }
            float deltaX = (float)System.Math.Sin((double)shipheading) * shipSpeed;
            float deltaY = (float)System.Math.Cos((double)shipheading) * shipSpeed * -1;
            Vector2 shipPosition = new Vector2(ship.X, ship.Y);
            shipPosition.X += deltaX;
            shipPosition.Y += deltaY;
            ship = new Entity(shipPosition, ship.Appearance, shipheading, shipSpeed);
            #endregion

            #region projectileUpdate
            List<Entity> newlasers = new List<Entity>();
            foreach(Entity laser in lasers) {
                Vector2 laserPositionDeltas = Vector2.Zero;
                laserPositionDeltas.X += (float)System.Math.Sin((double)laser.Heading) * laser.Speed;
                laserPositionDeltas.Y += (float)System.Math.Cos((double)laser.Heading) * laser.Speed * -1;
                newlasers.Add(laser.CreateNext(laserPositionDeltas, 0.0f));
            };
            #endregion

            #region cylonUpdate
            List<Entity> newCylonRaiders = new List<Entity>();
            foreach (Entity cylonRaider in cylonRaiders) {
                Vector2 cylonPosition = cylonRaider.Position;
                cylonPosition.X += ((float)Math.Cos((double)cylonRaider.Heading) * cylonRaider.Speed);
                cylonPosition.Y += ((float)Math.Sin((double)cylonRaider.Heading) * cylonRaider.Speed);

                float targetX = ship.X - cylonPosition.X;
                float targetY = ship.Y - cylonPosition.Y;

                Vector2 target = new Vector2(targetX, targetY);

                float heading = (float)Math.Atan2((double)target.Y, (double)target.X);
                float speed = target.Length() / 120;
                newCylonRaiders.Add(new Entity(cylonPosition, cylonRaider.Appearance, heading, speed));
            }

            #endregion

            #region shoot Bullets
            if (input.TriggerPressed) {
                currentWeapon.pullTrigger();
            }
            #endregion

            #region statagie decorater
            switch (gameLogic.Execute(deltaTime))
            {
                case InstructionResult.DoneAndCreateCylon:
                    cylonRaiders.Add(new Entity(new Vector2(100.0f, 100.0f), cylonTexture, 0.0f, 10.0f));
                    break;
                case InstructionResult.RunningAndCreateCylon:
                    cylonRaiders.Add(new Entity(new Vector2(100.0f, 100.0f), cylonTexture, 0.0f, 10.0f));
                    break;
            }
            #endregion

            #region updateVars
            lasers = newlasers;
            cylonRaiders = newCylonRaiders;
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(-200, -200), Color.White);

            //draw raiders
            foreach (Entity raider in cylonRaiders) {
                spriteBatch.Draw(raider.Appearance, raider.Position, null, Color.White, (float)((double)raider.Heading - (Math.PI / 2)), new Vector2(raider.Appearance.Width / 2, raider.Appearance.Height / 2), 1.0f, SpriteEffects.None, 0.0f); 
            }

            //draw ship
            spriteBatch.Draw(ship.Appearance, new Vector2(ship.X, ship.Y), null, Color.White, ship.Heading, new Vector2(ship.Appearance.Width / 2, ship.Appearance.Height / 2), 1.0f, SpriteEffects.None,0.0f);
            //draw laser beams
            foreach (var laser in lasers) {
                spriteBatch.Draw(laser.Appearance, new Vector2(laser.X, laser.Y), null, Color.White, laser.Heading, new Vector2(laser.Appearance.Width / 2, laser.Appearance.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

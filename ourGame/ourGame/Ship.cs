﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ourGame {
    class Ship {
        Texture2D texture;
        Texture2D engineTexture;
        float heading = 0.0f;
        float speed = 0.0f;
        Rectangle position = new Rectangle(300, 200, 50, 75);
        float x= 400.0f, y = 350.0f;
        float cannonSpeed = 10.0f;
        public Ship(Texture2D texture, Texture2D engineTexture) {
            this.texture = texture;
            this.engineTexture = engineTexture;
        }

        public void Update(KeyboardState keyboardState) {
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                heading -= MathHelper.Pi / 60;
                System.Console.WriteLine("Rotation: {0}", heading - MathHelper.Pi);
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                heading += MathHelper.Pi / 60;
                System.Console.WriteLine("Rotation: {0}", heading - MathHelper.Pi);
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) {
                speed+= .3f;
            }

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) {
                speed -= .1f;
            }
            
            if (keyboardState.IsKeyDown(Keys.Tab)) {
                speed = 0.0f;
                x = 0.0f;
                y = 0.0f;
            }

            if (heading > MathHelper.Pi * 2) {
                heading -= MathHelper.Pi * 2;
            }
            
            else if (heading < 0){
                heading += MathHelper.Pi * 2;
            }

            x += (float)System.Math.Sin((double)heading) * speed;
            y += (float)System.Math.Cos((double)heading) * speed * -1;
            
            position.Y = (int)y;
            position.X = (int)x;            
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, sourceRectangle: null, color: Color.White, rotation: heading, origin: new Vector2(texture.Width / 2, texture.Height / 2),effects: SpriteEffects.None, layerDepth: 1.0f);
        }
    }
}

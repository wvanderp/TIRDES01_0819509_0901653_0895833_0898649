using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ourGame
{
    class Ship
    {
        Texture2D texture;
        bool inertiaDampenersEnabled = false;
        float heading = 0.0f;
        float speed = 0.0f;
        Vector2 velocity = new Vector2(0, 0);
        Rectangle position = new Rectangle(300, 200, 50, 50);
        bool spacebarPressed = false;
        float x= 400.0f, y = 350.0f;
        public Ship(Texture2D texture)
        {
            this.texture = texture;
        }
        public void Update(KeyboardState keyboardState)
        {
            if(keyboardState.IsKeyDown(Keys.Left)) {
                heading -= 3.0f / 60;
                System.Console.WriteLine("Rotation: {0}", heading - MathHelper.Pi);
            }
            if(keyboardState.IsKeyDown(Keys.Right)) {
                heading += 3.0f / 60;
                System.Console.WriteLine("Rotation: {0}", heading - MathHelper.Pi);
            }
            if (keyboardState.IsKeyDown(Keys.Up)) {
                
                if (!inertiaDampenersEnabled) {
                    velocity.X += (float)System.Math.Sin((double)heading) * .3f;
                    velocity.Y += (float)System.Math.Cos((double)heading) * .3f * -1;
                } else {
                    speed += .3f;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down)) {
                if (!inertiaDampenersEnabled) {
                    velocity.X -= (float)System.Math.Sin((double)heading) * .3f;
                    velocity.Y -= (float)System.Math.Cos((double)heading) * .3f * -1;
                }
                else {
                    speed -= .1f;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Space)) {
                if (!spacebarPressed) {
                    spacebarPressed = true;
                    if (inertiaDampenersEnabled) {
                        inertiaDampenersEnabled = false;
                        velocity.X = (float)System.Math.Sin((double)heading) * speed;
                        velocity.Y = (float)System.Math.Cos((double)heading) * speed * -1;
                    }
                    else {
                        inertiaDampenersEnabled = true;
                    }
                }
                
            }
            if (keyboardState.IsKeyUp(Keys.Space)) {
                spacebarPressed = false;
            }
            if (keyboardState.IsKeyDown(Keys.Tab)) {
                speed = 0.0f;
                x = 0.0f;
                y = 0.0f;
            }
            if (heading > MathHelper.Pi * 2) {
                heading -= MathHelper.Pi * 2;
            } else if (heading < 0){
                heading += MathHelper.Pi * 2;
            }
            if (inertiaDampenersEnabled) {
                x += (float)System.Math.Sin((double)heading) * speed;
                y += (float)System.Math.Cos((double)heading) * speed * -1;
            }
            else {
                x += velocity.X;
                y += velocity.Y;
            }
            position.Y = (int)y;
            position.X = (int)x;            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle: null, color: Color.White, rotation: heading, origin: new Vector2(texture.Width / 2, texture.Height / 2),effects: SpriteEffects.None, layerDepth: 1.0f);
            // TODO: Add your drawing code here
        }
    }
}

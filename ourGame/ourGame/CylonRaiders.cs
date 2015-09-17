using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ourGame {
    class CylonRaider {
        static Random r;
        private Texture2D cylonGraphics;
        private Rectangle position;

        private float heading;
        private float speed;
        float x, y;

        public CylonRaider(Texture2D cylonGraphics, Rectangle position) {
            if (r == null) {
                r = new Random();
            }
            this.cylonGraphics = cylonGraphics;
            this.position = position;
            heading = (float)r.NextDouble() * MathHelper.Pi * 2 ;
            speed = .3f;
            x = (float)position.X;
            y = (float)position.Y;
        }

        public void Update(Rectangle targetBounds) {
            x += ((float)Math.Cos((double)heading) * speed);
            y += ((float)Math.Sin((double)heading) * speed);
            Vector2 target = new Vector2((float)(targetBounds.X + (targetBounds.Width / 2) - position.X - (position.Width / 2)), (float)(targetBounds.Y + (targetBounds.Height / 2) - position.Y - (position.Height / 2)));
            heading = (float)Math.Atan2((double)target.Y, (double)target.X);
            speed = target.Length() / 120;
            position.X = (int)x;
            position.Y = (int)y;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(cylonGraphics, position, sourceRectangle: null, color: Color.White, rotation: heading - (float)(Math.PI / 2), origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1.0f);
        }
    }
}

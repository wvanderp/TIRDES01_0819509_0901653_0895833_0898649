using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace ourGame {
    class Projectile {
        public Vector2 location = new Vector2(0f, 0f);
        public float heading = 0f;
        public float speed = 10f;
        Texture2D laserBeamTexture; 

        public Projectile(Vector2 location, Texture2D laserBeamTexture, float heading=0f) {
            this.location = location;
            this.heading = heading - (float)(Math.PI / 2);
            this.laserBeamTexture = laserBeamTexture;
        }
        
        public void Update() {
            location += new Vector2((float)Math.Cos((double)heading), (float)Math.Sin((double) heading)) * speed;            
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(laserBeamTexture, location, null, null, new Vector2(0, 0), heading - (float)(Math.PI / 2), null, Color.White, new SpriteEffects(), 1.0f);
        }

        public void SetHeading(float heading) {
            this.heading = heading;
        }

        public float getHeading()
        {
            return heading;
        }
    }
}

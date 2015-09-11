using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace ourGame
{
    class LaserBeam{
        public Vector2 location = new Vector2(0f, 0f);
        public float heading = 0f;
        public float speed = 10f;
        Texture2D laserBeamTexture; 

        public void Update() {
            location += new Vector2((float)Math.Cos((double)heading), (float)Math.Sin((double) heading)) * speed;            
        }

        public LaserBeam(Vector2 location, Texture2D laserBeamTexture, float heading=0f) {
            this.location = location;
            this.heading = heading - (float)(Math.PI / 2);
            this.laserBeamTexture = laserBeamTexture;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(laserBeamTexture, new Vector2((float)Math.Cos((double)heading) * 10 + location.X + 3, (float)Math.Sin((double)heading) * 10 + location.Y - 28), Color.White);
        }

        public void SetHeading(float heading) {
            this.heading = heading;
        }
    }
}

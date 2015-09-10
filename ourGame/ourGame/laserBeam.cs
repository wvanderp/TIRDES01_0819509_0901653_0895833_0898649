using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ourGame
{
    class LaserBeam{
        public Vector2 location = new Vector2(0f, 0f);
        public float heading = 0f;
        public float speed = 10f;

        public void Update() {
            location += new Vector2(0f, -1f) * speed;            
        }

        public LaserBeam(Vector2 location, float heading=0f) {
            this.location = location;
            this.heading = heading;
        }
    }
}

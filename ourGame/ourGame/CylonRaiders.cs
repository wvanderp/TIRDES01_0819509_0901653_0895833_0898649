using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ourGame {
    class CylonRaider {
        private Texture2D cylonGraphics;
        private Rectangle position;

        public CylonRaider(Texture2D cylonGraphics, Rectangle position) {
            this.cylonGraphics = cylonGraphics;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(cylonGraphics, position, color: Color.White);
        }
    }
}

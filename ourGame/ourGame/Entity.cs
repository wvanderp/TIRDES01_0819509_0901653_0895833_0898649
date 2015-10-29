using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Content;

struct Entity {
    public Entity(Vector2 position, Texture2D appearance, float heading, float speed) {
        Position = position;
        Appearance = appearance;
        Heading = heading;
        Speed = speed;
    }

    public Vector2 Position {
        get; private set;
    }

    public Texture2D Appearance {
        get; private set;
    }

    public float Heading {
        get; private set;
    }

    public float Speed {
        get; private set;
    }

    public float X {
        get {
            return Position.X;
        }
    }

    public float Y {
        get {
            return Position.Y;
        }
    }

    public float H {
        get {
            return this.Heading;
        }
    }

    public float S {
        get {
            return this.Speed;
        }
    }

    public Entity CreateNext(Vector2 deltaPosition, float deltaHeading) {
        return new Entity() {
            Position = this.Position + deltaPosition,
            Appearance = this.Appearance,
            Heading = this.Heading + deltaHeading
        };
    }
}
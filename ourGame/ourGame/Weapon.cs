using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

interface Weapon<Ammunition> {
    void pullTrigger();
    List<Ammunition> NewBullets { get; }
    void Update(float dt, Vector2 shipPosition, float shipHeading, float shipSpeed);
}

abstract class GenaricBlaster : Weapon<Entity> {
    protected List<Entity> barrel = new List<Entity>();
    float timeSinceLastShot = float.PositiveInfinity;
    float charge = 100;

    protected Vector2 shipPosition;
    protected float shipHeading;
    protected float shipSpeed;

    protected Texture2D texture;

    protected abstract void AddShots();

    public List<Entity> NewBullets {
        get {
            return barrel;
        }
    }

    public void pullTrigger() {
        if(charge >= 10 && timeSinceLastShot >= 0.10f) {
            charge -= 2;
            timeSinceLastShot = 0.0f;
            AddShots();
        }
    }

    public void Update(float dt, Vector2 shipPosition, float shipHeading, float shipSpeed) {
        charge += dt * 20.0f;
        timeSinceLastShot += dt;
        charge = MathHelper.Clamp(charge, 0, 100);
        barrel = new List<Entity>();

        this.shipPosition = shipPosition;
        this.shipSpeed = shipSpeed;
        this.shipHeading = shipHeading;
    }
}

class Blaster : GenaricBlaster {
    public Blaster(Texture2D texture) {
        this.texture = texture;
    }

    protected override void AddShots() {
        barrel.Add(new Entity( shipPosition, texture, shipHeading, shipSpeed));
    }
}

class DoubleBlaster : GenaricBlaster {
    public DoubleBlaster(Texture2D texture) {
        this.texture = texture;
    }

    protected override void AddShots() {
        int projectile1x = (int)(shipPosition.X - Math.Cos(shipHeading) * 10);
        int projectile1y = (int)(shipPosition.Y - Math.Sin(shipHeading) * 10);

        int projectile2x = (int)(shipPosition.X + Math.Cos(shipHeading) * 10);
        int projectile2y = (int)(shipPosition.Y + Math.Sin(shipHeading) * 10);

        barrel.Add(new Entity(new Vector2(projectile1x, projectile1y), texture, shipHeading, shipSpeed));
        barrel.Add(new Entity(new Vector2(projectile2x, projectile2y), texture, shipHeading, shipSpeed));
    }
}

class addWepon {
    Weapon<Entity> w1 = new DoubleBlaster();
    Weapon<Entity> w2 = null;
    public addWepon()

    public void pullTrigger() {
        w1.pullTrigger();
        w2.pullTrigger();
    }

    public void Update(float dt, Vector2 shipPosition, float shipHeading, float shipSpeed) {
        charge += dt * 20.0f;
        timeSinceLastShot += dt;
        charge = MathHelper.Clamp(charge, 0, 100);
        barrel = new List<Entity>();

        this.shipPosition = shipPosition;
        this.shipSpeed = shipSpeed;
        this.shipHeading = shipHeading;
    }

}

class nullWapon {
    public void pullTrigger() {

    }
}
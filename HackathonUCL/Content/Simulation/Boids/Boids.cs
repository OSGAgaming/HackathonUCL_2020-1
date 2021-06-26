
using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace HackathonUCL
{
    public class Boids
    {
        internal List<Flock> fishflocks = new List<Flock>();

        public void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (Flock fishflock in fishflocks)
            {
                fishflock.Draw(spriteBatch);
            }
        }

        public void OnUpdate()
        {
            foreach (Flock fishflock in fishflocks)
            {
                fishflock.Update();
                fishflock.OnUpdate();
            }

            if (Main.GlobalTimer % 150 == 0)
            {
                int randInt = Main.rand.Next(0, fishflocks.Count);
                Vector2 rand = new Vector2(Main.rand.Next(-1800, 1800), Main.rand.Next(-1800, 1800));

                //  fishflocks[randInt].Populate(Utils.MouseScreen.ToVector2(), Main.rand.Next(fishflocks[randInt].randMin, fishflocks[randInt].randMax), 50f);
            }
        }

        public void OnLoad()
        {
            fishflocks.Add(new Flock("Particles/Coralfin", 1f, 5, 15));
            fishflocks.Add(new Flock("Particles/Barracuda", 0.5f, 3, 7));
            fishflocks.Add(new Flock("Particles/LargeBubblefish", 0.75f, 10, 20));
            fishflocks.Add(new Flock("Particles/SmallBubblefish", 1f, 15, 25));
        }
    }

    internal class Fish : IComponent
    {
        public Vector2 velocity;
        public Vector2 position;
        public Vector2 acceleration { get; set; }

        public float Vision = 100;
        public float Alpha;
        public float MaxForce = 0.003f;
        public float MaxVelocity = 0.2f;
        public int Range = 25;
        public Location? Parent;

        public List<Fish> AdjFish = new List<Fish>();

        public Fish(Flock osSucksAtBedwars)
        {
        }

        Vector2 Limit(Vector2 vec, float val)
        {
            if (vec.LengthSquared() > val * val)
                return Vector2.Normalize(vec) * val;
            return vec;
        }

        public Vector2 Seperation(int range)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);
            for (int j = 0; j < AdjFish.Count; j++)
            {
                var OtherFish = AdjFish[j];
                float dist = Vector2.DistanceSquared(position, OtherFish.position);
                if (dist < range * range && dist > 0)
                {
                    Vector2 d = position - OtherFish.position;
                    Vector2 norm = Vector2.Normalize(d);
                    Vector2 weight = norm / dist;
                    sum += weight;
                    count++;
                }
            }
            if (count > 0)
            {
                sum /= count;
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public Vector2 StayWithinRegion(int range)
        {
            Vector2 sum = new Vector2(0, 0);
            Point tilePos = position.ToPoint();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    float pdist = Vector2.DistanceSquared(Parent.DrawPosition, new Vector2(tilePos.X + i * 16, tilePos.Y + j * 16));
                    if (pdist > range * range && pdist > 0)
                    {
                        Vector2 d = position - new Vector2(tilePos.X + i * 16, tilePos.Y + j * 16);
                        Vector2 norm = Vector2.Normalize(d);
                        Vector2 weight = norm;
                        sum += weight;
                    }
                }
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }

            return Vector2.Zero;
        }

        public Vector2 Allignment(int range)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);
            for (int j = 0; j < AdjFish.Count; j++)
            {
                var OtherFish = AdjFish[j];
                float dist = Vector2.DistanceSquared(position, OtherFish.position);
                if (dist < range * range && dist > 0)
                {
                    sum += OtherFish.velocity;
                    count++;
                }
            }
            if (count > 0)
            {
                sum /= count;
            }
            if (sum != Vector2.Zero)
            {
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public Vector2 Cohesion(int range)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);
            for (int j = 0; j < AdjFish.Count; j++)
            {
                var OtherFish = AdjFish[j];
                float dist = Vector2.DistanceSquared(position, OtherFish.position);
                if (dist < range * range && dist > 0)
                {
                    sum += OtherFish.position;
                    count++;
                }
            }

            if (count > 0)
            {
                sum /= count;
                sum -= position;
                sum = Vector2.Normalize(sum) * MaxVelocity;
                Vector2 acc = sum - velocity;
                return Limit(acc, MaxForce);
            }
            return Vector2.Zero;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Utils.DrawClosedCircle(position, 2, 1, Color.Red * Alpha);
        }

        public void ApplyForces()
        {
            velocity += acceleration;
            velocity = Limit(velocity, MaxVelocity);
            position += velocity;
            acceleration *= 0;
        }

        public void Update()
        {
            //arbitrarily weight
            acceleration += Seperation(Range) * ((Range - 5) / 25f + 1);
            acceleration += Allignment(50) * 1f;
            acceleration += Cohesion(50) * 1f;
            acceleration += StayWithinRegion(50) * 5f;

            ApplyForces();
        }
    }

    internal class Flock : Manager<Fish>
    {
        public string flockTex;
        public float fishScale;
        public int randMin;
        public int randMax;

        public float Vision;
        public float Alpha;
        public float MaxForce;
        public float MaxVel;
        public float Range;

        public Flock(string tex, float scale, int randmin, int randmax)
        {
            flockTex = tex;
            fishScale = scale;
            randMin = randmin;
            randMax = randmax;
        }

        internal void Populate(Vector2 position, int amount, int Vision, Location Parent)
        {
            for (int i = 0; i < amount; i++)
            {
                if (Components.Count < 60)
                {
                    Fish fish = new Fish(this)
                    {
                        position = position,
                        velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)),
                        Parent = Parent,
                        Vision = Vision
                    };
                    Components.Add(fish);
                }
            }
        }

        public void OnUpdate()
        {
            foreach (Fish fish in Components.ToArray())
            {
                fish.Vision = Vision;
                fish.Alpha = Alpha;
                fish.MaxForce = MaxForce;
                fish.MaxVelocity = MaxVel;
                fish.Range = (int)Range;

                if (fish != null)
                {
                    fish.AdjFish.Clear();
                    foreach (Fish adjfish in Components)
                    {
                        if (!fish.Equals(adjfish))
                        {
                            if (Vector2.DistanceSquared(fish.position, adjfish.position) < fish.Vision * fish.Vision)
                            {
                                fish.AdjFish.Add(adjfish);
                            }
                        }
                    }
                }
            }
        }
    }
}

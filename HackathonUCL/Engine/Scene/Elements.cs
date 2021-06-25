using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HackathonUCL
{
    public class Element
    {
        public Element(Action UpdateMethod, Action<SpriteBatch> DrawMethod)
        {
            this.UpdateMethod = UpdateMethod;
            this.DrawMethod = DrawMethod;
            
        }
        public Element()
        {
            Size = Vector2.One;
        }
        public Element(bool Add)
        {
            Size = Vector2.One;
            if(Add)
            Main.sceneManager.CurrentScene.Elements.Add(this);
        }
        public float rotation;

        public float Alpha = 1;
        public Vector2 Size { get; set; }

        public Color Color = Color.White;
        public Vector2 Position { get; set; }

        public Vector2 Center { get => Position + Size / 2; set => Position = value - Size / 2; }
        public Vector2 PosVelocity { get; set; }
        public Vector2 SizeVelocity { get; set; }

        public Action UpdateMethod;

        public Action<SpriteBatch> DrawMethod;
        public bool IsBeingClickedOn() => Utils.isClicking;


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawMethod?.Invoke(spriteBatch);
        }
        public void Update()
        {
            Size += SizeVelocity;
            Position += PosVelocity;
            if (UpdateMethod != null)
                UpdateMethod.Invoke();
        }
    }
    public class TexturedElement : Element
    {
        public Texture2D Texture { get; set; } = TextureCache.pixel;

        public float RotationSpeed;
        public TexturedElement(Scene scene)
        {
            Size = Vector2.One;
            scene.Elements.Add(this);
        }

        public TexturedElement(Scene scene, Texture2D texture)
        {
            Size = Vector2.One;
            scene.Elements.Add(this);
            Texture = texture;
        }

        public TexturedElement()
        {
            Size = Vector2.One;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            rotation += RotationSpeed;
            spriteBatch.Draw(Texture, new Rectangle(Position.ToPoint(), Size.ToPoint()), Texture.Bounds, Color* Alpha, rotation, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
    public class StringedElement : Element
    {
        internal string Text = "";

        public StringedElement(Scene scene)
        {
            Size = Vector2.One;
                scene.Elements.Add(this);
        }

        public StringedElement()
        {
            Size = Vector2.One;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawText(Text, Color, Center, rotation, Size);
        }
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ludum_dare_24
{
    public class Sprite // I have shared this class, so I copied and pasted it here.
    {
        Texture2D sprite;
        public Vector2 Position;
        public Color color;
        double rotation = 0;
        float scale = 1.0f;
        int layer = 0;

        public bool flipped = false;
        public string Texture;



        public Sprite(ContentManager content, string texture, Vector2 position)
        {

            Texture = texture;
            sprite = content.Load<Texture2D>(texture);
            Position = position;
            color = Color.White;
        }

        public Vector2 GetRadius()
        {
            return new Vector2(sprite.Width, sprite.Height);
        }

        public void SetLayer(int newlayer)
        {
            layer = newlayer;
        }

        public int GetLayer()
        {
            return layer;
        }

        public void UpdatePosition(Vector2 new_pos)
        {
            Position = new_pos;
        }

        public double GetRotation()
        {
            return rotation;
        }

        public void SetTexturePixels(bool[,] pixels)
        {
            Color[] data = new Color[sprite.Width * sprite.Height];
            sprite.GetData(data);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
                int row = (i / sprite.Height);
                int pos = (i - (row * sprite.Width));
                if (pixels[pos, row] == true)
                    data[i] = Color.Black;
            }
            sprite.GraphicsDevice.Textures[0] = null;
            sprite.SetData(data);
        }

        public void SetScale(float newscale)
        {
            scale = newscale;
        }

        public float GetScale()
        {
            return scale;
        }

        public void SetRotation(double newrotation)
        {
            rotation = newrotation;
        }
        public void SetColor(Color Color)
        {
            color = Color;
        }

        public void Settransculency(float trans)
        {
            color = color * trans;
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 Position2 = GetPosition();
            float radians = 0;
            Vector2 origin = Vector2.Zero;
            radians = MathHelper.ToRadians((float)rotation);
            if (rotation > 0)
            {
                origin.X = (sprite.Width / 2);
                origin.Y = (sprite.Height / 2);
                Position2.X = Position.X + (sprite.Width / 2);
                Position2.Y = Position.Y + (sprite.Height / 2);
            }
            if (sprite != null)
            {
                if (flipped == false)
                    batch.Draw(sprite, Position2, null, color, radians, origin, scale, SpriteEffects.None, 0f);
                if (flipped == true)
                    batch.Draw(sprite, Position2, null, color, radians, origin, scale, SpriteEffects.FlipHorizontally, 0f);
            }
        }

    }



}

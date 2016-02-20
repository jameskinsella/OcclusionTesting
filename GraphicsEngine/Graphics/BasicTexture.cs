using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame;
namespace GraphicsEngine.Graphics
{
    public class BasicTexture : CustomEffectModel
    {
        public class BasicTextureMaterial : Material
        {
            public Vector3 Color { get; set; }
            public Texture2D Texture { get; set; }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["Color"] != null)
                    effect.Parameters["Color"].SetValue(Color);

                if (effect.Parameters["Texture"] != null)
                    effect.Parameters["Texture"].SetValue(Texture);
            }
        }

        private Random random = new Random();
        private float elapsed = 0;

        public BasicTexture(string id, string asset, Vector3 position) : base(id, asset, position)
        {

        }

        public override void LoadContent()
        {

            Material = new BasicTextureMaterial()
            {
                Color = Color.Green.ToVector3(),
                Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand")
            };
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects\\BasicTextureEffect");

            base.LoadContent();
        }
    }
}
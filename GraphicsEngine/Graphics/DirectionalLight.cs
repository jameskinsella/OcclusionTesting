using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine.Graphics
{
    public class DirectionalLight : CustomEffectModel
    {
        public class LamberMaterial : Material
        {
            public Vector3 AmbientColor { get; set; }
            public Vector3 LightDirection { get; set; }
            public Vector3 LightColor { get; set; }
            public Texture2D Texture { get; set; }
            public bool TextureEnabled { get; set; }
            public Vector3 DiffuseColor { get; set; }

            public LamberMaterial() : base()
            {
                TextureEnabled = true;
            }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["DiffuseColor"] != null)
                    effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);

                if (effect.Parameters["AmbientColor"] != null)
                    effect.Parameters["AmbientColor"].SetValue(AmbientColor);

                if (effect.Parameters["LightDirection"] != null)
                    effect.Parameters["LightDirection"].SetValue(LightDirection);

                if (effect.Parameters["LightColor"] != null)
                    effect.Parameters["LightColor"].SetValue(LightColor);

                //if (effect.Parameters["TextureEnabled"] != null)
                //    effect.Parameters["TextureEnabled"].SetValue(TextureEnabled);

                //if (effect.Parameters["Texture"] != null)
                //    effect.Parameters["Texture"].SetValue(Texture);

                base.SetEffectParameters(effect);
            }

        }

        public DirectionalLight(string id, string asset, Vector3 position) : base(id, asset, position)
        {

        }

        public override void LoadContent()
        {
            Material = new LamberMaterial()
            {
                //Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand"),
                LightColor = Color.PeachPuff.ToVector3(),
                LightDirection = new Vector3(1, 1, 0),
                AmbientColor = Color.RosyBrown.ToVector3(),
                //TextureEnabled = true,
                DiffuseColor = Color.SandyBrown.ToVector3()
            };

            CustomEffect = GameUtilities.Content.Load<Effect>("Effects\\DirectionalLight");

            base.LoadContent();
        }

        public override void Update()
        {
            base.Update();
        }

    }
}

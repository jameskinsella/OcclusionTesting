using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine.Graphics
{
    public class PointLight : CustomEffectModel
    {
        public class LambertPointLightMaterial : Material
        {
            public Vector3 AmbientLightColor { get; set; }
            public Vector3 LightPosition { get; set; }
            public Vector3 LightColor { get; set; }

            public float LightAttenuation { get; set; }
            public float LightFallOff { get; set; }

            public Texture2D Texture { get; set; }
            public bool TextureEnabled { get; set; }
            public Vector3 DiffuseColor { get; set; }
            public LambertPointLightMaterial() : base()
            {

                LightFallOff = 2;
                LightAttenuation = 40;
                TextureEnabled = true;
            }

            public LambertPointLightMaterial(Color ambientColor, Color lightColor, Vector3 lightPosition, float attenuation, float falloff) : base()
            {
                
            }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["DiffuseColor"] != null)
                    effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);

                if (effect.Parameters["AmbientLightColor"] != null)
                    effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);

                if (effect.Parameters["LightPosition"] != null)
                    effect.Parameters["LightPosition"].SetValue(LightPosition);

                if (effect.Parameters["LightColor"] != null)
                    effect.Parameters["LightColor"].SetValue(LightColor);

                if (effect.Parameters["LightAttenuation"] != null)
                    effect.Parameters["LightAttenuation"].SetValue(LightAttenuation);

                if (effect.Parameters["LightFallOff"] != null)
                    effect.Parameters["LightFallOff"].SetValue(LightFallOff);


                base.SetEffectParameters(effect);
            }

            public override void Update()
            {


                if (InputEngine.IsKeyHeld(Keys.Up))
                {
                    LightPosition = LightPosition + new Vector3(0, 1, 0);
                }
                if (InputEngine.IsKeyHeld(Keys.Down))
                {
                    LightPosition = LightPosition + new Vector3(0, -1, 0);
                }
                if (InputEngine.IsKeyHeld(Keys.Left))
                {
                    LightPosition = LightPosition + new Vector3(-1, 0, 0);
                }
                if (InputEngine.IsKeyHeld(Keys.Right))
                {
                    LightPosition = LightPosition + new Vector3(1, 0, 0);
                }
                if (InputEngine.IsKeyHeld(Keys.Home))
                {
                    LightPosition = LightPosition + new Vector3(0, 0, 1);
                }
                if (InputEngine.IsKeyHeld(Keys.End))
                {
                    LightPosition = LightPosition + new Vector3(0, 0, -1);
                }
                base.Update();
            }


        }

        public PointLight(string id, string asset, Vector3 position) : base(id, asset, position)
        {

        }

        public override void LoadContent()
        {
            Material = new LambertPointLightMaterial()
            {
                LightColor = Color.White.ToVector3(),
                LightPosition = new Vector3(1, 1, 0),
                AmbientLightColor = Color.Black.ToVector3(),
                DiffuseColor = Color.White.ToVector3(),
                //LightAttenuation = 40
            };
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects\\PointLight");
            base.LoadContent();
        }

        public override void Update()
        {
            Material.Update();
            
            base.Update();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine.Graphics
{
    public class BasicColor : CustomEffectModel
    {
        public class ColorMaterial : Material
        {
            public Vector3 Color { get; set; }

            public override void SetEffectParameters(Effect effect)
            {
                if (effect.Parameters["Color"] != null)
                    effect.Parameters["Color"].SetValue(Color);
            }
        }

        public BasicColor(string id, string asset, Vector3 position) : base(id, asset, position)
        {

        }

        public override void LoadContent()
        {
            Material = new ColorMaterial() { Color = Color.Green.ToVector3() };
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects\\BasicColorEffect");
            base.LoadContent();
        }

    }
}

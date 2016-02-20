using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine
{
    public class BasicColorModel : CustomEffectModel
    {
        public Color Color { get; set; }
        public BasicColorModel(string id, string asset, Vector3 position, Color color) : base(id, asset, position)
        {
            Color = color;
        }

        public override void LoadContent()
        {
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects\\BasicColorEffect");
            base.LoadContent();
        }

        public override void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = CustomEffect;

                    part.Effect.Parameters["World"].SetValue(BoneTransforms[mesh.ParentBone.Index] * World);
                    part.Effect.Parameters["View"].SetValue(camera.View);
                    part.Effect.Parameters["Projection"].SetValue(camera.Projection);

                    part.Effect.Parameters["Color"].SetValue(Color.ToVector3());
                }
                mesh.Draw();
            }

            //base.Draw(camera);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine
{
    public class BasicTextureModel : CustomEffectModel
    {
        public Texture2D Texture { get; set; }
        public BasicTextureModel(string id, string asset, Vector3 position) : base(id, asset, position)
        {
        }

        public override void LoadContent()
        {
            CustomEffect = GameUtilities.Content.Load<Effect>("Effects\\BasicTextureEffect");
            Texture = GameUtilities.Content.Load<Texture2D>("Textures\\sand");
            base.LoadContent();
        }

        //public override void Draw(Camera camera)
        //{
        //    foreach (ModelMesh mesh in Model.Meshes)
        //    {
        //        foreach (ModelMeshPart part in mesh.MeshParts)
        //        {
        //            part.Effect = CustomEffect;

        //            part.Effect.Parameters["World"].SetValue(BoneTransforms[mesh.ParentBone.Index] * World);
        //            part.Effect.Parameters["View"].SetValue(camera.View);
        //            part.Effect.Parameters["Projection"].SetValue(camera.Projection);

        //            part.Effect.Parameters["Color"].SetValue(Color.White.ToVector3());
        //            part.Effect.Parameters["Texture"].SetValue(Texture);
        //        }
        //        mesh.Draw();
        //    }

        //    //base.Draw(camera);
        //}

    }
}

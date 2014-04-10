using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using IRT.Engine;

namespace IRT.Viewer
{
    class Drawable : IDrawable
    {
        private Vector3 dimensions, position;
        private Model mesh;

        public Drawable(Model mesh, IShape shape)
        {
            this.dimensions = shape.Dimensions;
            this.position = shape.Position;
            this.mesh = mesh;
        }

        public void Draw()
        {
            Matrix[] transforms = new Matrix[this.mesh.Bones.Count];
            this.mesh.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mm in mesh.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mm.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mm.ParentBone.Index] * Matrix.CreateScale(this.dimensions) * Matrix.CreateTranslation(this.position);
                    effect.View = Matrix.CreateLookAt(cameraPosition,
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), 16f / 9f,
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mm.Draw();
            }
        }
    }
}

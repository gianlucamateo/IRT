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

        private Camera cam;

        public Drawable(Model mesh, IShape shape, Camera camera)
        {
            this.dimensions = shape.Dimensions;
            this.position = shape.Position;
            this.mesh = mesh;
            this.cam = camera;
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
                    effect.View = cam.ViewMatrix;//Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);//cam.ViewMatrix;
                    effect.Projection = cam.ProjectionMatrix;//Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 16f / 9f, 1, 100);//cam.ProjectionMatrix;
                    effect.Alpha = 0.5f;
                }
                // Draw the mesh, using the effects set above.
                mm.Draw();
            }
        }
    }
}

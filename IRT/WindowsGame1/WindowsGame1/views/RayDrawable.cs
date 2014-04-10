using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using IRT.Engine;
using Ray = IRT.Engine.Ray;

namespace IRT.Viewer
{
    class RayDrawable : IDrawable
    {
        private Vector3 dimensions;
        private Model mesh;

        private Ray ray;

        private Camera cam;

        public RayDrawable(Model mesh, Ray ray, Camera camera)
        {
            this.dimensions = 0.005f * Vector3.One;
            this.mesh = mesh;
            this.cam = camera;
            this.ray = ray;
        }

        public void Draw()
        {
            Matrix[] transforms = new Matrix[this.mesh.Bones.Count];
            this.mesh.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.

            Matrix scale = Matrix.CreateScale(this.dimensions);
            Vector3[] positions = ray.segments.ToArray<Vector3>();
            for (int i = 0; i < positions.Length; i++)
            {
                if (i % 3 == 0)
                {
                    foreach (ModelMesh mm in mesh.Meshes)
                    {
                        // This is where the mesh orientation is set, as well 
                        // as our camera and projection.
                        foreach (BasicEffect effect in mm.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = transforms[mm.ParentBone.Index] * scale * Matrix.CreateTranslation(positions[i]);
                            effect.View = cam.ViewMatrix;//Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);//cam.ViewMatrix;
                            effect.Projection = cam.ProjectionMatrix;//Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 16f / 9f, 1, 100);//cam.ProjectionMatrix;
                            effect.Alpha = 0.5f;
                            effect.DiffuseColor = Vector3.UnitX;
                        }
                        // Draw the mesh, using the effects set above.
                        mm.Draw();
                    }
                }
            }

        }
    }
}

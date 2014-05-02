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
        private IShape shape;
        private Camera cam;
		private Texture2D texture;

		public Color Color { get; set; }
		public float Transparency { get; set; }

        public Drawable(Model mesh, IShape shape, Camera camera, float transparency = 0.95f, Color? color = null, Texture2D texture = null)
        {
            this.dimensions = shape.Dimensions;
            this.position = shape.Position;
            this.mesh = mesh;
            this.cam = camera;
            this.shape = shape;
			this.Color = color == null ? Color.LightCyan : (Color)color;
			this.Transparency = transparency;
			this.texture = texture;
        }

        public void Draw(int timestamp)
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
                    //effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = transforms[mm.ParentBone.Index] * Matrix.CreateScale(this.dimensions) * Matrix.CreateTranslation(this.position);
                    effect.View = cam.ViewMatrix;
                    effect.Projection = cam.ProjectionMatrix;
                    effect.Alpha = 1.0f - this.Transparency;
                    effect.DiffuseColor = this.Color.ToVector3();

					if (this.texture != null)
					{
						effect.TextureEnabled = true;
						effect.Texture = this.texture;
					}
					else
					{
						effect.TextureEnabled = false;
					}
                }
                // Draw the mesh, using the effects set above.
                mm.Draw();
            }
        }


        public int getZ()
        {
            Shape s = (Shape)(this.shape);
            return s.zIndex;
        }
    }
}

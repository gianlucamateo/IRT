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
			this.dimensions = 0.007f * Vector3.One;
			this.mesh = mesh;
			this.cam = camera;
			this.ray = ray;
		}

		public void Draw()
		{
			Matrix[] transforms = new Matrix[this.mesh.Bones.Count];
			this.mesh.CopyAbsoluteBoneTransformsTo(transforms);

			Matrix scale = Matrix.CreateScale(this.dimensions);
			Vector3[] positions = ray.segments.ToArray<Vector3>();

			Color rayColor = WavelengthToColor (ray.Wavelength);
			for (int i = 0; i < positions.Length; i++)
			{
				if (i % 10 == 0)
				{
					foreach (ModelMesh mm in mesh.Meshes)
					{
						// This is where the mesh orientation is set, as well 
						// as our camera and projection.
						foreach (BasicEffect effect in mm.Effects)
						{
							effect.EnableDefaultLighting();
							effect.LightingEnabled = false;
							effect.World = transforms[mm.ParentBone.Index] * scale * Matrix.CreateTranslation(positions[i]);
							effect.View = cam.ViewMatrix;
							effect.Projection = cam.ProjectionMatrix;
							effect.Alpha = 1f;
							effect.DiffuseColor = rayColor.ToVector3 ();
						}
						// Draw the mesh, using the effects set above.
						mm.Draw();
					}
				}
			}
		}

		private static Color WavelengthToColor (float wavelength)
		{
			// takes wavelength in nm and returns an rgba value
			float r, g, b, alpha, wl = wavelength;

			if (wl >= 380f && wl < 440f) {
				r = -1f * (wl - 440f) / (440 - 380f);
				g = 0f;
				b = 1f;
			}
			else if (wl >= 440f && wl < 490f) {
				r = 0;
				g = (wl - 440f) / (490f - 440f);
				b = 1f;
			}
			else if (wl >= 490f && wl < 510f) {
				r = 0f;
				g = 1f;
				b = -1 * (wl - 510f) / (510f - 490f);
			}
			else if (wl >= 510f && wl < 580f) {
				r = (wl - 510f) / (580f - 510f);
				g = 1f;
				b = 0f;
			}
			else if (wl >= 580f && wl < 645f) {
				r = 1f;
				g = -1 * (wl - 645f) / (645f - 580f);
				b = 0f;
			}
			else if (wl >= 645f && wl <= 780f) {
				r = 1f;
				g = 0f;
				b = 0f;
			}
			else {
				r = 0f;
				g = 0f;
				b = 0f;
			}

			// intensty is lower at the edges of the visible spectrum.
			if (wl > 780 || wl < 380) {
				alpha = 0;
			}
			else if (wl > 700) {
				alpha = (780 - wl) / (780 - 700);
			}
			else if (wl < 420) {
				alpha = (wl - 380) / (420 - 380);
			}
			else {
				alpha = 1;
			}

			return new Color (r, g, b, alpha);
		}
	}
}

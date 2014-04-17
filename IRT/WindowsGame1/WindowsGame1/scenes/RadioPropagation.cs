using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IRT.Engine;
using Ray = IRT.Engine.Ray;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace IRT.Viewer
{
	class RadioPropagation : Scene
	{
		public RadioPropagation(ContentManager cm, Camera cam)
			: base(cm, cam)
		{
			this.maxCount = 1200;
			this.maxSpawns = 3;
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			this.rays = rays;

			Sphere earth = new Sphere(Vector3.Zero, 0.5f, 5);
			earth.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 1000f,
				Vector3.Zero);
			space.AddShape(earth);

			space.SpawnCluster(new Vector3(-2, 0, 0), Vector3.UnitX, 600f, 650f, 1);

			Drawable dEarth = new Drawable(sphereModel, earth, cam);
			shapes.Add(dEarth);			
		}
	}
}

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
	class InhomoCube : Scene
	{
		public InhomoCube(ContentManager cm, Camera cam)
			: base(cm, cam)
		{
			this.maxCount = 2000;
			this.maxSpawns = 10;
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			this.rays = rays;

			Shape cuboid = new Cuboid(Vector3.Zero, 4f, 1f, 1f, 0, 1f);
			cuboid.Inhomogeniety = new Inhomogeneity((x, y, z) => -10 * (y - 1f), lambda => 1f + 0.001f * lambda, Vector3.Zero);

			space.AddShape(cuboid);

			Vector3 spawnPoint = new Vector3(-0.8f, 0.3f, 0f);
			Vector3 spawnDirection = new Vector3(1f, 1f, 0);

			space.SpawnCluster(spawnPoint, spawnDirection, 475f, 650f, 5);

			Drawable drawCuboid = new Drawable(cubeModel, cuboid, cam);

			shapes.Add(drawCuboid);
		}
	}
}

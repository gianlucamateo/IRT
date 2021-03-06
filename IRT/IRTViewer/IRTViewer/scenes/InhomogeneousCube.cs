﻿using System;
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
			this.maxCount = 20000;
			this.maxSpawns = 10;

			cam.Position = new Vector3(-10f, 0f, 1f);
			cam.CameraMatrix = Matrix.Invert(Matrix.CreateLookAt(cam.Position, Vector3.Zero, Vector3.UnitY));
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			this.rays = rays;

			Shape cuboid = new Cuboid(Vector3.Zero, 4f, 2f, 1f, 0, 1f);
			cuboid.Inhomogeniety = new Inhomogeneity((x, y, z) => 1 + 30 * (float)Math.Sqrt(y + 4f) + 2 * z, lambda => 1f + 0.0003f * lambda, Vector3.Zero);

			space.AddShape(cuboid);

			Vector3 spawnPoint = new Vector3(-0.8f, -0.8f, 0f);
			Vector3 spawnDirection = new Vector3(1f, 0, 0);

			space.SpawnCluster(spawnPoint, spawnDirection, 475f, 650f, 5);

			Drawable drawCuboid = new Drawable(cubeModel, cuboid, cam,transparency: 0.7f);

			shapes.Add(drawCuboid);
		}
	}
}

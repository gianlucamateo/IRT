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
	class FataMorgana : Scene
	{
		public FataMorgana(ContentManager cm, Camera cam)
			: base(cm, cam)
		{
			this.maxCount = 2000;
			this.maxSpawns = 10;
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			this.rays = rays;

			Shape coldAir = new Cuboid(Vector3.Zero, 4f, 0.2f, 1f, 0, 1f);
			coldAir.Inhomogeniety = new Inhomogeneity((x, y, z) => 1f, lambda => 1f, Vector3.Zero);

			Shape warmAir = new Cuboid(new Vector3(0, 0.15f, 0), 4, 0.1f, 1f, 0);
			warmAir.Inhomogeniety = new Inhomogeneity((x, y, z) => calc(y) , lambda => 1f, Vector3.Zero);

			space.AddShape(warmAir);
			space.AddShape(coldAir);

			Vector3 spawnPoint = new Vector3(1.5f, 0f, 0f);
			Vector3 spawnDirection = new Vector3(-1f, 0.1f, 0);

			space.SpawnCluster(spawnPoint, spawnDirection, 475f, 650f, 1);

			Drawable drawCold = new Drawable(cubeModel, coldAir, cam);
			Drawable drawWarm = new Drawable(cubeModel, warmAir, cam); 

			shapes.Add(drawCold);
			shapes.Add(drawWarm);
		}
		public float calc(float y)
		{
			return 1f;
		}
	}
}

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
	class InferiorMirage : Scene
	{
		public InferiorMirage(ContentManager cm, Camera cam)
			: base(cm, cam)
		{
			this.maxCount = 8000;
			this.maxSpawns = 10;

			cam.Position = new Vector3(-6f, 0f, 3f);
			cam.CameraMatrix = Matrix.Invert(Matrix.CreateLookAt(cam.Position, Vector3.Zero, Vector3.UnitY));
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			
			this.rays = rays;

			Shape coldAir = new Cuboid(Vector3.Zero, 8f, 0.2f, 1f, 0, 1f, interact:false);
			coldAir.Inhomogeniety = new Inhomogeneity((x, y, z) => 1f, lambda => 1f, Vector3.Zero);

			Shape warmAir = new Cuboid(new Vector3(0, -0.25f, 0), 8, 0.3f, 1f, 0, interact:false);
			warmAir.Inhomogeniety = new Inhomogeneity((x, y, z) => calc(y) , lambda => 1f, Vector3.Zero);

			space.AddShape(warmAir);
			space.AddShape(coldAir);

			Vector3 spawnPoint = new Vector3(3.9f, 0.05f, 0f);
			Vector3 spawnDirection = new Vector3(-1f, -0.1f, 0);

			space.SpawnCluster(spawnPoint, spawnDirection, 650f, 650f, 1);			
			space.SpawnCluster(spawnPoint - new Vector3(0, 0.05f, 0), spawnDirection+new Vector3(0,0.04f,0), 500f, 500f, 1);
			

			Drawable drawCold = new Drawable(cubeModel, coldAir, cam, transparency: 0.85f);
			Drawable drawWarm = new Drawable(cubeModel, warmAir, cam, transparency: 0.85f,color:Color.Red);
			Drawable stickM = new Drawable(stickMan, new Cuboid(new Vector3(-2.4f,-0.4f,0.08f), 0.00035f, 0.00035f, 0.00035f, 0, 1f, false), cam, transparency: 0.5f);

			shapes.Add(drawCold);
			shapes.Add(drawWarm);
			shapes.Add(stickM);
		}
		public float calc(float y)
		{
			float initial = 1.02f;
			
			return initial + 16.5f*(y-0.1f);
		}
	}
}

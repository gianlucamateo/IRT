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
	class AstronomicRefraction : Scene
	{
		public AstronomicRefraction(ContentManager cm, Camera cam)
			: base(cm, cam)
		{
			this.maxCount = 4000;
			this.maxSpawns = 2;

			cam.Position = new Vector3(0f, 0f, 18f);
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			this.rays = rays;

			InitNightConfig(shapes);
		}

		private void InitNightConfig(List<IDrawable> shapes)
		{
			Vector3 nightPos = new Vector3(0f, 0f, 0f);
			Func<float, float> inhomo = r => 30 / r - 14;

			// Earth
			Shape earth = new Sphere(nightPos, 1f, 5);
			earth.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 80f,
				nightPos);
			space.AddShape(earth);

			// atmosphere
			Shape troposphere = new Sphere(nightPos, 2f, 4);
			troposphere.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 1f,
				nightPos
				);
			space.AddShape(troposphere);

			// Spawn radio waves
			space.SpawnCluster (nightPos + new Vector3 (-1.0001f, 0, 0f), new Vector3 (0f, 1f, 0f), 600f, 650f, 1);

			Texture2D earthTextureNight = Content.Load<Texture2D>("Textures\\earth_texture_night");

			// Add shapes to drawables
			shapes.Add(new Drawable(sphereModel, earth, cam, 0f, texture: earthTextureNight));
			shapes.Add(new Drawable(sphereModel, troposphere, cam));
		}
	}
}

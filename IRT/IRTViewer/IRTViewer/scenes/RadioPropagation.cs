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
			this.maxCount = 2000;
			this.maxSpawns = 8;

			cam.Position = new Vector3(0f, 0f, 18f);
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			this.rays = rays;

			InitDayConfig(shapes);
			InitNightConfig(shapes);
		}

		private void InitDayConfig(List<IDrawable> shapes)
		{
			Vector3 dayPos = new Vector3(-2f, 0f, 0f);
			Func<float, float> inhomo = r => 15f / (r * r);

			// Earth
			Shape earth = new Sphere(dayPos, 1f, 5);
			earth.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 1000f,
				dayPos);
			space.AddShape(earth);

			// Troposphere
			Shape troposphere = new Sphere(dayPos, 1.2f, 4);
			troposphere.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 1f,
				dayPos
				);
			space.AddShape(troposphere);

			// Ionosphere D-Layer
			Shape dLayer = new Sphere(dayPos, 1.3f, 3, 0.994f);
			dLayer.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 1f,
				dayPos
				);
			space.AddShape(dLayer);

			// Ionosphere E-Layer
			Shape eLayer = new Sphere(dayPos, 1.4f, 2);
			eLayer.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 1.7f,
				dayPos
				);
			space.AddShape(eLayer);

			// Ionosphere F-Layer
			Shape fLayer = new Sphere(dayPos, 1.6f, 1);
			fLayer.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 1.7f,
				dayPos
				);
			space.AddShape(fLayer);

			// Spawn radio waves
			space.SpawnCluster (dayPos + new Vector3 (-1.01f, 0f, 0f), new Vector3 (-0.5f, 1f, 0f), 600f, 650f, 1);
			space.SpawnCluster (dayPos + new Vector3 (-1.01f, 0f, 0f), new Vector3 (-0.25f, 1f, 0f), 600f, 650f, 1);
			space.SpawnCluster (dayPos + new Vector3 (-1.01f, 0f, 0f), new Vector3 (-0.75f, 1f, 0f), 600f, 650f, 1);


			Texture2D earthTexture = Content.Load<Texture2D>("Textures\\earth_texture");

			// Add shapes to drawables
			shapes.Add(new Drawable(sphereModel, earth, cam, 0f, texture: earthTexture));
			shapes.Add(new Drawable(sphereModel, troposphere, cam));
			shapes.Add(new Drawable(sphereModel, dLayer, cam));
			shapes.Add(new Drawable(sphereModel, eLayer, cam));
			shapes.Add(new Drawable(sphereModel, fLayer, cam));
		}

		private void InitNightConfig(List<IDrawable> shapes)
		{
			Vector3 nightPos = new Vector3(2f, 0f, 0f);
			Func<float, float> inhomo = r => 10f / (r*r);

			// Earth
			Shape earth = new Sphere(nightPos, 1f, 5);
			earth.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 80f,
				nightPos);
			space.AddShape(earth);

			// Troposphere
			Shape troposphere = new Sphere(nightPos, 1.2f, 4);
			troposphere.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 1f,
				nightPos
				);
			space.AddShape(troposphere);

			// No D-Layer at night

			// Ionosphere E-Layer
			Shape eLayer = new Sphere(nightPos, 1.5f, 2, 0.9995f);
			eLayer.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 1f,
				nightPos
				);
			space.AddShape(eLayer);

			// Ionosphere F-Layer
			Shape fLayer = new Sphere(nightPos, 1.6f, 1);
			fLayer.Inhomogeniety = new Inhomogeneity(
				inhomo,
				l => 20f,
				nightPos
				);
			space.AddShape(fLayer);

			// Spawn radio waves
			space.SpawnCluster (nightPos + new Vector3 (-1.01f, 0f, 0f), new Vector3 (-0.5f, 1f, 0f), 600f, 650f, 1);
			space.SpawnCluster (nightPos + new Vector3 (-1.01f, 0f, 0f), new Vector3 (-0.25f, 1f, 0f), 600f, 650f, 1);
			space.SpawnCluster (nightPos + new Vector3 (-1.01f, 0f, 0f), new Vector3 (-0.75f, 1f, 0f), 600f, 650f, 1);

			Texture2D earthTextureNight = Content.Load<Texture2D>("Textures\\earth_texture_night");

			// Add shapes to drawables
			shapes.Add(new Drawable(sphereModel, earth, cam, 0f, texture: earthTextureNight));
			shapes.Add(new Drawable(sphereModel, troposphere, cam));
			shapes.Add(new Drawable(sphereModel, eLayer, cam));
			shapes.Add(new Drawable(sphereModel, fLayer, cam));
		}
	}
}

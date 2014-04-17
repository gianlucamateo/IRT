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
			this.maxSpawns = 6;
		}

		public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
		{
			this.rays = rays;

			// Earth
			Sphere earth = new Sphere(Vector3.Zero, 1f, 5);
			earth.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 1000f,
				Vector3.Zero);
			space.AddShape(earth);

			// Troposphere
			Sphere troposphere = new Sphere(Vector3.Zero, 1.2f, 4);
			troposphere.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 1f,
				Vector3.Zero
				);
			space.AddShape(troposphere);

			// Ionosphere D-Layer
			Sphere dLayer = new Sphere(Vector3.Zero, 1.3f, 3);
			dLayer.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 1.2f,
				Vector3.Zero
				);
			space.AddShape(dLayer);

			// Ionosphere E-Layer
			Sphere eLayer = new Sphere(Vector3.Zero, 1.4f, 2);
			eLayer.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 1.4f,
				Vector3.Zero
				);
			space.AddShape(eLayer);

			// Ionosphere F-Layer
			Sphere fLayer = new Sphere(Vector3.Zero, 1.6f, 1);
			fLayer.Inhomogeniety = new Inhomogeneity(
				r => 1f,
				l => 1.7f,
				Vector3.Zero
				);
			space.AddShape(fLayer);

			// Spawn radio waves
			space.SpawnCluster(new Vector3(-1.05f, 0f, 0f), new Vector3(0f, 1f, 0f), 600f, 650f, 1);

			// Add shapes to drawables
			shapes.Add(new Drawable(sphereModel, earth, cam, 0f, color: Color.DarkGreen));
			shapes.Add(new Drawable(sphereModel, troposphere, cam));
			shapes.Add(new Drawable(sphereModel, dLayer, cam));
			shapes.Add(new Drawable(sphereModel, eLayer, cam));
			shapes.Add(new Drawable(sphereModel, fLayer, cam));
		}
	}
}

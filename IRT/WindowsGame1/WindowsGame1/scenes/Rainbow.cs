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
    class Rainbow : Scene
    {
        public Rainbow(ContentManager cm, Camera cam) : base(cm, cam) {
			this.maxCount = 1200;
			this.maxSpawns = 5;
		}

        public override void Load(List<IDrawable> rays, List<IDrawable> shapes)
        {
            this.rays = rays;

            Shape sphere = new Sphere(Vector3.Zero, 0.5f, 0);
            sphere.Inhomogeniety = new Inhomogeneity((x, y, z) => 1f,
                lambda => -0.013f / 400f * lambda + 1.353f,
                Vector3.Zero);
            space.AddShape(sphere);

            Vector3 spawnPoint = new Vector3(-1f, 0.31f, 0f);
            Vector3 spawnDirection = Vector3.UnitX;

            space.SpawnCluster(spawnPoint, spawnDirection, 475f, 650f, 7);
            space.SpawnCluster(spawnPoint - Vector3.UnitY * .7f, spawnDirection, 475f, 650f, 7);

            Drawable drawSphere = new Drawable(sphereModel, sphere, cam);

            shapes.Add(drawSphere);
        }
    }
}

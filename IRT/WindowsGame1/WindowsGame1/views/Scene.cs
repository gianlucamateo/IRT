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
    abstract class Scene : IScene
    {
        protected ContentManager Content;
        protected Camera cam;
        protected Space space;

        protected List<IDrawable> rays;

        protected Model c = null;
        protected Model s = null;

        public Scene(ContentManager cm, Camera cam)
        {
            this.Content = cm;
            this.cam = cam;

            this.space = new Space(1.0f);
            this.rays = null;

            c = Content.Load<Model>("Models\\cuboid");
            s = Content.Load<Model>("Models\\sphere");
        }

        public abstract void Load(List<IDrawable> rays, List<IDrawable> shapes);

        public void Run()
        {
            this.space.Update(count: 1200, maxSpawns: 5);

            foreach (Ray ray in this.space.finishedRays)
            {
                rays.Add(new RayDrawable(s, ray, this.cam));
            }
        }
    }
}

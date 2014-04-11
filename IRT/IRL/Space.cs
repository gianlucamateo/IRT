using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    public class Space
    {
        public const float RAY_RESOLUTION = 0.001f, COMPUTE_RESOLUTION = 0.0001f;
        public float refractionIndex;
        private List<Shape> shapes;
        public List<Ray> rays;
        public List<Ray> newlySpawned;
        public List<Ray> finishedRays;

        public Space(float refractionIndex)
        {
            this.shapes = new List<Shape>();
            this.rays = new List<Ray>();
            this.finishedRays = new List<Ray>();
            this.newlySpawned = new List<Ray>();
            this.refractionIndex = refractionIndex;
        }

        public void spawnRay(Vector3 position, Vector3 direction, float wavelength, float intensity = 1)
        {
            if (intensity > 0.0001)
            {
                this.newlySpawned.Add(new Ray(position, direction, this, wavelength, intensity));
            }
        }

        public void Update(int maxSpawns = 4, int count = 1)
        {
            int iterations = 0;

            do
            {
                rays = newlySpawned;
                newlySpawned = new List<Ray>();
                Console.WriteLine(rays.Count);
                for (int i = 0; i < count; i++)
                {
                    foreach (var ray in this.rays)
                    {
                        ray.propagate();
                    }
                }
                foreach (var ray in this.rays)
                {
                    finishedRays.Add(ray);
                }
                iterations++;
            } while (this.newlySpawned.Count > 0 && iterations < maxSpawns);
        }

        public Shape getMedium(Vector3 r)
        {
            foreach (Shape s in this.shapes)
            {
                if (s.isInside(r))
                {
                    return s;
                }
            }
            return null;
        }

        public void addShape(Shape shape)
        {
            this.shapes.Add(shape);
        }
    }


}

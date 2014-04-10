using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    public class Ray
    {
        public List<Vector3> segments;
        private float wavelength;
        private Vector3 position;
        private Vector3 direction;
        private Shape medium;
        private Space space;

        public Ray(Vector3 startPosition, Vector3 direction, Space space, float wavelength)
        {
            this.segments = new List<Vector3>();
            this.position = startPosition;
            this.direction = direction;
            direction.Normalize();
            this.space = space;
            this.medium = space.getMedium(position);
            this.wavelength = wavelength;
        }

        public void propagate()
        {
            Vector3 predictedPosition = position + direction * Space.RAY_RESOLUTION;

            Shape nextMedium = space.getMedium(predictedPosition);

            // Check for medium change
            if (nextMedium != medium)
            {
                Vector3 reflected, refracted;
                if (nextMedium == null)
                {
                    medium.interact(position, direction, out reflected, out refracted, space.refractionIndex, wavelength);
                }
                else
                {
                    nextMedium.interact(position, direction, out reflected, out refracted, medium.getRefractionIndex(position,wavelength), wavelength);
                }
                this.space.spawnRay(position, reflected, wavelength);
                this.space.spawnRay(predictedPosition, refracted, wavelength);

                //Console.WriteLine("Encountered medium change, refracting and reflecting...");
                return;
            }
            else
            {
                if (medium == null)
                {
                    this.position = predictedPosition;
                    this.segments.Add(position);
                    Console.WriteLine(position);

                    return;
                }
                Vector3 rayDir = direction, dirStep = Vector3.Zero;
                Vector3 sum = Vector3.Zero, doubleSum = Vector3.Zero;

                for (float dl = 0f; dl < Space.RAY_RESOLUTION; dl += Space.COMPUTE_RESOLUTION)
                {
                    dirStep = dl * rayDir;
                    Vector3 gradient = medium.getGradient(position + dirStep, wavelength);
                    //Console.WriteLine(gradient);
                    Vector3 dr = gradient * Space.COMPUTE_RESOLUTION * medium.getRefractionIndex(position + dirStep, wavelength);
                    sum += dr;
                    doubleSum += sum * Space.COMPUTE_RESOLUTION;
                }

                rayDir += doubleSum;
                rayDir.Normalize();

                this.position += rayDir * Space.RAY_RESOLUTION;
                this.direction = rayDir;
            }
            this.segments.Add(position);

            Console.WriteLine(position);
        }
    }
}

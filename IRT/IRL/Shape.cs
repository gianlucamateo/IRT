using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    public abstract class Shape : IShape
    {
        public Inhomogeneity Inhomogeniety;
        protected Vector3 Center;
        protected int zIndex;

        public Shape(Vector3 center, int zIndex = 0)
        {
            this.Center = center;
            this.zIndex = zIndex;
        }

        public Vector3 getGradient(Vector3 r, float wavelength, float step = Space.COMPUTE_RESOLUTION)
        {
            float dx = 0, dy = 0, dz = 0;

            Vector3 xDiff = new Vector3(step, 0, 0);
            Vector3 yDiff = new Vector3(0, step, 0);
            Vector3 zDiff = new Vector3(0, 0, step);

            // Calculate differentials
            dx = (getRefractionIndex(r + xDiff, wavelength) - getRefractionIndex(r, wavelength)) / step;
            dy = (getRefractionIndex(r + yDiff, wavelength) - getRefractionIndex(r, wavelength)) / step;
            dz = (getRefractionIndex(r + zDiff, wavelength) - getRefractionIndex(r, wavelength)) / step;

            Vector3 gradient = new Vector3(dx, dy, dz);
            return gradient;
        }

        public float getRefractionIndex(Vector3 r, float wavelength)
        {
            return this.Inhomogeniety.Evaluate(r - this.Center, wavelength);
        }

        public abstract bool isInside(Vector3 r);

        public abstract void interact(Vector3 r, Vector3 incident, out Vector3 reflected, out Vector3 refracted, float outerRefractionIndex, float wavelength);

        public Vector3 Position
        {
            get { return this.Center; }
        }

        public abstract Vector3 Dimensions { get; }
    }
}

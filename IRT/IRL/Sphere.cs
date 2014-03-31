using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    class Sphere : Shape, IShape
    {
        private float Radius;

        public Sphere(Vector3 center, float radius, Inhomogeneity i)
        {
            this.Center = center;
            this.Inhomogeniety = i;
            this.Radius = radius;
        }



        public bool isInside(Vector3 r)
        {
            Vector3 diff = r - Center;
            return diff.Length() <= Radius;
        }

        public Vector3 getGradient(Vector3 r)
        {
            throw new NotImplementedException();
        }

        public float getRefractionIndex(Vector3 r)
        {
            throw new NotImplementedException();
        }
    }
}

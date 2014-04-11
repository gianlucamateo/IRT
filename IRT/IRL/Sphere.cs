using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    public class Sphere : Shape, IShape
    {
        private float Radius;

        public Sphere(Vector3 center, float radius, int zIndex = 0) : base(center, zIndex) 
        {
            this.Radius = radius;
        }

        public override bool isInside(Vector3 r)
        {
            Vector3 diff = r - Center;
            return diff.Length() <= Radius;
        }


        public override Vector3 Dimensions
        {
            get { return Vector3.One * this.Radius*2; }
        }

        public override Vector3 getNormal(Vector3 r)
        {
            Vector3 normal = (r - Center);
            normal.Normalize();
            return normal;
        }
    }
}

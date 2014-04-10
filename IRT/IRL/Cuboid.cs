using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    public class Cuboid : Shape, IShape
    {
        float heightY, widthX, depthZ;

        public Cuboid(Vector3 center, float heightY, float widthX, float depthZ, int zIndex = 0) : base(center, zIndex)
        {
            this.heightY = heightY;
            this.widthX = widthX;
            this.depthZ = depthZ;
        }

        public override bool isInside(Vector3 r)
        {
            float x = r.X;
            float y = r.Y;
            float z = r.Z;

            if (x > Center.X + widthX / 2 || x < Center.X - widthX / 2)
                return false;
            if (y > Center.Y + heightY / 2 || y < Center.Y - heightY / 2)
                return false;
            if (z > Center.Z + depthZ / 2 || z < Center.Z - depthZ / 2)
                return false;

            return true;
        }

        public override void interact(Vector3 r, Vector3 incident, out Vector3 reflected, out Vector3 refracted, float outerRefractionIndex, float wavelength)
        {
            throw new NotImplementedException();
        }

        public override Vector3 Dimensions
        {
            get { return new Vector3(this.widthX, this.heightY, this.depthZ); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    class Cuboid : Shape, IShape
    {
        float heightY, widthX, depthZ;

        public Cuboid(Vector3 center, float heightY, float widthX, float depthZ)
        {
            this.Center = center;
            this.heightY = heightY;
            this.widthX = widthX;
            this.depthZ = depthZ;
        }

        public bool isInside(Vector3 r)
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

        public Vector3 getGradient(Vector3 r, float wavelength, float step = Space.COMPUTE_RESOLUTION)
        {
            throw new NotImplementedException();
        }

        public float getRefractionIndex(Microsoft.Xna.Framework.Vector3 r, float wavelength)
        {
            throw new NotImplementedException();
        }
    }
}

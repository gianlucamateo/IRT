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

        public Cuboid(Vector3 center, float widthX, float heightY, float depthZ, int zIndex = 0)
            : base(center, zIndex)
        {
            this.heightY = heightY;
            this.widthX = widthX;
            this.depthZ = depthZ;
        }

        public override bool IsInside(Vector3 r)
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

        public override Vector3 Dimensions
        {
            get { return new Vector3(this.widthX, this.heightY, this.depthZ); }
        }

        public override Vector3 GetNormal(Vector3 r)
        {
            Vector3 pos = r - Center;
			pos /= this.Dimensions;

            Vector3[] unitV = { Vector3.UnitX, -Vector3.UnitX, Vector3.UnitY, -Vector3.UnitY, Vector3.UnitZ, -Vector3.UnitZ };

            int index = 0;

            if (Math.Max(Math.Max(Math.Abs(pos.X), Math.Abs(pos.Y)), Math.Abs((pos.Z))) == Math.Abs(pos.X))
            {
                index = pos.X >= 0 ? 0 : 1;
            }
            else if (Math.Max(Math.Max(Math.Abs(pos.X), Math.Abs(pos.Y)), Math.Abs((pos.Z))) == Math.Abs(pos.Y))
            {
                index = pos.Y >= 0 ? 2 : 3;
            }
            else if (Math.Max(Math.Max(Math.Abs(pos.X), Math.Abs(pos.Y)), Math.Abs((pos.Z))) == Math.Abs(pos.Z))
            {
                index = pos.Z >= 0 ? 4 : 5;
            }

            return unitV[index];
        }
    }
}

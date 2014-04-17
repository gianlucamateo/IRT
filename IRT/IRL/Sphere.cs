using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
	public class Sphere : Shape, IShape
	{
		public float Radius { get; set; }

		public Sphere(Vector3 center, float radius, int zIndex = 0, float attenuation = 1f) : base(center, zIndex, attenuation) 
		{
			this.Radius = radius;
		}

		public override bool IsInside(Vector3 r)
		{
			Vector3 diff = r - Center;
			return diff.Length() <= Radius;
		}


		public override Vector3 Dimensions
		{
			get { return Vector3.One * this.Radius * 2; }
		}

		public override Vector3 GetNormal(Vector3 r)
		{
			Vector3 normal = (r - Center);
			normal.Normalize();
			return normal;
		}
	}
}

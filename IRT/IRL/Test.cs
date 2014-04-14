using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
	class Test
	{
		public static void Main()
		{
			Space space = new Space(1f);

			Shape cuboid = new Cuboid(Vector3.Zero, 10f, 10f, 10f, 0);
			cuboid.Inhomogeniety = new Inhomogeneity ((x, y, z) => 1 * y + 1f,lambda => -0.013f / 400f * lambda + 1.353f, Vector3.Zero);
			
			space.AddShape(cuboid);
			space.SpawnRay(Vector3.Zero, Vector3.UnitX, 533f,1);

			for (int i = 0; i < 1000; i++)
			{
				space.Update();
			}

			Console.Read();
		}
	}
}

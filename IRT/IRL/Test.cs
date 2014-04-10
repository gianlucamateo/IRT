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
            cuboid.Inhomogeniety = new Inhomogeneity((x, y, z) => y + 3.5f, Vector3.Zero);
            space.addShape(cuboid);

            Ray ray = new Ray(Vector3.Zero, Vector3.UnitX, space);

            for (int i = 0; i < 5000; i++)
            {
                ray.propagate();
            }

            Console.Read();
        }
    }
}

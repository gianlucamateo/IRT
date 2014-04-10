using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    public class Space
    {
        public const float RAY_RESOLUTION = 0.001f, COMPUTE_RESOLUTION = 0.0001f;
        public float refractionIndex;
        private List<Shape> shapes;

        public Space(float refractionIndex)
        {
            this.shapes = new List<Shape>();
            this.refractionIndex = refractionIndex;
        }

        public Shape getMedium(Vector3 r)
        {
            foreach(Shape s in this.shapes){
                if (s.isInside(r))
                {
                    return s;
                }
            }
            return null;
        }

        public void addShape(Shape shape)
        {
            this.shapes.Add(shape);

        }
    }


}

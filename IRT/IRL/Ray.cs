using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    class Ray
    {
        private List<Vector3> segments;
        private Vector3 currentPosition;
        private Vector3 direction;
        private Shape currentMedium;
        private Space space;

        public Ray(Vector3 startPosition, Vector3 direction, Space space){
            this.segments = new List<Vector3>();
            this.currentPosition = startPosition;
            this.direction = direction;
            this.space = space;
            this.currentMedium = space.getMedium(currentPosition);
        }
    }
}

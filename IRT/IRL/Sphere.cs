﻿using System;
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

        public float getRefractionIndex(Vector3 r)
        {
            throw new NotImplementedException();
        }

        public void interact(Vector3 r, Vector3 incident, out Vector3 reflected, out Vector3 refracted, float outerRefractionIndex, float wavelength)
        {
            Vector3 normal = r - Center;
            incident.Normalize();
            normal.Normalize();

            Vector3.Reflect(ref incident, ref normal, out reflected);

            bool flip = Vector3.Dot(incident, normal) > 0;
            if (flip)
            {
                normal *= -1;
            }

            float thetaIn = (float)Math.Acos(Vector3.Dot(incident, normal));
            float thetaOut = (float)Math.Asin((outerRefractionIndex / getRefractionIndex(r, wavelength)) * Math.Sin(thetaIn));

            refracted = incident;
            if (Math.Abs(thetaIn - (float)Math.PI / 2) > 0.01)
            {
                Vector3 axis = Vector3.Cross(incident, normal);
                Matrix rot = Matrix.CreateFromAxisAngle(axis, thetaOut);
                normal *= -1;
                refracted = Vector3.Transform(normal, rot);
            }
        }
    }
}
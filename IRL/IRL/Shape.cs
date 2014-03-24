﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRL
{
    private enum InhomogeneityType
    {
        RADIAL, XYZ
    };
    private struct Inhomogeneity
    {
        private Func<float, float, float, float> XYZInhomogeneity;
        private Func<float, float> RadialInhomogeneity;
        private Vector3 Origin= new Vector3(0,0,0);
        private InhomogeneityType Type = InhomogeneityType.XYZ;

        public Inhomogeneity(Func<float, float, float, float> XYZ, Vector3 origin)
        {
            this.XYZInhomogeneity = XYZ;
            this.RadialInhomogeneity = null;
            this.Type = InhomogeneityType.XYZ;
            this.Origin = origin;
        }

        public Inhomogeneity(Func<float, float> radial, Vector3 origin)
        {
            this.XYZInhomogeneity = null;
            this.RadialInhomogeneity = radial;
            this.Type = InhomogeneityType.RADIAL;
            this.Origin = origin;
        }
        public float Evaluate(Vector3 r){
            if (this.Type == InhomogeneityType.XYZ)
            {
                r -= Origin;
                return XYZInhomogeneity(r.X, r.Y, r.Z);
            }
            else
            {
                Vector3 relativePosition = r - Origin;
                float rDist = relativePosition.Length();
                return RadialInhomogeneity(rDist);
            }
        }
    }
    public abstract class Shape
    {
        public readonly Inhomogeneity inhomogeneity;

    }
}
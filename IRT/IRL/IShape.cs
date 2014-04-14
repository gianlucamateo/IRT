using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    public interface IShape
    {
        Vector3 Position { get; }
        Vector3 Dimensions { get; }

        bool IsInside(Vector3 r);

        Vector3 GetGradient(Vector3 r, float wavelength, float step=Space.COMPUTE_RESOLUTION);

        float GetRefractionIndex(Vector3 r, float wavelength);

        void Interact(Vector3 r, Vector3 incident, out Vector3 reflected, out Vector3 refracted, out float reflectance , float outerRefractionIndex, float wavelength);

        Vector3 GetNormal(Vector3 r);
    }
}

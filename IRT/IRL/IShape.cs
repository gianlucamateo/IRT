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

        bool isInside(Vector3 r);

        Vector3 getGradient(Vector3 r, float wavelength, float step=Space.COMPUTE_RESOLUTION);

        float getRefractionIndex(Vector3 r, float wavelength);

        void interact(Vector3 r, Vector3 incident, out Vector3 reflected, out Vector3 refracted, out float reflectance , float outerRefractionIndex, float wavelength);

        Vector3 getNormal(Vector3 r);
    }
}

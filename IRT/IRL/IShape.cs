using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    interface IShape
    {
        bool isInside(Vector3 r);

        Vector3 getGradient(Vector3 r, float wavelength, float step=Space.COMPUTE_RESOLUTION);

        float getRefractionIndex(Vector3 r, float wavelength);

    }
}

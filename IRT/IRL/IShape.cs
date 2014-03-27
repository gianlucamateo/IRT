using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRL.Engine
{
    interface IShape
    {        
        bool isInside(Vector3 r);

        Vector3 getGradient(Vector3 r);

        float getRefractionIndex(Vector3 r);
    }
}

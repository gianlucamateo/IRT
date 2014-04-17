using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace IRT.Viewer
{
    interface IScene
    {
        void Load(List<IDrawable> rays, List<IDrawable> shapes);
        void Run();
    }
}

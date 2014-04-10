using System;

namespace IRT.Viewer
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (IRTViewer game = new IRTViewer())
            {
                game.Run();
            }
        }
    }
#endif
}


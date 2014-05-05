using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRT.Viewer
{
	interface IDrawable
	{
		void Draw(int timestamp);
		int getZ();
		float Transparency { get; set; }
	}
}

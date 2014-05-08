using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
	public enum InhomogeneityType
	{
		RADIAL, XYZ
	};

	public struct Inhomogeneity
	{
		private Func<float, float, float, float> XYZInhomogeneity;
		private Func<float, float> RadialInhomogeneity;
		private Vector3 Origin;
		private InhomogeneityType Type;
		public Func<float, float> Slope;

		/// <summary>
		/// Radial inhomogeneity: n(r) = ...
		/// </summary>
		public Inhomogeneity(Func<float, float, float, float> XYZ, Func<float, float> slope, Vector3 origin)
		{
			this.XYZInhomogeneity = XYZ;
			this.RadialInhomogeneity = null;
			this.Type = InhomogeneityType.XYZ;
			this.Origin = origin;
			this.Slope = slope;
		}

		/// <summary>
		/// XYZ inhomogeneity: n(x,y,z) = ...
		/// </summary>
		public Inhomogeneity(Func<float, float> radial, Func<float, float> slope, Vector3 origin)
		{
			this.XYZInhomogeneity = null;
			this.RadialInhomogeneity = radial;
			this.Type = InhomogeneityType.RADIAL;
			this.Origin = origin;
			this.Slope = slope;
		}

		/// <summary>
		/// Evaluate inhomogeneity at specific position for specified wavelength
		/// Yields refraction index at that location
		/// </summary>
		public float Evaluate(Vector3 r, float wavelength)
		{
			if (this.Type == InhomogeneityType.XYZ)
			{
				r -= Origin;
				return XYZInhomogeneity(r.X, r.Y, r.Z) * Slope(wavelength); // Slope is dispersion parameter: n(lamba) = ...
			}
			else
			{
				Vector3 relativePosition = r - Origin;
				float rDist = relativePosition.Length();
				return RadialInhomogeneity(rDist) * Slope(wavelength);
			}
		}
	}
}

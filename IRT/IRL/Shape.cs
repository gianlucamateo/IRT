using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
	public abstract class Shape : IShape
	{
		public Inhomogeneity Inhomogeniety;
		protected Vector3 Center;
		public int zIndex;

		public Shape(Vector3 center, int zIndex = 0)
		{
			this.Center = center;
			this.zIndex = zIndex;
		}

		public Vector3 GetGradient(Vector3 r, float wavelength, float step = Space.COMPUTE_RESOLUTION)
		{
			float dx = 0, dy = 0, dz = 0;

			Vector3 xDiff = new Vector3(step, 0, 0);
			Vector3 yDiff = new Vector3(0, step, 0);
			Vector3 zDiff = new Vector3(0, 0, step);

			// Calculate differentials
			dx = (GetRefractionIndex(r + xDiff, wavelength) - GetRefractionIndex(r - xDiff, wavelength)) / (2 * step);
			dy = (GetRefractionIndex(r + yDiff, wavelength) - GetRefractionIndex(r - yDiff, wavelength)) / (2 * step);
			dz = (GetRefractionIndex(r + zDiff, wavelength) - GetRefractionIndex(r - zDiff, wavelength)) / (2 * step);

			Vector3 gradient = new Vector3(dx, dy, dz);
			return gradient;
		}

		public float GetRefractionIndex(Vector3 r, float wavelength)
		{
			return this.Inhomogeniety.Evaluate(r - this.Center, wavelength);
		}

		public abstract bool IsInside(Vector3 r);

		public void Interact(Vector3 r, Vector3 incident, out Vector3 reflected, out Vector3 refracted, out float refl, float outerRefractionIndex, float wavelength)
		{
			Vector3 normal = GetNormal(r);
			incident.Normalize();
			normal.Normalize();

			// Flip normal if incident ray direction is coming from the inside
			bool fromInside = Vector3.Dot(normal, incident) > 0;

			// Compute incoming and outgoing angles according to Snell's Law
			Vector3 tempIncident = incident;
			float nIn = GetRefractionIndex(r, wavelength);

			if (!fromInside)
			{
				normal *= -1;
				float thetaIn = (float)Math.Acos(Vector3.Dot(tempIncident, normal));
				float thetaOut = (float)Math.Asin((outerRefractionIndex / nIn) * Math.Sin(thetaIn));

				refracted = incident;

				// Rotate reversed normal by outgoing angle
				Vector3 axis = Vector3.Cross(normal, incident);

				axis.Normalize();

				Matrix rot = Matrix.CreateFromAxisAngle(axis, thetaOut);
				refracted = Vector3.Transform(normal, rot);

				refl = reflectance(thetaIn, nIn, outerRefractionIndex);
				
				// Compute and return reflected vector
				normal *= -1;
				Vector3.Reflect(ref incident, ref normal, out reflected);
			}
			else
			{
				// Swap n1 <-> n2
				float temp = nIn;
				nIn = outerRefractionIndex;
				outerRefractionIndex = temp;

				float thetaIn = (float)Math.Acos(Vector3.Dot(tempIncident, normal));
				float thetaOut = (float)Math.Asin((outerRefractionIndex / nIn) * Math.Sin(thetaIn));

				Vector3 axis = Vector3.Cross(normal, incident);

				axis.Normalize();

				Matrix rot = Matrix.CreateFromAxisAngle(axis, thetaOut);
				refl = reflectance(thetaIn, nIn, outerRefractionIndex);
				refracted = Vector3.Transform(normal, rot);
				
				// Compute and return reflected vector
				normal *= -1;
				Vector3.Reflect(ref incident, ref normal, out reflected);
			}
		}

		private float reflectance(float thetaIn, float n1, float n2)
		{
			float theta = thetaIn;
			float R0 = (n1 - n2) / (n1 + n2);
			R0 *= R0;
			float R = R0 + (1f - R0) * (1f - (float)Math.Pow(Math.Cos(theta), 5));
			return R;
		}

		public Vector3 Position
		{
			get { return this.Center; }
		}

		public abstract Vector3 Dimensions { get; }
		public abstract Vector3 GetNormal(Vector3 r);
	}
}

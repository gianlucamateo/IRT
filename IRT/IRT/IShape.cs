using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
	public interface IShape
	{
		/// <summary>
		/// The shape's center position
		/// </summary>
		Vector3 Position { get; }
		/// <summary>
		/// The shape's dimension
		/// </summary>
		Vector3 Dimensions { get; }

		/// <summary>
		/// Returns true if specified position is inside the current shape
		/// </summary>
		bool IsInside(Vector3 r);

		/// <summary>
		/// Get Gradient at specified position for a given wavelength
		/// </summary>
		Vector3 GetGradient(Vector3 r, float wavelength, float step = Space.COMPUTE_RESOLUTION);

		/// <summary>
		/// Get the refractive index at specified position for a given wavelength
		/// </summary>
		float GetRefractionIndex(Vector3 r, float wavelength);

		/// <summary>
		/// Solve medium change
		/// </summary>
		void Interact(Vector3 r, Vector3 incident, out Vector3 reflected, out Vector3 refracted, out float reflectance, float outerRefractionIndex, float wavelength, out bool spawnRefl, out bool spawnRefr);

		/// <summary>
		/// Get attenuation per step of current medium
		/// </summary>
		float getAttenuation();

		/// <summary>
		/// Get normal at specified position
		/// </summary>
		Vector3 GetNormal(Vector3 r);
	}
}

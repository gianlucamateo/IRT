using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
	public class Ray
	{
		public List<Vector3> segments;
		private float wavelength;
		private Vector3 position;
		private Vector3 direction;
		private Shape medium;
		private Space space;
		private bool dead;

		public float Intensity { get; set; }
		public float Wavelength { get { return this.wavelength; } }

		public Ray(Vector3 startPosition, Vector3 direction, Space space, float wavelength, float intensity = 1f)
		{
			this.dead = false;
			this.segments = new List<Vector3>();
			this.position = startPosition;
			this.direction = direction;
			this.direction.Normalize();
			this.space = space;
			this.medium = space.GetMedium(position);
			this.wavelength = wavelength;
			this.Intensity = intensity;
		}

		public void Propagate()
		{
			if (dead)
			{
				return;
			}

			Vector3 predictedPosition = position + direction * Space.RAY_RESOLUTION;
			Shape nextMedium = space.GetMedium(predictedPosition);

			// Check for medium change
			if (nextMedium != medium)
			{
				float reflectance;
				Vector3 reflected, refracted;
				if (nextMedium == null)
				{
					medium.Interact(position, direction, out reflected, out refracted, out reflectance, space.refractionIndex, wavelength);
				}
				else
				{
					float previousRefrac = medium == null ? space.refractionIndex : medium.GetRefractionIndex(position,wavelength);
					nextMedium.Interact(position, direction, out reflected, out refracted, out reflectance, previousRefrac, wavelength);
				}
				//TODO: maybe swap this
				this.space.SpawnRay(position, reflected, wavelength, this.Intensity*(reflectance));
				this.space.SpawnRay(predictedPosition, refracted, wavelength, this.Intensity*(1-reflectance));
				this.dead = true;

				return;
			}
			else
			{
				if (medium == null)
				{
					this.position = predictedPosition;
					this.segments.Add(position);

					return;
				}

				Vector3 rayDir = direction, dirStep = Vector3.Zero;
				Vector3 sum = Vector3.Zero, doubleSum = Vector3.Zero;

				for (float dl = 0f; dl < Space.RAY_RESOLUTION; dl += Space.COMPUTE_RESOLUTION)
				{
					dirStep = dl * rayDir;
					Vector3 gradient = medium.GetGradient(position + dirStep, wavelength);

					Vector3 dr = gradient * Space.COMPUTE_RESOLUTION * medium.GetRefractionIndex(position + dirStep, wavelength);
					sum += dr;
					doubleSum += sum * Space.COMPUTE_RESOLUTION;
				}

				rayDir += doubleSum;
				rayDir.Normalize();

				this.position += rayDir * Space.RAY_RESOLUTION;
				this.direction = rayDir;
			}

			this.segments.Add(position);
		}
	}
}

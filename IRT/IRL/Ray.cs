using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
	public struct RaySegment
	{
		public int timeStamp;
		public float intensity;
		public Vector3 position;

		public RaySegment(int timeStamp, Vector3 position, float intensity = 1)
		{
			this.timeStamp = timeStamp;
			this.position = position;
			this.intensity = intensity;
		}
	}
	public class Ray
	{
		public List<RaySegment> segments;
		private float wavelength;
		private Vector3 position;
		private Vector3 direction;
		private Shape medium;
		private Space space;
		private bool dead;
		private int timeStamp;
		private float minIntensity;

		public float Intensity { get; set; }
		public float Wavelength { get { return this.wavelength; } }

		public Ray(Vector3 startPosition, Vector3 direction, Space space, float wavelength, float intensity = 1f, int timeStamp = 0, float minIntensity = Space.DEFAULT_MIN_INTENSITY)
		{
			this.timeStamp = timeStamp;
			this.dead = false;
			this.segments = new List<RaySegment>();
			this.position = startPosition;
			this.direction = direction;
			this.direction.Normalize();
			this.space = space;
			this.medium = space.GetMedium(position);
			this.wavelength = wavelength;
			this.minIntensity = minIntensity;
			this.setIntensity(intensity);
		}

		public void setIntensity(float intensity)
		{
			if (intensity > this.minIntensity)
			{
				this.Intensity = intensity;
			}
			else
			{
				this.Intensity = this.minIntensity;
			}
		}

		public void Propagate()
		{
			if (dead)
			{
				return;
			}

			Vector3 predictedPosition = position + direction * Space.RAY_RESOLUTION;
			Shape nextMedium = space.GetMedium(predictedPosition);

			bool spawnRefl, spawnRefr;

			// Check for medium change
			if (nextMedium != medium)
			{
				float reflectance;
				Vector3 reflected, refracted;
				if (nextMedium == null)
				{
					medium.Interact(position, direction, out reflected, out refracted, out reflectance, space.refractionIndex, wavelength, out spawnRefl, out spawnRefr);
				}
				else
				{
					float previousRefrac = medium == null ? space.refractionIndex : medium.GetRefractionIndex(position, wavelength);
					nextMedium.Interact(position, direction, out reflected, out refracted, out reflectance, previousRefrac, wavelength, out spawnRefl, out spawnRefr);
				}

				if (!spawnRefl)
				{
					reflectance = 0;
				}
				if (!spawnRefr)
				{
					reflectance = 1;
				}

				if (spawnRefl)
				{
					Console.WriteLine("spawning refl");
					this.space.SpawnRay(position, reflected, wavelength, this.Intensity * (reflectance), this.timeStamp + 1, this.minIntensity);
				}
				if (spawnRefr)
				{
					Console.WriteLine("spawning refr");
					this.space.SpawnRay(predictedPosition, refracted, wavelength, this.Intensity * (1f - reflectance), this.timeStamp + 1, this.minIntensity);
				}

				// Kill ray after spawning children
				this.dead = true;

				return;
			}
			else
			{
				if (medium == null)
				{
					this.position = predictedPosition;
					this.segments.Add(new RaySegment(this.timeStamp, position, this.Intensity));
					timeStamp++;

					return;
				}

				Vector3 rayDir = direction, dirStep = Vector3.Zero;
				Vector3 sum = Vector3.Zero;//, doubleSum = Vector3.Zero;

				for (float dl = 0f; dl < Space.RAY_RESOLUTION; dl += Space.COMPUTE_RESOLUTION)
				{
					dirStep = dl * rayDir;
					Vector3 gradient = medium.GetGradient(position + dirStep, wavelength);
					//Math.Abs to catch negative refractive indices
					Vector3 dr = gradient * Space.COMPUTE_RESOLUTION * 
						Math.Abs(medium.GetRefractionIndex(position + dirStep, wavelength));
					sum += dr;
					
				}
				this.setIntensity(this.Intensity * medium.getAttenuation());
				sum *= Space.RAY_RESOLUTION;
				rayDir += sum;
				rayDir.Normalize();

				this.position += rayDir * Space.RAY_RESOLUTION;
				this.direction = rayDir;
			}

			this.segments.Add(new RaySegment(this.timeStamp, position, this.Intensity));
			timeStamp++;
		}
	}
}

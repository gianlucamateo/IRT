﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace IRT.Engine
{
	public class Space
	{
		public const float RAY_RESOLUTION = 0.001f, COMPUTE_RESOLUTION = 0.00001f, DEFAULT_MIN_INTENSITY = 0.1f;
		public float refractionIndex;
		private List<Shape> shapes;
		public List<Ray> rays;
		public List<Ray> newlySpawned;
		public List<Ray> finishedRays;

		public Space(float refractionIndex)
		{
			this.shapes = new List<Shape>();
			this.rays = new List<Ray>();
			this.finishedRays = new List<Ray>();
			this.newlySpawned = new List<Ray>();
			this.refractionIndex = refractionIndex;
		}

		/// <summary>
		/// Spawn Ray with parameters
		/// </summary>
		public void SpawnRay(Vector3 position, Vector3 direction, float wavelength, float intensity = 1f, int timestamp = 0, float minIntensity = Space.DEFAULT_MIN_INTENSITY)
		{
			if (intensity > 0)
			{
				this.newlySpawned.Add(new Ray(position, direction, this, wavelength, intensity, timestamp, minIntensity: minIntensity));
			}
		}

		/// <summary>
		/// Spawn a number of rays with indicated wavelengths
		/// </summary>
		public void SpawnCluster(Vector3 position, Vector3 direction, float begin, float end, int numRays, float intensity = 1f, float minInt = Space.DEFAULT_MIN_INTENSITY, int timestamp = 0)
		{
			for (int i = 0; i < numRays; i++)
			{
				float wl = MathHelper.Lerp(begin, end, i / (float)numRays);
				SpawnRay(position, direction, wl, intensity, timestamp, minIntensity: minInt);
			}
		}

		/// <summary>
		/// Start the simulation
		/// </summary>
		public void Update(int maxSpawns = 4, int count = 1)
		{
			int iterations = 0;

			// Loop while ray is not dead and still able to spawn child rays
			do
			{
				rays = newlySpawned;
				newlySpawned = new List<Ray>();

				// Propagate each ray in parallel for faster performance
				Parallel.ForEach(rays, ray => ray.Propagate(count));
				// Add finished rays to collection
				rays.ForEach(ray => finishedRays.Add(ray));
				iterations++;

			} while (this.newlySpawned.Count > 0 && iterations < maxSpawns);
		}

		// Get the medium at a given position within space
		public Shape GetMedium(Vector3 r)
		{
			int zIndex = -1;
			Shape currentShape = null;
			foreach (Shape s in this.shapes)
			{
				if (s.IsInside(r))
				{
					if (zIndex < s.zIndex)
					{
						zIndex = s.zIndex;
						currentShape = s;
					}
				}
			}
			if (zIndex == -1)
				return null;
			else
				return currentShape;
		}

		public void AddShape(Shape shape)
		{
			this.shapes.Add(shape);
		}
	}
}

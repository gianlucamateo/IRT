﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IRT.Engine
{
    class Ray
    {
        private List<Vector3> segments;
        private float wavelength;
        private Vector3 position;
        private Vector3 direction;
        private Shape medium;
        private Space space;

        public Ray(Vector3 startPosition, Vector3 direction, Space space){
            this.segments = new List<Vector3>();
            this.position = startPosition;
            this.direction = direction;
            this.space = space;
            this.medium = space.getMedium(position);
        }

        public void propagate()
        {
            Vector3 predictedPosition = position + direction * Space.RAY_RESOLUTION;

            Shape nextMedium = space.getMedium(predictedPosition);
            
            // Check for medium change
            if (nextMedium != medium)
            {
                Console.WriteLine("Encountered medium change, refracting and reflecting...");
                return;
            }

            Vector3 rayDir = direction, dirStep = Vector3.Zero;
            Vector3 sum = Vector3.Zero, doubleSum = Vector3.Zero;

            for (float dl = 0f; dl < Space.RAY_RESOLUTION; dl += Space.COMPUTE_RESOLUTION)
            {
                dirStep = dl * rayDir;
                Vector3 gradient = medium.getGradient(position + dirStep, wavelength);
                Vector3 dr = gradient * Space.COMPUTE_RESOLUTION * medium.getRefractionIndex(position + dirStep, wavelength);
                sum += dr;
                doubleSum += sum * Space.COMPUTE_RESOLUTION;                
            }

            rayDir += doubleSum;
            rayDir.Normalize();

            this.position += rayDir * Space.RAY_RESOLUTION;
            this.direction = rayDir;

            this.segments.Add(position);
            Console.WriteLine(position);
        }
    }
}
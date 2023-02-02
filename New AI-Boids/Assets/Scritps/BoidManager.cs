using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    //const int threadGroupSize = 1024;

    private int goldenNumber = 0;
    public float viewRadius;

    public BoidSettings settings;

    // public ComputeShader compute;
    Boid[] boids;

    void Start()
    {
        boids = FindObjectsOfType<Boid>();
        foreach (Boid b in boids)
        {
            b.Initialize(settings, null);
        }
    }

    void Update()
    {
        if (boids != null)
        {
            int numBoids = boids.Length;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Length; i++)
            {
                boidData[i].position = boids[i].position;
                boidData[i].direction = boids[i].forward;
            }

            for (int i = 0; i < numBoids; i++)
            {
                if (goldenNumber != i)
                {
                    Boid boidB = boids[i];
                    Vector3 offset = boidB.position - boids[goldenNumber].position;
                    float sqrDst = offset.x * offset.x + offset.y * offset.y + offset.z * offset.z;

                    if (sqrDst < viewRadius * viewRadius)
                    {
                        boids[goldenNumber].numPerceivedFlockmates += 1;
                        boids[goldenNumber].avgFlockHeading += boidB.cachedTransform.position;
                        boids[goldenNumber].centreOfFlockmates += boidB.position;

                        if (sqrDst < settings.avoidanceRadius * settings.avoidanceRadius)
                        {
                            boids[goldenNumber].seperationForce -= offset / sqrDst;
                        }
                    }
                }
            }

            for (int i = 0; i < boids.Length; i++)
            {
                boids[i].avgFlockHeading = boidData[i].flockHeading;
                boids[i].centreOfFlockmates = boidData[i].flockCentre;
                boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
                boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

                boids[i].UpdateBoid();
            }
        }
    }

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size
        {
            get { return sizeof(float) * 3 * 5 + sizeof(int); }
        }
    }
}
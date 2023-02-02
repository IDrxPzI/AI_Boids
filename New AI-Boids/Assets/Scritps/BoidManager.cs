using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private BoidSettings settings;
    [SerializeField] private float viewRadius;

    private Boid[] boids;
    private int goldenNumber = 0;


    void Start()
    {
        boids = FindObjectsOfType<Boid>();
        foreach (Boid boid in boids)
        {
            boid.Initialize(settings);
        }
    }

    //update direction/position of the boids
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
                    Vector3 offset = boidB.position - boids[i].position;
                    float sqrDst = offset.x * offset.x + offset.y * offset.y + offset.z * offset.z;

                    if (sqrDst < viewRadius * viewRadius)
                    {
                        boids[i].numNeighbours += 1;
                        boids[i].avgNeighbourDirection += boidData[i].direction;
                        boids[i].centerOfNeighbours += boidData[i].position;

                        if (sqrDst < settings.avoidanceRadius * settings.avoidanceRadius)
                        {
                            boids[i].avgAvoidanceDirection -= offset / sqrDst;
                        }
                    }
                }
            }

            for (int i = 0; i < boids.Length; i++)
            {
                boids[i].avgNeighbourDirection = boidData[i].neighbourDirection;
                boids[i].centerOfNeighbours = boidData[i].NeighbourCenter;
                boids[i].avgAvoidanceDirection = boidData[i].avoidanceDirection;
                boids[i].numNeighbours = boidData[i].numNeighbours;

                boids[i].UpdateBoid();
            }
        }
    }

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 neighbourDirection;
        public Vector3 NeighbourCenter;
        public Vector3 avoidanceDirection;
        public int numNeighbours;
    }
}
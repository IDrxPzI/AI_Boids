using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{
    [Header("Boid Controls")]
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float avoidanceRadius = 1;
    public float maxSteerForce = 3;
    public float Alignment = 1;
    public float Cohesion = 1;
    public float Separation = 1;

    [Header("Collisions")] public LayerMask obstacleMask;
    public float RayCastRadius = .27f;
    public float AvoidCollision = 10;
    public float AvoidCollisionDistance = 5;
}
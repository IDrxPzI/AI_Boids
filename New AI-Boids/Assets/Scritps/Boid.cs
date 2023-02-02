using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boid : MonoBehaviour
{
    BoidSettings settings;

    Vector3 velocity;
    Vector3 acceleration;
    
    private Transform localTransform;
    
    private Vector3 alignmentForce;
    private Vector3 cohesionForce;
    private Vector3 seperationForce;
    private Vector3 direction;
    
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 forward;
    [HideInInspector] public Vector3 avgNeighbourDirection;
    [HideInInspector] public Vector3 avgAvoidanceDirection;
    [HideInInspector] public Vector3 centerOfNeighbours;
    [HideInInspector] public int numNeighbours;

    void Awake()
    {
        localTransform = transform;
    }

    public void Initialize(BoidSettings _settings)
    {
        this.settings = _settings;

        position = localTransform.position;
        forward = localTransform.forward;

        float startSpeed = (_settings.minSpeed + _settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }
    
    /// <summary>
    /// update direction,position,speed of the Boid
    /// </summary>
    public void UpdateBoid()
    {
        Vector3 acceleration = Vector3.zero;

        if (numNeighbours != 0)
        {
            centerOfNeighbours /= numNeighbours;

            Vector3 offsetToNeighboursCenter = (centerOfNeighbours - position);


            alignmentForce = SteerTowards(avgNeighbourDirection) * settings.Alignment;
            cohesionForce = SteerTowards(offsetToNeighboursCenter) * settings.Cohesion;
            seperationForce = SteerTowards(avgAvoidanceDirection) * settings.Separation;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDiraction = ObstacleRays();
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDiraction) * settings.AvoidCollision;
            acceleration += collisionAvoidForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        direction = velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = direction * speed;

        localTransform.position += velocity * Time.deltaTime;
        localTransform.forward = direction;
        position = localTransform.position;
        forward = direction;
    }

    //Raycast forward to check for collision
    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(position, settings.RayCastRadius, forward, out hit, settings.AvoidCollisionDistance,
                settings.obstacleMask))
        {
            return true;
        }

        return false;
    }
    
//Raycast in every direction
    Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = localTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, settings.RayCastRadius, settings.AvoidCollisionDistance,
                    settings.obstacleMask))
            {
                return dir;
            }
        }

        return forward;
    }
    
    
    public Vector3 SteerTowards(Vector3 _vector)
    {
        Vector3 v = _vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }
}
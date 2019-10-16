using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBoid 
{
    void UpdatePosition();
    void UpdateVectors(List<IBoid> boids, BoidsBehaviourPreset preset);
    bool IsNearby(IBoid boid);
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractBoid : IBoid
{
    protected float speed;
    protected BoidConfiguration config;

    public BoidConfiguration Config
    {
        get
        {
            return config;
        }

        set
        {
            config = value;
        }
    }

    public virtual void UpdatePosition()
    {
        Axelerate();
    }

    protected virtual void Axelerate()
    {
        speed = Mathf.Min(speed + config.axeleration, config.maxSpeed);
    }

    public virtual AbstractBoid Construct(BoidConfiguration config)
    {
        Config = config;
        return this;
    }   

    public abstract bool IsNearby(IBoid boid);
    public abstract void UpdateVectors(List<IBoid> boids, BoidsBehaviourPreset preset);
}

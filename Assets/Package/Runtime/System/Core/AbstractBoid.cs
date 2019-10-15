using UnityEngine;
using System.Collections;

public abstract class AbstractBoid : IBoid
{
    protected float speed;
    protected BoidConfiguration config;

    public virtual void Update()
    {
        Axelerate();
    }

    protected void Axelerate()
    {
        speed = Mathf.Min(speed + config.axeleration, config.maxSpeed);
    }

    public virtual AbstractBoid Construct(BoidConfiguration config)
    {
        this.config = config;
        return this;
    }

    protected abstract void UpdatePosition();

    public abstract bool IsNearby(IBoid boid);
}

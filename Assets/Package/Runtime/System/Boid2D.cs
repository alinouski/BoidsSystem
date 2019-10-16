using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid2D : AbstractBoid
{
    protected Vector2 currentPosition;
    protected Vector2 currentDirection = Vector2.up;
    protected Rect cage;
    protected bool useSpace = false;
    protected bool unlimitSpace = false;

    protected event Action<Vector2> updatePosition;


    public Vector2 Position
    {
        get
        {
            return currentPosition;
        }

        set
        {
            currentPosition = value;
            if (updatePosition != null)
                updatePosition(currentPosition);
        }
    }

    public Vector2 Velocity
    {
        get
        {
            return currentDirection * speed;
        }
    }

    public Boid2D(Vector2 position, Vector2 direction)
    {
        currentPosition = position;
        currentDirection = direction.normalized;        
    }

    public Boid2D(Vector2 position, Vector2 direction, BoidConfiguration config) : this(position, direction)
    {
        base.Construct(config);
    }

    public Boid2D(Vector2 position, Vector2 direction, BoidConfiguration config, Rect simulateArea, bool unlimSpace) : this(position, direction, config)
    {
        Construct(config, simulateArea, unlimSpace);
    }

    public AbstractBoid Construct(BoidConfiguration config, Rect simulateArea, bool unlimSpace)
    {
        cage = simulateArea;
        useSpace = true;
        unlimitSpace = unlimSpace;
        return base.Construct(config);
    }

    public override void UpdateVectors(List<IBoid> boids, BoidsBehaviourPreset preset)
    {
        Vector2 aligment = Vector2.zero;
        Vector2 cohesion = Vector2.zero;
        Vector2 separation = Vector2.zero;

        Vector2 area = Vector2.zero;

        int countOfNearby = 0;
        for (int i = boids.Count - 1; i >= 0; i--)
        {
            Boid2D boid = (Boid2D)boids[i];
            if (boid != this && IsNearby(boid))
            {
                aligment += boid.Velocity;
                cohesion += boid.Position;

                float distance = Vector2.Distance(Position, boid.Position);
                separation += (Position - boid.Position) / Mathf.Pow(distance, 2);

                countOfNearby++;
            }
        }

        if (countOfNearby > 0)
        {
            aligment /= (float)countOfNearby;
            cohesion /= (float)countOfNearby;
            separation /= (float)countOfNearby;

            cohesion -= Position;

            aligment *= preset.aligment;
            cohesion *= preset.cohesion;
            separation *= preset.separation;

            Vector2 newForce = (cohesion + aligment + separation);

            currentDirection = (newForce + currentDirection).normalized;
        }
    }

    public override void UpdatePosition()
    {
        base.UpdatePosition();
        Position += Velocity;
        if (useSpace && unlimitSpace)
        {
            UpdatePositionInCage();
        }
    }

    protected void UpdatePositionInCage()
    {
        Vector2 fixVector = Vector2.zero;
        if (Position.x < cage.x)
        {
            fixVector.x = cage.width;
        }
        else if (Position.x > cage.xMax)
        {
            fixVector.x = -cage.width;
        }

        if (Position.y < cage.y)
        {
            fixVector.y = cage.height;
        }
        else if (Position.y > cage.yMax)
        {
            fixVector.y = -cage.height;
        }

        Position += fixVector;
    }

    public override bool IsNearby(IBoid boid)
    {
        return Vector2.Distance(Position, ((Boid2D)boid).Position) <= this.config.watchRadius;
    }

    ///////////////////////////////////////////////

    public void SubscribeOnUpdatePosition(Action<Vector2> updateAction)
    {
        updatePosition += updateAction;
    }

    public void UnsubscribeOnUpdatePosition(Action<Vector2> updateAction)
    {
        updatePosition -= updateAction;
    }    
}

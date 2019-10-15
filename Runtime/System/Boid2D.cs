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

    public AbstractBoid Construct(BoidConfiguration config, Rect simulateArea, bool unlimSpace)
    {
        cage = simulateArea;
        useSpace = true;
        unlimitSpace = unlimSpace;
        return base.Construct(config);
    }

    public override void Update()
    {
        base.Update();
        UpdatePosition();
    }

    public void Update(List<Boid2D> boids, BoidsBehaviourPreset preset)
    {
        Vector2 aligment = Vector2.zero;
        Vector2 cohesion = Vector2.zero;
        Vector2 separation = Vector2.zero;
        int countOfNearby = 0;
        for (int i = boids.Count - 1; i >= 0; i--)
        {
            if (boids[i] != this && IsNearby(boids[i]))
            {
                aligment += boids[i].Velocity;
                cohesion += boids[i].Position;

                float distance = Vector2.Distance(Position, boids[i].Position);
                separation += (Position - boids[i].Position) / distance;

                countOfNearby++;
            }
        }

        if (countOfNearby > 0)
        {
            aligment /= (float)countOfNearby;
            cohesion /= (float)countOfNearby;
            separation /= (float)countOfNearby;

            //resultVelocity = resultVelocity.normalized;
            cohesion -= Position;
            //resultPosition = resultPosition.normalized;

            aligment *= preset.aligment;
            cohesion *= preset.cohesion;
            separation *= preset.separation;

            Vector2 newForce = (cohesion + aligment + separation);

            currentDirection = (newForce + currentDirection).normalized;
            //currentDirection = (resultVelocity.normalized * config.maxConnectForce + currentDirection).normalized;
        }
    }

    protected override void UpdatePosition()
    {
        Position += Velocity;
        if (useSpace)
        {
            UpdatePositionInCage();
        }
    }

    protected void UpdatePositionInCage()
    {
        if (unlimitSpace)
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

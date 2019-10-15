using UnityEngine;
using System.Collections;

public class SpriteBoid : MonoBehaviour
{
    protected Boid2D boid;

    private void OnEnable()
    {
        if(boid != null)
            boid.SubscribeOnUpdatePosition(UpdatePosition);
    }

    private void OnDisable()
    {
        if (boid != null)
            boid.UnsubscribeOnUpdatePosition(UpdatePosition);
    }

    public void InitBoid(Boid2D boid)
    {
        this.boid = boid;
        boid.SubscribeOnUpdatePosition(UpdatePosition);
    }

    protected Vector3 prevPosition;

    protected void UpdatePosition(Vector2 newPosition)
    {
        prevPosition = transform.position;
        transform.position = newPosition;
        transform.rotation = Quaternion.FromToRotation(prevPosition, newPosition);
    }
}

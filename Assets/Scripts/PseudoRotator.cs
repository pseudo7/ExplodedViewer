using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoRotator : MonoBehaviour
{
    public static Transform target;

    public float dampening = 10;

    void Start()
    {
        target = transform;
    }

    void LateUpdate()
    {
        if (Input.touchCount == 1)
        {
            if (ExplosionCamera.isMoving) return;
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition * (1 / dampening);
            if (ExplosionCamera.hasMoved)
                target.Rotate(-touchDelta.y, touchDelta.x, 0, Space.World);
            else target.Rotate(touchDelta.y, -touchDelta.x, 0, Space.World);
        }
    }
}

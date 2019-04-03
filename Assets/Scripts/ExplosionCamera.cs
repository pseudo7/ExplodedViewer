using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCamera : MonoBehaviour
{
    public static ExplosionCamera Instance;

    public static bool hasMoved;

    public static bool isMoving;

    Vector3 origPosition;

    const float MOVE_STEP = .15f;

    private void Awake()
    {
        if (!Instance) Instance = this;
        origPosition = transform.position;
    }

    public void MoveCameraTo(Transform target)
    {
        if (!isMoving) StartCoroutine(MoveCamera(target));
    }

    public void MoveCameraBack()
    {
        if (!isMoving) StartCoroutine(MoveCamera(origPosition));
    }

    IEnumerator MoveCamera(Transform target)
    {
        Vector3 targetPosition = target.position;
        isMoving = true;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, MOVE_STEP);
            yield return new WaitForEndOfFrame();
        }

        hasMoved = targetPosition != origPosition;
        PseudoRotator.target = targetPosition == origPosition ? ExplosionManager.Instance.explosionParent : transform;
        isMoving = false;
    }

    IEnumerator MoveCamera(Vector3 targetPosition)
    {
        transform.rotation = Quaternion.identity;

        isMoving = true;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, MOVE_STEP);
            yield return new WaitForEndOfFrame();
        }
        
        PseudoRotator.target = targetPosition == origPosition ? ExplosionManager.Instance.explosionParent : transform;
        hasMoved = targetPosition != origPosition;

        isMoving = false;
    }
}

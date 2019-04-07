using UnityEngine;

public class ExplosionPiece : MonoBehaviour
{
    public bool referOrigin = true;
    public float distance = 5f;

    public Transform Piece { private set; get; }
    public Vector3 StartPoint { private set; get; }
    public Vector3 EndPoint { private set; get; }

    float lastTapTime = -1;

    const float DOUBLE_TAP_TIME = .3f;

    void Awake()
    {
        Piece = transform;
        StartPoint = transform.position;
        EndPoint = referOrigin ? transform.localPosition.normalized * distance : transform.GetChild(0).position;
    }

    private void OnMouseUpAsButton()
    {
        if (lastTapTime > 0 && Time.timeSinceLevelLoad - lastTapTime < DOUBLE_TAP_TIME)
            ExplosionCamera.Instance.MoveCameraTo(transform);
        lastTapTime = Time.timeSinceLevelLoad;
    }
}

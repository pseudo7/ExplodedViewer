using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager Instance;

    public bool useUnclampedExplosion;
    public Slider explosionSlider;
    public Transform explosionParent;
    public ExplosionPiece[] explosionPieces;
    [Space]
    public bool useNewTransformations;
    public Vector3 positionOffset;
    public Vector3 eulerRotation;
    public Vector3 localScale = Vector3.one;

    bool resetting;

    void Awake()
    {
        Instance = this;

        if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
            explosionSlider.gameObject.SetActive(false);
    }

    void Start()
    {
        if (useNewTransformations)
        {
            explosionParent.position = positionOffset;
            explosionParent.localEulerAngles = eulerRotation;
            explosionParent.localScale = localScale;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (ExplosionCamera.hasMoved)
                ExplosionCamera.Instance.MoveCameraBack();
            else if (!resetting)
                StartCoroutine(ResetTransform());
    }

    IEnumerator ResetTransform()
    {
        resetting = true;
        if (useNewTransformations)
        {
            Quaternion quaternion = Quaternion.Euler(eulerRotation);

            while (Quaternion.Angle(quaternion, explosionParent.rotation) > 3)
            {
                explosionParent.rotation = Quaternion.RotateTowards(explosionParent.rotation, quaternion, 2);
                yield return new WaitForEndOfFrame();
            }
            explosionParent.rotation = quaternion;
        }
        else
        {
            while (explosionParent.eulerAngles.sqrMagnitude > 10)
            {
                explosionParent.rotation = Quaternion.RotateTowards(explosionParent.rotation, Quaternion.identity, 1);
                yield return new WaitForEndOfFrame();
            }
            explosionParent.rotation = Quaternion.identity;
        }
        yield return StartCoroutine(ResetSlider());
        resetting = true;

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    IEnumerator ResetSlider()
    {
        float step = (explosionSlider.maxValue - explosionSlider.minValue) / 100f;

        if (explosionSlider.value > 0)
            while (explosionSlider.value > 0)
            {
                explosionSlider.value -= step;
                yield return new WaitForEndOfFrame();
            }
        else
            while (explosionSlider.value < 0)
            {
                explosionSlider.value += step;
                yield return new WaitForEndOfFrame();
            }
        explosionSlider.value = 0;
    }

    public void MoveExplosion()
    {
        foreach (ExplosionPiece explosionPiece in explosionPieces)
            if (useUnclampedExplosion) explosionPiece.Piece.localPosition = Vector3.LerpUnclamped(explosionPiece.StartPoint, explosionPiece.EndPoint, explosionSlider.value);
            else explosionPiece.Piece.localPosition = Vector3.Lerp(explosionPiece.StartPoint, explosionPiece.EndPoint, explosionSlider.value);
    }
}
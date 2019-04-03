using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager Instance;

    public Slider explosionSlider;
    public Transform explosionParent;
    public ExplosionPiece[] explosionPieces;

    private void Awake()
    {
        if (!Instance) Instance = this;

        if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor)
            explosionSlider.gameObject.SetActive(false);

        foreach (ExplosionPiece explosionPiece in explosionPieces)
            explosionPiece.transform.parent = explosionParent;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (ExplosionCamera.hasMoved)
                ExplosionCamera.Instance.MoveCameraBack();
            else Application.Quit();
    }

    public void MoveExplosion()
    {
        foreach (ExplosionPiece explosionPiece in explosionPieces)
            explosionPiece.Piece.localPosition = Vector3.LerpUnclamped(explosionPiece.StartPoint, explosionPiece.EndPoint, explosionSlider.value);
    }
}
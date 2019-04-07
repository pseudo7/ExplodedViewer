using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatticeGenerator : MonoBehaviour
{
    public GameObject prefab;
    public Material prefabMat;
    public float radius = 1;
    public float scale = 1;

    float width;
    int counter;
    List<ExplosionPiece> explosionPieces = new List<ExplosionPiece>();

    void Start()
    {
        Generate3D();
    }

    void Generate3D()
    {
        width = radius * scale;
        prefab.transform.localScale = Vector3.one * scale;
        prefab.GetComponent<SphereCollider>().radius = scale / 2;

        for (float k = -width; k <= width; k += scale / 2, counter++)
            Generate2D(counter % 2 != 0 ? width : width - scale / 2, k);
    }

    void Generate2D(float width, float offset)
    {
        for (float i = -width; i <= width; i += scale)
            for (float j = -width; j <= width; j += scale)
            {
                GameObject sphere = Instantiate(prefab, new Vector3(i, offset, j), Quaternion.identity, transform);
                sphere.GetComponent<MeshRenderer>().material = new Material(prefabMat) { mainTextureOffset = new Vector2(.25f, Random.value) };
                explosionPieces.Add(sphere.GetComponent<ExplosionPiece>());
            }
        ExplosionManager.Instance.explosionPieces = explosionPieces.ToArray();
    }
}

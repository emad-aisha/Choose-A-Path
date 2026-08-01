using UnityEngine;
using UnityEditor;

public class SpriteLayerManager : MonoBehaviour {

    void Start() {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < sprites.Length; i++) {
            sprites[i].sortingOrder = (int)-sprites[i].transform.position.z * 10;
        }
    }


    [MenuItem("Sorting Layers/Readjust")]
    static void AdjustSortingLayers() {
        SpriteRenderer[] sprites = FindObjectsByType<SpriteRenderer>();

        for (int i = 0; i < sprites.Length; i++) {
            sprites[i].sortingOrder = (int)-sprites[i].transform.position.z * 10;
        }
    }

}

using UnityEngine;

public class SpriteLayerManager : MonoBehaviour {

    void Start() {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < sprites.Length; i++) {
            sprites[i].sortingOrder = (int)(sprites[i].transform.position.z / 100);
        }
    }

}

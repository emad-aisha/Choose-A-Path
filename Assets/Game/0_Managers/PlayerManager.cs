using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    Transform playerTransform;


    void Awake() {
        if (instance == null) instance = this;
        SetUpdates();
    }

    void Update() {
        SetUpdates();
    }

    void SetUpdates() {
        playerTransform = transform;
    }

    public Transform GetPlayerTransform() { return playerTransform; }

}

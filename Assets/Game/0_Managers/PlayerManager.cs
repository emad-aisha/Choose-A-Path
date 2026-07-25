using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    GameObject player;
    Transform playerTransform;


    void Awake() {
        if (instance == null) instance = this;
        player = GameObject.FindGameObjectWithTag("Player");

        playerTransform = player.transform;
    }

    void Update() {
        playerTransform = player.transform;
    }

    public Transform GetPlayerTransform() {
        return playerTransform;
    }

}

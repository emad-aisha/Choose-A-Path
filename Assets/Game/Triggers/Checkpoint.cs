using UnityEngine;

public class Checkpoint : MonoBehaviour {
    [SerializeField] Transform respawnPoint;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player Parent")) {
            PlayerManager.instance.SetRespawnPoint(respawnPoint.position);
            Debug.Log("Checkpoint Got");
        }
    }

}

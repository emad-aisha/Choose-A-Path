using UnityEngine;

public class Checkpoint : MonoBehaviour {
    [SerializeField] Transform respawnPoint;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player Parent")) {
            PlayerManager.instance.SetCheckPoint(respawnPoint.position);
            Debug.Log("Checkpoint Got");
        }
    }

}

using UnityEngine;

public class KillBox : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.TryGetComponent(out Health playerHealth);
            if (playerHealth == null) return;

            playerHealth.Hurt(1000000);
        }
    }
}

using UnityEngine;

public class Water : MonoBehaviour {


    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (!other.TryGetComponent(out Health playerHealth)) return;
            playerHealth.Hurt(1);

            StartCoroutine(PlayerManager.instance.GoToLastCheckpoint(0.5f));
        }

    }

}

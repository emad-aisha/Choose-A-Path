using System.Collections;
using UnityEngine;

public class HitHead : MonoBehaviour {
    [SerializeField] float timeToWait;
    bool canCheckCollision = true;


    void OnTriggerEnter(Collider other) {
        if (canCheckCollision && !other.CompareTag("Player") && !other.CompareTag("Enemy")) {
            PlayerManager.instance.ResetJumpVelocity();
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait() {
        canCheckCollision = false;
        yield return new WaitForSeconds(timeToWait);
        canCheckCollision = true;
    }

}

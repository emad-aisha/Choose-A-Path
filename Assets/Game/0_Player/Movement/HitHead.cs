using UnityEngine;

public class HitHead : MonoBehaviour {
    [SerializeField] float timeToWait;
    float internalTimer;
    bool canCheckCollision = true;


    void Start() {
        internalTimer = timeToWait;
    }

    void Update() {
        if (internalTimer >= timeToWait) {
            canCheckCollision = true;
        }
        else if (internalTimer < timeToWait) {
            internalTimer += Time.deltaTime;
        }

        // only reset time to wait if JUST checked
        if (canCheckCollision == false) {
            internalTimer = 0;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (canCheckCollision && !other.CompareTag("Player") && !other.CompareTag("Enemy")) {
            PlayerManager.instance.ResetJumpVelocity();
            canCheckCollision = false;
        }
    }

}

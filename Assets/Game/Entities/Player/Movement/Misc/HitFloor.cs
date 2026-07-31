using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFloor : MonoBehaviour {
    [SerializeField] List<string> ignoreTags;
    [SerializeField] float timeToWait;
    bool canCheckCollision = true;


    void OnTriggerEnter(Collider other) {
        bool comparison = NotTags(other);
        if (canCheckCollision && comparison) {
            PlayerManager.instance.GetMovementController().SetGrounded(true);
            StartCoroutine(Wait());
        }
    }

    void OnTriggerExit(Collider other) {
        bool comparison = NotTags(other);
        if (canCheckCollision && comparison) {
            PlayerManager.instance.GetMovementController().SetGrounded(false);
            StartCoroutine(Wait());
        }
    }


    bool NotTags(Collider collider) {
        for (int i = 0; i < ignoreTags.Count; i++) {
            if (collider.CompareTag(ignoreTags[i])) return false;
        }
        return true;
    }

    IEnumerator Wait() {
        canCheckCollision = false;
        yield return new WaitForSeconds(timeToWait);
        canCheckCollision = true;
    }

}

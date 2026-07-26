using UnityEngine;

public class Hitbox : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Debug.Log("attack");
        }
    }

}

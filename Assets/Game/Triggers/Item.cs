using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField] GameObject reward;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player Parent")) {
            Instantiate(reward, other.transform);
            Destroy(gameObject);
        }
    }

}

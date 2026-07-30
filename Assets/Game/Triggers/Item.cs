using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField] GameObject reward;
    [SerializeField] List<Door> doorsToShut;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player Parent")) {
            ShutDoors();

            Instantiate(reward, other.transform);
            Destroy(gameObject);
        }
    }

    void ShutDoors() {
        for (int i = 0; i < doorsToShut.Count; i++) {
            doorsToShut[i].SetOpen(false);
        }
    }
}

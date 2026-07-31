using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
    [SerializeField] List<Door> doors;
    [SerializeField] bool doesOpen;


    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player Parent")) {
            OpenDoors(doesOpen);
        }
    }


    void OpenDoors(bool value) {
        for (int i = 0; i < doors.Count; i++) {
            doors[i].SetOpen(value);
        }
    }


}

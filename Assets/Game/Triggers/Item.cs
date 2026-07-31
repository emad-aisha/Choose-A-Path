using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField] GameObject reward;
    [SerializeField] List<Door> doorsToShut;
    [SerializeField] List<Door> doorsToOpen;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player Parent")) {
            ShutDoors();
            OpenDoors();

            Instantiate(reward, other.transform);
            if (reward.TryGetComponent(out Grapple ignore)) AbilityManager.instance.SetGrappleAbility(reward);
            else if (reward.TryGetComponent(out Top ignore2)) AbilityManager.instance.SetTopAbility(reward);
            else if (reward.TryGetComponent(out Weapon ignore3)) AbilityManager.instance.SetWeaponAbility(reward);

            Destroy(gameObject);
        }
    }

    void ShutDoors() {
        for (int i = 0; i < doorsToShut.Count; i++) {
            doorsToShut[i].SetOpen(false);
        }
    }

    void OpenDoors() {
        for (int i = 0; i < doorsToOpen.Count; i++) {
            doorsToOpen[i].SetOpen(true);
        }
    }
}

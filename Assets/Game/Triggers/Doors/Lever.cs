using UnityEngine;

public class Lever : MonoBehaviour {
    [SerializeField] Door door;
    [SerializeField] Health health;


    void Update() {
        if (health && health.IsDead()) {
            door.SetOpen(true);
            health.enabled = false;
            Destroy(health);
        }
    }

}

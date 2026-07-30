using UnityEngine;

public class Lever : MonoBehaviour {
    [SerializeField] Door door;
    Health health;

    void Start() {
        health = GetComponent<Health>();
    }

    void Update() {
        if (health.IsDead()) {
            door.SetOpen(true);
        }
    }




}

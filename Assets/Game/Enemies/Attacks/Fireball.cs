using UnityEngine;

public class Fireball : MonoBehaviour {
    [SerializeField] float distanceRange;
    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] bool isHoning;

    void Start() {

    }

    void Update() {
        // if within range, throw fireball towards player
        // if isHoning, following towards player
    }
}

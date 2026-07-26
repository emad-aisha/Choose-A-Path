using UnityEngine;

public class Fireball : BasicAttack {
    [SerializeField] float speed;
    [SerializeField] bool isHoning;

    void Start() {

    }

    void Update() {
        // if within range, throw fireball towards player
        // if isHoning, following towards player
    }

    public override void Attack() {

    }
}

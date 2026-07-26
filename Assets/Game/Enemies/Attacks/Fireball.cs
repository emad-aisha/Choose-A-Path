using UnityEngine;

public class Fireball : BasicAttack {
    enum Type { None, Honing, Boomerang };
    [Header("Fireball Stats")]
    [SerializeField] Type type;
    [SerializeField] float speed;
    [SerializeField] int numberOfFireballs;


    void Start() {

    }

    void Update() {
        // if within range, throw fireball towards player
        // if isHoning, following towards player
    }

    public override void Attack() {

    }
}

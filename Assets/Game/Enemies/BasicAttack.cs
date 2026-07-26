using UnityEngine;

public abstract class BasicAttack : MonoBehaviour {
    [Header("Basic Attack Stats")]
    [SerializeField] protected float range;
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldown;
    float internalTimer;


    void Start() {

    }

    void Update() {
        // if withing range, dash to attack
    }

    public abstract void Attack();
}

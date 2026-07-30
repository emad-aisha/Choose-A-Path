using UnityEngine;

public class Hitbox : MonoBehaviour {
    int damage;
    bool hitSomething;
    Vector3 hitDirection;

    void OnEnable() { hitSomething = false; }




    public void SetDamage(int _damage) { damage = _damage; }
    public bool GetHitSomething() { return hitSomething; }
    public Vector3 GetHitDirection() { return hitDirection; }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || !other.CompareTag("Player Parent")) {
            other.TryGetComponent(out Health health);
            if (!health) return;

            health.Hurt(damage);
            hitSomething = true;
            hitDirection = other.transform.position;
        }
    }

}

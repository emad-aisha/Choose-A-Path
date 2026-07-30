using System.Collections;
using UnityEngine;

public class Hurtbox : MonoBehaviour {
    int damage;
    bool isDestroyedOnHit = true;

    public void SetDestroyOnHit(bool onHit) { isDestroyedOnHit = onHit; }

    public void SetDamage(int newDamage) { damage = newDamage; }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.TryGetComponent(out Health playerHealth);
            if (!playerHealth) return;

            playerHealth.Hurt(damage);
            if (isDestroyedOnHit) Destroy(gameObject);
        }
    }


}

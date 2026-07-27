using UnityEngine;

public class Hurtbox : MonoBehaviour {
    int damage;
    bool isDestroyedOnHit = true;

    public void SetDestroyOnHit(bool onHit) { isDestroyedOnHit = onHit; }

    public void SetDamage(int newDamage) { damage = newDamage; }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Health playerHealth;
            other.TryGetComponent(out playerHealth);
            if (!playerHealth) return;


            playerHealth.Hurt(damage);
            if (isDestroyedOnHit) Destroy(gameObject);
        }
    }

}

using UnityEngine;

public class Hurtbox : MonoBehaviour {
    int damage;

    public void SetDamage(int newDamage) { damage = newDamage; }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Health playerHealth;
            other.TryGetComponent(out playerHealth);
            if (!playerHealth) return;


            playerHealth.Hurt(damage);
            Destroy(gameObject);
        }
    }

}

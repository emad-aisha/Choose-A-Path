using UnityEngine;

public class Hitbox : MonoBehaviour {
    int damage;

    public void SetDamage(int _damage) {
        damage = _damage;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Debug.Log("attack");
            other.GetComponent<Health>().Hurt(damage);
        }
    }

}

using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] int maxHealth;
    int currentHealth;
    bool isDead;

    void Start() {
        currentHealth = maxHealth;
        isDead = false;
    }

    void Update() {
        if (isDead) {
            // TODO: do somethign when the player dies
            Debug.Log(gameObject + " died");
            // TODO: play a flashy animation idk
            if (gameObject.CompareTag("Enemy")) Destroy(gameObject);
        }
    }


    public void Hurt(int damangeAmount) {
        currentHealth -= damangeAmount;
        if (currentHealth < 0) {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void Heal(int healAmount) {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

}

using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] int maxHealth;
    [SerializeField] float Iframes;
    int currentHealth;
    bool isDead;

    bool canBeHurt;

    void Start() {
        currentHealth = maxHealth;
        isDead = false;
        canBeHurt = true;
    }

    void Update() {
        if (isDead) {
            // TODO: do somethign when the player dies
            Debug.Log(name + " died");
            // TODO: play a flashy animation idk
            if (gameObject.CompareTag("Enemy")) Destroy(gameObject);
        }
    }


    public void Hurt(int damangeAmount) {
        if (!canBeHurt) return;
        Debug.Log(name + " got hurt " + damangeAmount);
        currentHealth -= damangeAmount;
        StartCoroutine(IFrames());
        if (currentHealth <= 0) {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void Heal(int healAmount) {
        Debug.Log(name + " got healed " + healAmount);
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    IEnumerator IFrames() {
        canBeHurt = false;
        yield return new WaitForSeconds(Iframes);
        canBeHurt = true;
    }

}

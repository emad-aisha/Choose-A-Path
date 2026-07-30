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

            enabled = false;
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

    public bool IsDead() { return isDead; }
    public int GetHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }


    bool isTimePaused;
    IEnumerator IFrames() {
        if (gameObject.CompareTag("Player")) {
            if (!isTimePaused) StartCoroutine(PauseTime());
            PlayerManager.instance.GetMovementController().SetKnockback(RandomDirection(), 0.2f);
        }

        canBeHurt = false;
        yield return new WaitForSeconds(Iframes);
        canBeHurt = true;
    }

    IEnumerator PauseTime() {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isTimePaused = true;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = originalTimeScale;
        isTimePaused = false;
    }

    Vector3 RandomDirection() {
        Vector3 returnValue = new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), 0) * 3;
        return returnValue;
    }

}

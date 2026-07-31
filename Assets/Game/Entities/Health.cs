using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Color deadRed = Color.darkRed;
    [SerializeField] int maxHealth;
    int currentHealth;

    [Header("IFrame Stats")]
    [SerializeField] float IFrames;
    [SerializeField] float flashTimes;

    [Header("IFrame Colors")]
    [SerializeField] Color grayedOut;
    [SerializeField] Color lightGrayedOut;

    bool isDead;
    bool canBeHurt;

    void Start() {
        currentHealth = maxHealth;
        isDead = false;
        canBeHurt = true;
    }


    void Update() {
        if (isDead) {
            Debug.Log(name + " died");

            if (gameObject.CompareTag("Enemy")) Destroy(gameObject);
            if (gameObject.CompareTag("Player")) {
                // TODO: play a flashy animation idk
                ResetHealth();
                StartCoroutine(PlayerManager.instance.Respawn(1));
                return;
            }

            enabled = false;
        }
    }


    public void Hurt(int damangeAmount) {
        if (!canBeHurt) return;
        Debug.Log(name + " got hurt " + damangeAmount);
        currentHealth -= damangeAmount;

        if (currentHealth <= 0) {
            if (sprite) sprite.color = deadRed;

            currentHealth = 0;
            isDead = true;
        }
        else {
            StartCoroutine(IFrameTime());
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

    void ResetHealth() {
        currentHealth = maxHealth;
        isDead = false;
        canBeHurt = true;
    }

    bool isTimePaused;
    public IEnumerator IFrameTime(bool ignoreKnockback = false) {
        if (gameObject.CompareTag("Player")) {
            if (!isTimePaused) StartCoroutine(PauseTime());
            if (!isDead && !ignoreKnockback) PlayerManager.instance.GetMovementController().SetKnockback(RandomDirection(), 0.2f);
        }
        canBeHurt = false;

        float time = 0;
        float flashTime = IFrames / flashTimes;
        while (time < IFrames) {
            PlayerManager.instance.SetSpriteRendererColor(lightGrayedOut);
            yield return new WaitForSeconds(flashTime);
            time += flashTime;

            PlayerManager.instance.SetSpriteRendererColor(grayedOut);
            yield return new WaitForSeconds(flashTime);
            time += flashTime;
        }

        PlayerManager.instance.ResetSpriteRendererColor();
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

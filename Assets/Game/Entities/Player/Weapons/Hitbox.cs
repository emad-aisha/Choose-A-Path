using System.Collections;
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
            StartCoroutine(PauseTime());
            hitSomething = true;
            hitDirection = other.transform.position;
        }
    }


    bool isTimePaused;
    IEnumerator PauseTime() {
        if (isTimePaused) yield break;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isTimePaused = true;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = originalTimeScale;
        isTimePaused = false;
    }

}

using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;

    bool isAttacking;
    bool isWinding;

    void Update() {
        if (Time.timeScale == 0) return;
        if (!animator.gameObject.activeSelf) return; // if inactive, dont do anything
        FlipSprite();

        animator.SetBool("is Attacking", isAttacking);
        animator.SetBool("is Winding", isWinding);
    }

    public void SetAttacking(bool value) {
        isAttacking = value;
    }

    public void SetWinding(bool value) {
        isWinding = value;
    }


    void FlipSprite() {

    }
}

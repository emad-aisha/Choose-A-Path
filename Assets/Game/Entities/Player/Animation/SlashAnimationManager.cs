using UnityEngine;

public class SlashAnimationManager : MonoBehaviour {
    public static SlashAnimationManager instance;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;

    bool isAttacking;
    int upOrDown; // 0 is neither
    int leftOrRight; // flips sprite renderer accordingly

    void Awake() {
        if (instance == null) instance = this;
    }

    void Update() {
        if (Time.timeScale == 0) return;
        FlipSprite();
        if (!animator.gameObject.activeSelf) return; // if inactive, dont do anything

        animator.SetBool("Is Attacking", isAttacking);
        animator.SetInteger("Up or Down", upOrDown);
        animator.SetInteger("Left or Right", leftOrRight);
    }


    public void SetIsAttacking(bool value) { isAttacking = value; }
    public void SetUpOrDown(int value) { upOrDown = value; }
    public void SetLeftOrRight(int value) { leftOrRight = value; }


    void FlipSprite() {
        if (leftOrRight < 0) {
            sprite.flipX = true;
        }
        else if (leftOrRight > 0) {
            sprite.flipX = false;
        }
    }
}

using Unity.Mathematics;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
    public static AnimationManager instance;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;

    float runSpeed;
    bool isGrounded;
    bool isJumping;

    bool isGrappling;
    bool isHorizontal;
    bool isVertical;


    bool hitTarget;


    void Awake() {
        if (instance == null) instance = this;
    }

    void Update() {
        FlipSprite();

        animator.SetFloat("Run Speed", math.abs(runSpeed));
        animator.SetBool("Is Grounded", isGrounded);
        animator.SetBool("Is Jumping", isJumping);

        animator.SetBool("Is Grappling", isGrappling);
        animator.SetBool("Is Horizontal", isHorizontal);
        animator.SetBool("Is Vertical", isVertical);
        animator.SetBool("Hit Target", hitTarget);
    }

    public void SetRunSpeed(float newSpeed) { runSpeed = newSpeed; }
    public void SetIsGrounded(bool boolValue) { isGrounded = boolValue; }
    public void SetIsJumping(bool boolValue) { isJumping = boolValue; }

    public void SetIsGrappling(bool boolValue) { isGrappling = boolValue; }
    public void SetIsHorizontal(bool boolValue) { isHorizontal = boolValue; }
    public void SetIsVertical(bool boolValue) { isVertical = boolValue; }
    public void SetHitTarget(bool boolValue) { hitTarget = boolValue; }



    // helpers
    void FlipSprite() {
        if (runSpeed < 0) {
            sprite.flipX = runSpeed < 0;
        }
        else if (runSpeed > 0) {
            sprite.flipX = false;
        }
    }
}


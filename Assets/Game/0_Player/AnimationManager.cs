using Unity.Mathematics;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
    public static AnimationManager instance;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;

    float runSpeed;
    float jumpSpeed;
    bool isGrounded;
    bool isJumping;


    void Awake() {
        if (instance == null) instance = this;
    }

    void Update() {
        FlipSprite();

        animator.SetFloat("Run Speed", math.abs(runSpeed));
        animator.SetBool("Is Grounded", isGrounded);
        animator.SetBool("Is Jumping", isJumping);
    }

    public void SetRunSpeed(float newSpeed) { runSpeed = newSpeed; }
    public void SetIsGrounded(bool grounded) { isGrounded = grounded; }
    public void SetIsJumping(bool jumped) { isJumping = jumped; }



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


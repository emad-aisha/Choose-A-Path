using Unity.Mathematics;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
    public static AnimationManager instance;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;


    float runSpeed;

    void Awake() {
        if (instance == null) instance = this;
    }

    void Update() {
        FlipSprite();

        animator.SetFloat("Run Speed", math.abs(runSpeed));
    }

    public void SetRunSpeed(float newSpeed) { runSpeed = newSpeed; }



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


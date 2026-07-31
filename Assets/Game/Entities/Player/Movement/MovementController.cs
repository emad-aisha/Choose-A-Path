using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : Input {
    CharacterController controller;
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] LayerMask ignoreLayer;

    [Header("Walk Stats")]
    [SerializeField] float speed;
    [SerializeField] float maxSprint;
    float internalSprint;
    [SerializeField] float sprintTimer;
    float internalSprintTimer;


    [Header("Jump Stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] float maxfallVelocity;

    [Header("Jump Variation")]
    [SerializeField] float jumpMod;
    [SerializeField] float jumpTimer;
    float internalJumpTimer;

    [SerializeField] float coyoteTime;
    float internalCoyoteTimer;

    float maxJumps = 1;
    float jumps;

    Vector3 moveDirection;
    Vector3 jumpVelocity;

    Vector3 knockback;
    bool isGrounded;


    void Start() {
        controller = PlayerManager.instance.GetCharacterController();

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        jumpAction = InputManager.instance.GetAction(actionName, "Jump");

        internalSprint = 1;
        internalJumpTimer = 0;
    }

    bool resetMovement;
    Vector2 moveInput;
    void Update() {
        CheckGrounded();
        moveInput = moveAction.ReadValue<Vector2>();

        if (!controller.isGrounded) {
            internalCoyoteTimer += Time.deltaTime;
        }
        else {
            internalCoyoteTimer = 0;
        }

        //jump
        JumpLogic();
        JumpHeightLogic();

        // walk
        SprintLogic();
        if (!resetMovement) AnimationManager.instance.SetRunSpeed(moveInput.x);

        moveDirection = new Vector3(moveInput.x, jumpVelocity.y, 0); // moveInput.x * transform.right + 0 * transform.forward + jumpVelocity.y * transform.up;
        if (!resetMovement) controller.Move((moveDirection + knockback) * (speed * Time.deltaTime));


        GravityLogic();
    }

    void SprintLogic() {
        if (moveAction.IsPressed()) {
            internalSprintTimer += Time.deltaTime;
        }
        else {
            internalSprintTimer = 0;
        }

        if (internalSprintTimer >= sprintTimer) {
            if (internalSprint < maxSprint) internalSprint += Time.deltaTime;
            if (internalSprint > maxSprint) internalSprint = maxSprint;

            moveInput.x *= internalSprint;
        }
        else {
            internalSprint -= Time.deltaTime;
            if (internalSprint < 1) internalSprint = 1;
        }
    }

    void JumpHeightLogic() {
        if (jumpAction.IsPressed() && !controller.isGrounded) {
            if (internalJumpTimer < jumpTimer) {
                internalJumpTimer += Time.deltaTime;
            }

            if (jumpVelocity.y > 0 && internalJumpTimer <= jumpTimer) {
                jumpVelocity.y += Time.deltaTime * jumpMod;
            }
        }

    }

    void JumpLogic() {
        bool canDoubleJump = maxJumps > 1 && jumps < maxJumps;

        if (canDoubleJump && jumpAction.WasPressedThisFrame()) {
            AnimationManager.instance.SetIsJumping(true);
            //PlayerManager.instance.UpdateCharacterCapsule(1.4f, new Vector3(0, -0.02f, 0));

            // dont allow double jump off air
            if (!controller.isGrounded && internalCoyoteTimer > coyoteTime) jumps++;
            jumpVelocity.y = jumpSpeed;
            jumps++;
        }
        else if (jumpAction.WasPressedThisFrame() && (controller.isGrounded || internalCoyoteTimer < coyoteTime)) {
            AnimationManager.instance.SetIsJumping(true);
            //PlayerManager.instance.UpdateCharacterCapsule(1.4f, new Vector3(0, -0.02f, 0));

            jumpVelocity.y = jumpSpeed;
            jumps++;
        }
        else {
            AnimationManager.instance.SetIsJumping(false);
        }

    }

    void GravityLogic() {
        // gravity logic
        if (controller.isGrounded) {
            jumpVelocity = Vector3.zero;
            internalJumpTimer = 0;
            jumps = 0;
        }
        else if (jumpVelocity.y > -maxfallVelocity) {
            jumpVelocity.y -= gravity * Time.deltaTime;
        }

        if (resetMovement) {
            jumpVelocity = Vector3.zero;
        }
    }

    void CheckGrounded() {
        AnimationManager.instance.SetIsGrounded(isGrounded);
    }

    IEnumerator DecreaseKnockback(float time) {
        while (time > 0) {
            if (knockback.x < 0) knockback.x += speed * Time.deltaTime;
            if (knockback.x > 0) knockback.x -= speed * Time.deltaTime;

            if (knockback.y < 0) knockback.y += speed / 2 * Time.deltaTime;
            if (knockback.y > 0) knockback.y -= speed / 2 * Time.deltaTime;
            time -= Time.deltaTime;
            jumpVelocity = Vector3.zero;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        knockback = Vector3.zero;
    }



    public void ResetJumpVelocity() { jumpVelocity = Vector3.zero; }
    public void SetKnockback(Vector3 velocity, float time) {
        knockback = velocity;
        StartCoroutine(DecreaseKnockback(time));
    }

    public void LockMovement() { resetMovement = true; }
    public void UnlockMovement() { resetMovement = false; }

    public void SetGrounded(bool _isGrounded) { isGrounded = _isGrounded; }
    public bool GetIsGrounded() { return isGrounded; }

    public float GetMovementSpeed() { return speed; }
    public void SetMovementSpeed(float newSpeed) { speed = newSpeed; }

    public void SetMaxJumps(int newJumps) { maxJumps = newJumps; }



}

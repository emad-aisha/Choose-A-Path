using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : Input {
    CharacterController controller;
    InputAction moveAction;
    InputAction jumpAction;

    [Header("Walk Stats")]
    [SerializeField] float speed;
    [SerializeField] float maxSprint;
    float internalSprint;
    [SerializeField] float sprintTimer;
    float internalSprintTimer;


    [Header("Jump Stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;

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
        if (!resetMovement) controller.Move(moveDirection * (speed * Time.deltaTime));


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
            // dont allow double jump off air
            if (!controller.isGrounded && internalCoyoteTimer > coyoteTime) jumps++;
            jumpVelocity.y = jumpSpeed;
            jumps++;
        }
        else if (jumpAction.WasPressedThisFrame() && (controller.isGrounded || internalCoyoteTimer < coyoteTime)) {
            AnimationManager.instance.SetIsJumping(true);
            jumpVelocity.y = jumpSpeed;
            jumps++;
        }
        else {
            AnimationManager.instance.SetIsJumping(false);
        }

    }

    void GravityLogic() {
        AnimationManager.instance.SetIsGrounded(controller.isGrounded);

        // gravity logic
        if (controller.isGrounded) {
            jumpVelocity = Vector3.zero;
            internalJumpTimer = 0;
            jumps = 0;
        }
        else {
            jumpVelocity.y -= gravity * Time.deltaTime;
        }

        if (resetMovement) {
            jumpVelocity = Vector3.zero;
        }
    }



    public void ResetJumpVelocity() { jumpVelocity = Vector3.zero; }

    public void LockMovement() { resetMovement = true; }
    public void UnlockMovement() { resetMovement = false; }


    public float GetMovementSpeed() { return speed; }
    public void SetMovementSpeed(float newSpeed) { speed = newSpeed; }

    public void SetMaxJumps(int newJumps) { maxJumps = newJumps; }


}

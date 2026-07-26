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
    bool isJumping;

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
        controller = GetComponent<CharacterController>();

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        jumpAction = InputManager.instance.GetAction(actionName, "Jump");

        internalSprint = 1;
        internalJumpTimer = 0;
        isJumping = false;
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
        //if (jumpAction.WasPressedThisFrame() && !isJumping && (controller.isGrounded || internalCoyoteTimer < coyoteTime || jumps < maxJumps)) {
        //    jumpVelocity.y = jumpSpeed;
        //    isJumping = true;
        //    jumps++;
        //}
        bool canDoubleJump = maxJumps > 1 && jumps < maxJumps;

        if (canDoubleJump && jumpAction.WasPressedThisFrame()) {
            // dont allow double jump off air
            if (!(controller.isGrounded || internalCoyoteTimer < coyoteTime)) jumps++;
            jumpVelocity.y = jumpSpeed;
            isJumping = true;
            jumps++;
        }
        else if (jumpAction.WasPressedThisFrame() && (controller.isGrounded || internalCoyoteTimer < coyoteTime)) {
            jumpVelocity.y = jumpSpeed;
            isJumping = true;
            jumps++;
        }

    }

    void GravityLogic() {
        // gravity logic
        if (controller.isGrounded) {
            isJumping = false;
            jumpVelocity = Vector3.zero;
            internalJumpTimer = 0;
            jumps = 0;
        }
        else {
            jumpVelocity.y -= gravity * Time.deltaTime;
        }
    }



    public void ResetJumpVelocity() { jumpVelocity = Vector3.zero; }

    public void LockMovement() { resetMovement = true; }
    public void UnlockMovement() { resetMovement = false; }


    public float GetMovementSpeed() { return speed; }
    public void SetMovementSpeed(float newSpeed) { speed = newSpeed; }

    public void SetMaxJumps(int newJumps) { maxJumps = newJumps; }


}

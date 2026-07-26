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
    bool fullyResetMovement;
    Vector2 moveInput;
    void Update() {
        moveInput = moveAction.ReadValue<Vector2>();

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
        if (jumpAction.WasPressedThisFrame() && !isJumping) {
            jumpVelocity.y = jumpSpeed;
            isJumping = true;
        }
    }

    void GravityLogic() {
        // gravity logic
        if (controller.isGrounded) {
            isJumping = false;
            jumpVelocity = Vector3.zero;
            internalJumpTimer = 0;
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


}

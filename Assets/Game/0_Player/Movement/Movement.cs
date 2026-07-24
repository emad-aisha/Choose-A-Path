using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : Input {
    CharacterController controller;
    InputAction moveAction;
    InputAction jumpAction;

    [Header("Walk Stats")]
    [SerializeField] int speed;
    [SerializeField] float maxSprint;
    float internalSprint;
    [SerializeField] float sprintTimer;
    float internalSprintTimer;


    [Header("Jump Stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpMod;
    [SerializeField] float jumpTimer;
    float internalJumpTimer;

    [SerializeField] float gravity;
    bool isJumping = false;

    Vector3 moveDirection;
    Vector3 jumpVelocity;


    void Start() {
        controller = GetComponent<CharacterController>();

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        jumpAction = InputManager.instance.GetAction(actionName, "Jump");

        internalSprint = 1;
        internalJumpTimer = 0;
    }

    Vector3 moveInput;
    void Update() {
        moveInput = moveAction.ReadValue<Vector2>();
        JumpHeightLogic();
        JumpLogic();

        moveDirection = moveInput.x * transform.right + 0 * transform.forward + jumpVelocity.y * transform.up;

        SprintLogic();
        MoveLogic();
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

            moveDirection.x *= internalSprint;
        }
        else {
            internalSprint -= Time.deltaTime;
            if (internalSprint < 1) internalSprint = 1;
        }
    }

    void MoveLogic() {
        controller.Move(moveDirection * (speed * Time.deltaTime));
    }

    void JumpHeightLogic() {
        if (jumpAction.IsPressed() && !controller.isGrounded) {
            if (internalJumpTimer < jumpTimer) {
                internalJumpTimer += Time.deltaTime;
            }

            if (jumpVelocity.y > 0 && internalJumpTimer <= jumpTimer) {
                // TODO: needs polish?
                jumpVelocity.y += Time.deltaTime * jumpMod;
            }
        }

    }

    void JumpLogic() {
        if (jumpAction.IsPressed() && !isJumping) {
            jumpVelocity.y = jumpSpeed;
            isJumping = true;
        }

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


}

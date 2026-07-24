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
    bool isSprinting;

    [Header("Jump Stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    bool isJumping = false;

    Vector3 moveDirection;
    Vector3 jumpVelocity;


    void Start() {
        controller = GetComponent<CharacterController>();

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        jumpAction = InputManager.instance.GetAction(actionName, "Jump");

        internalSprint = 1;
    }

    Vector3 moveInput;
    void Update() {
        moveInput = moveAction.ReadValue<Vector2>();

        SprintLogic();
        MoveLogic();
        JumpLogic();
    }

    void SprintLogic() {
        if (moveAction.IsPressed()) {
            internalSprintTimer += Time.deltaTime;
        }
        else {
            internalSprintTimer = 0;
            isSprinting = false;
        }

        if (internalSprintTimer >= sprintTimer) {
            isSprinting = true;
        }
    }

    void MoveLogic() {
        moveDirection = moveInput.x * transform.right + 0 * transform.forward + jumpVelocity.y * transform.up;

        if (isSprinting) {
            if (internalSprint < maxSprint) internalSprint += Time.deltaTime;
            if (internalSprint > maxSprint) internalSprint = maxSprint;

            moveDirection.x *= internalSprint;
        }
        else if (internalSprint > 1) {
            internalSprint -= Time.deltaTime;
            if (internalSprint < 1) internalSprint = 1;
        }

        controller.Move(moveDirection * (speed * Time.deltaTime));
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
        }
        else {
            jumpVelocity.y -= gravity * Time.deltaTime;
        }
    }


}

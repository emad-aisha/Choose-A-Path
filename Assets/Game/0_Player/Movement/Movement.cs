using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : Input {
    CharacterController controller;
    InputAction moveAction;
    InputAction jumpAction;

    [Header("Stats")]
    [SerializeField] int speed;
    [SerializeField] float sprintMod;
    float sprint;

    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;


    [Header("data")]
    [SerializeField] float sprintTimer;
    float internalTimer;
    bool isSprinting;


    Vector3 moveDirection;
    Vector3 moveInput;
    Vector3 jumpVelocity;



    bool isJumping = false;

    void Start() {
        controller = GetComponent<CharacterController>();

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        jumpAction = InputManager.instance.GetAction(actionName, "Jump");

        sprint = 1;
    }

    void Update() {
        MoveLogic();
        JumpLogic();
    }


    void MoveLogic() {
        // moving logic
        moveInput = moveAction.ReadValue<Vector2>();
        // dont move on the z axis
        moveDirection = moveInput.x * transform.right + 0 * transform.forward + jumpVelocity.y * transform.up;

        // sprint
        if (!moveAction.IsPressed()) {
            internalTimer = 0;
            isSprinting = false;
        }
        else {
            internalTimer += Time.deltaTime;
        }

        if (internalTimer >= sprintTimer) { // if moving for long enough
            isSprinting = true;
        }


        if (isSprinting) {
            if (sprint < sprintMod) sprint += Time.deltaTime;
            if (sprint > sprintMod) sprint = sprintMod;
            moveDirection.x *= sprint;
            moveDirection.z *= sprint;
        }
        else if (sprint > 1) {
            sprint -= Time.deltaTime;
            if (sprint < 1) sprint = 1;
        }
        controller.Move(moveDirection * speed * Time.deltaTime);

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

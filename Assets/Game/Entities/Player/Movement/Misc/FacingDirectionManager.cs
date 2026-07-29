using UnityEngine;
using UnityEngine.InputSystem;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;
    [SerializeField] float yOffset;

    [SerializeField] float upperBounds;
    [SerializeField] float lowerBounds;
    Vector3 playerPosition;

    InputAction mousePos;
    InputAction moveAction;
    Vector2 moveDirection;

    Vector3 horizontalPosition;
    Vector3 verticalPosition;
    bool locked;

    void Awake() {
        if (instance == null) instance = this;
        mousePos = InputManager.instance.GetAction(actionName, "Mouse Position");
        moveAction = InputManager.instance.GetAction(actionName, "Move");

        playerPosition = PlayerManager.instance.GetTransform().position;
        locked = false;
    }

    void Update() {
        if (Time.timeScale == 0) return;
        playerPosition = PlayerManager.instance.GetTransform().position;

        moveDirection.y = mousePos.ReadValue<Vector2>().y;
        moveDirection.y -= Screen.height / 2;
        if (moveAction.ReadValue<Vector2>().x != 0) moveDirection.x = moveAction.ReadValue<Vector2>().x;

        if (moveDirection.y < lowerBounds) moveDirection.y = -1;
        else if (moveDirection.y < upperBounds) moveDirection.y = 0;
        else moveDirection.y = 1;

        if (moveDirection.x < 0) moveDirection.x = -1;
        else if (moveDirection.x > 0) moveDirection.x = 1;

        // save last facing direction
        //if (!locked) {
        transform.position = new Vector3(moveDirection.x, moveDirection.y + yOffset, 0) + playerPosition;
        horizontalPosition = new Vector3(moveDirection.x, 0, 0) + playerPosition;
        verticalPosition = new Vector3(0, moveDirection.y + yOffset, 0) + playerPosition;
        //}
    }


    public Vector3 GetFacingDirection() { return (transform.position - playerPosition).normalized; }
    public Vector3 GetHorizontalDirection() { return (horizontalPosition - playerPosition).normalized; }
    public Vector3 GetUpwardsDirection() { return PlayerManager.instance.GetTransform().up.normalized; }

    public Vector3 GetAttackDirection() {
        Vector3 returnValue;

        if (PlayerManager.instance.GetMovementController().GetIsGrounded()) {
            if ((verticalPosition - playerPosition).y == 1) returnValue = PlayerManager.instance.GetTransform().up.normalized;
            else returnValue = (horizontalPosition - playerPosition).normalized;
        }
        else {
            if ((verticalPosition - playerPosition).y == 1 || (verticalPosition - playerPosition).y == -1) returnValue = (verticalPosition - playerPosition).normalized;
            else returnValue = (horizontalPosition - playerPosition).normalized;
        }

        return returnValue;
    }

    public void LockDirection() { locked = true; }
    public void UnlockDirection() { locked = false; }
}

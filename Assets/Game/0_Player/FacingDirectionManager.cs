using UnityEngine;
using UnityEngine.InputSystem;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;
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
        playerPosition = PlayerManager.instance.GetTransform().position;
        moveDirection.y = mousePos.ReadValue<Vector2>().y;
        if (moveAction.ReadValue<Vector2>().x != 0) moveDirection.x = moveAction.ReadValue<Vector2>().x;

        if (moveDirection.x < 0) moveDirection.x = -1;
        else if (moveDirection.x > 0) moveDirection.x = 1;

        // save last facing direction
        if (!locked && moveDirection != Vector2.zero) {
            transform.position = new Vector3(moveDirection.x, moveDirection.y, 0) + playerPosition;
            horizontalPosition = new Vector3(moveDirection.x, 0, 0) + playerPosition;
            verticalPosition = new Vector3(0, moveDirection.y, 0) + playerPosition;
        }
    }


    public Vector3 GetFacingDirection() { return (transform.position - PlayerManager.instance.GetTransform().position).normalized; }
    public Vector3 GetHorizontalDirection() { return (horizontalPosition - PlayerManager.instance.GetTransform().position).normalized; }
    public Vector3 GetUpwardsDirection() { return PlayerManager.instance.GetTransform().up.normalized; }

    public void LockDirection() { locked = true; }
    public void UnlockDirection() { locked = false; }
}

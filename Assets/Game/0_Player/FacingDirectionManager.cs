using UnityEngine;
using UnityEngine.InputSystem;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;
    Vector3 playerPosition;

    InputAction mousePos;
    Vector2 moveDirection;

    Vector3 horizontalPosition;
    Vector3 verticalPosition;
    bool locked;

    void Awake() {
        if (instance == null) instance = this;
        mousePos = InputManager.instance.GetAction(actionName, "Mouse Position");
        playerPosition = PlayerManager.instance.GetTransform().position;
        locked = false;
    }

    void Update() {
        playerPosition = PlayerManager.instance.GetTransform().position;
        moveDirection = mousePos.ReadValue<Vector2>();
        moveDirection.x -= Screen.width / 2;
        moveDirection.x /= Screen.width;

        moveDirection.y -= Screen.height / 2;
        moveDirection.y /= Screen.height;

        if (moveDirection.x < 0) moveDirection.x = -1;
        else if (moveDirection.x > 0) moveDirection.x = 1;

        if (moveDirection.y < 0) moveDirection.y = -1;
        else if (moveDirection.y > 0) moveDirection.y = 1;

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

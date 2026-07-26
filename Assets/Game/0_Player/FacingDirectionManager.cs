using UnityEngine;
using UnityEngine.InputSystem;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;
    Vector3 playerPosition;

    InputAction moveAction;
    float moveDirection;
    bool locked;

    void Awake() {
        if (instance == null) instance = this;
        moveAction = InputManager.instance.GetAction(actionName, "Move");
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;
        locked = false;
    }

    void Update() {
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;
        moveDirection = moveAction.ReadValue<Vector2>().x;

        // save last facing direction
        if (!locked && moveDirection != 0) transform.position = new Vector3(moveDirection, 0, 0) + playerPosition;
    }


    public Vector3 GetFacingDirection() { return (transform.position - PlayerManager.instance.GetPlayerTransform().position).normalized; }
    public Vector3 GetUpwardsDirection() { return PlayerManager.instance.GetPlayerTransform().up.normalized; }

    public void LockDirection() { locked = true; }
    public void UnlockDirection() { locked = false; }
}

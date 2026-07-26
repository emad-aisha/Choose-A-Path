using UnityEngine;
using UnityEngine.InputSystem;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;
    Vector3 playerPosition;

    InputAction moveAction;
    float moveDirection;

    void Awake() {
        if (instance == null) instance = this;
        moveAction = InputManager.instance.GetAction(actionName, "Move");
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;
    }

    void Update() {
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;
        moveDirection = moveAction.ReadValue<Vector2>().x;

        transform.position = new Vector3(moveDirection, 0, 0) + playerPosition;
    }


}

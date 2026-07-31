using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;

    [Header("Bounds")]
    [SerializeField] Image upBounds;
    [SerializeField] Image lowBounds;
    [SerializeField, Range(0, 1080 / 2)] float upperBounds;
    [SerializeField, Range(0, 1080 / 2)] float lowerBounds;
    float negativeLowerBounds;
    [SerializeField] bool isDebugging;

    Vector3 playerPosition;

    InputAction mousePos;
    InputAction moveAction;
    Vector2 moveDirection;

    Vector3 horizontalPosition;
    Vector3 verticalPosition;

    void Awake() {
        if (instance == null) instance = this;
        UpdateBounds();

        mousePos = InputManager.instance.GetAction(actionName, "Mouse Position");
        moveAction = InputManager.instance.GetAction(actionName, "Move");

        playerPosition = PlayerManager.instance.GetTransform().position;
        moveDirection.x = 1;
    }

    void Update() {
        if (Time.timeScale == 0) return;
        UpdateBounds();
        playerPosition = PlayerManager.instance.GetTransform().position;

        moveDirection.y = mousePos.ReadValue<Vector2>().y;
        moveDirection.y -= Screen.height / 2;
        if (moveAction.ReadValue<Vector2>().x != 0) moveDirection.x = moveAction.ReadValue<Vector2>().x;
        ClampMoveDirection();

        transform.position = new Vector3(moveDirection.x, moveDirection.y) + playerPosition;
        horizontalPosition = new Vector3(moveDirection.x, 0) + playerPosition;
        verticalPosition = new Vector3(0, moveDirection.y) + playerPosition;
    }


    public Vector3 GetFacingDirection() { return (transform.position - playerPosition).normalized; }
    public Vector3 GetHorizontalDirection() { return (horizontalPosition - playerPosition).normalized; }
    public Vector3 GetUpwardsDirection() { return PlayerManager.instance.GetTransform().up.normalized; }

    public Vector3 GetAttackDirection() {
        if (PlayerManager.instance.GetMovementController().GetIsGrounded()) {
            if (math.round((verticalPosition - playerPosition).y) == 1) {
                SetAnimationDirection((int)math.round((horizontalPosition - playerPosition).x), 1);
                return PlayerManager.instance.GetTransform().up.normalized;
            }
            else {
                SetAnimationDirection((int)math.round((horizontalPosition - playerPosition).x), (int)math.round((horizontalPosition - playerPosition).y));
                return (horizontalPosition - playerPosition).normalized;
            }
        }
        else {
            if ((verticalPosition - playerPosition).y != 0) {
                SetAnimationDirection((int)math.round((horizontalPosition - playerPosition).x), (int)math.round((verticalPosition - playerPosition).y));
                return (verticalPosition - playerPosition).normalized;
            }
            else {
                SetAnimationDirection((int)math.round((horizontalPosition - playerPosition).x), (int)math.round((horizontalPosition - playerPosition).y));
                return (horizontalPosition - playerPosition).normalized;
            }
        }
    }

    void SetAnimationDirection(int leftOrRight, int upOrDown) {
        AnimationManager.instance.SetUpOrDown(upOrDown);
        SlashAnimationManager.instance.SetUpOrDown(upOrDown);

        AnimationManager.instance.SetLeftOrRight(leftOrRight);
        SlashAnimationManager.instance.SetLeftOrRight(leftOrRight);
    }



    void ClampMoveDirection() {
        if (moveDirection.y < negativeLowerBounds) moveDirection.y = -1;
        else if (moveDirection.y < upperBounds) moveDirection.y = 0;
        else moveDirection.y = 1;

        if (moveDirection.x < 0) moveDirection.x = -1;
        else if (moveDirection.x > 0) moveDirection.x = 1;
    }

    void UpdateBounds() {
        if (isDebugging) {
            upBounds.GetComponent<RectTransform>().sizeDelta = new Vector2(0, upperBounds);
            if (lowerBounds > 0) {
                lowBounds.GetComponent<RectTransform>().sizeDelta = new Vector2(0, lowerBounds);
                negativeLowerBounds = -lowerBounds;
            }
        }
        else {
            upBounds.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            lowBounds.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
    }

}

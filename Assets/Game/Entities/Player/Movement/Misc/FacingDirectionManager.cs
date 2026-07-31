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
    [SerializeField] Color boundsColor;
    [SerializeField] bool isDebugging;

    Vector3 playerPosition;

    InputAction mousePos;
    InputAction moveAction;
    Vector2 moveDirection;

    Vector3 horizontalPosition;
    Vector3 verticalPosition;

    void Awake() {
        if (instance == null) instance = this;
        UpdateBoundsVisualizer();

        mousePos = InputManager.instance.GetAction(actionName, "Mouse Position");
        moveAction = InputManager.instance.GetAction(actionName, "Move");

        playerPosition = PlayerManager.instance.GetTransform().position;
        moveDirection.x = 1;
    }

    void Update() {
        if (Time.timeScale == 0) return;
        playerPosition = PlayerManager.instance.GetTransform().position;

        if (moveAction.ReadValue<Vector2>().x != 0) moveDirection.x = moveAction.ReadValue<Vector2>().x;
        UpdateBoundsY(playerPosition.y);
        UpdateBoundsVisualizer();

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

    void ClampYMoveDirection() {
        Vector3 mousePosition = mousePos.ReadValue<Vector2>();

        bool inBounds = RectTransformUtility.RectangleContainsScreenPoint(upBounds.GetComponent<RectTransform>(), mousePosition) ||
            RectTransformUtility.RectangleContainsScreenPoint(lowBounds.GetComponent<RectTransform>(), mousePosition);
        bool isOverBounds = mousePosition.y > upBounds.GetComponent<RectTransform>().rect.yMax + upBounds.GetComponent<RectTransform>().position.y;

        if (inBounds) moveDirection.y = 0;
        else if (isOverBounds) moveDirection.y = 1;
        else moveDirection.y = -1;
    }

    void ClampMoveDirection() {
        ClampYMoveDirection();
        if (moveDirection.x < 0) moveDirection.x = -1;
        else if (moveDirection.x > 0) moveDirection.x = 1;
    }

    void UpdateBoundsY(float y) {
        Vector3 position = new Vector3(Camera.main.WorldToScreenPoint(playerPosition).x, Camera.main.WorldToScreenPoint(playerPosition).y, 0);

        upBounds.GetComponent<RectTransform>().position = position;
        lowBounds.GetComponent<RectTransform>().position = position;
    }

    void UpdateBoundsVisualizer() {
        upBounds.GetComponent<RectTransform>().sizeDelta = new Vector2(0, upperBounds);
        lowBounds.GetComponent<RectTransform>().sizeDelta = new Vector2(0, lowerBounds);
        negativeLowerBounds = -lowerBounds;

        if (!isDebugging) {
            upBounds.color = new Color(0, 0, 0, 0);
            lowBounds.color = new Color(0, 0, 0, 0);
        }
        else {
            upBounds.color = boundsColor;
            lowBounds.color = boundsColor;
        }
    }

}

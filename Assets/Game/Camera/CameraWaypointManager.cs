using Unity.Mathematics;
using UnityEngine;

public class CameraWaypointManager : MonoBehaviour {
    public static CameraWaypointManager instance;

    [Header("Bounds")]
    [SerializeField] GameObject rightBounds;
    [SerializeField] GameObject leftBounds;
    [SerializeField] GameObject upBounds;
    [SerializeField] GameObject downBounds;
    [SerializeField, InspectorName("Horizontal Clamp")] float horizontalDistanceFromBounds;
    [SerializeField, InspectorName("Vertical Clamp")] float verticalDistanceFromBounds;

    [Header("Waypoints")]
    [SerializeField] GameObject waypoint;
    [SerializeField] float distanceLimit;
    [SerializeField] float yOffset;

    [Header("Tween")]
    [SerializeField, Range(0, 0.2f)] float XmovePercentage;
    [SerializeField, Range(0, 0.2f)] float YmovePercentage;

    Vector3 playerPosition;
    Vector3 movePosition;

    void Awake() {
        if (instance == null) instance = this;
    }

    void Update() {
        UpdatePlayerPosition();
        UpdatePosition();
    }

    void UpdatePlayerPosition() {
        playerPosition = PlayerManager.instance.GetTransform().position;

        if (!PlayerManager.instance.GetMovementController().GetIsGrounded()) {
            // if player is too far, move
            if (math.distance(playerPosition.y, waypoint.transform.position.y) > distanceLimit) playerPosition.y += yOffset;
            else if (math.distance(playerPosition.y, waypoint.transform.position.y) > distanceLimit * distanceLimit) playerPosition.y += yOffset * distanceLimit;
            else playerPosition.y = waypoint.transform.position.y;
        }
        else {
            playerPosition.y += yOffset;
        }
    }

    void ClampFromBounds() {
        ClampHorizontal(rightBounds);
        ClampHorizontal(leftBounds);

        ClampVertical(upBounds);
        ClampVertical(downBounds);
    }


    void UpdatePosition() {
        movePosition = playerPosition - waypoint.transform.position;

        movePosition.x *= XmovePercentage;
        movePosition.y *= YmovePercentage;

        ClampFromBounds();
        waypoint.transform.position += movePosition;
    }

    void ClampHorizontal(GameObject bound) {
        if (math.distance(bound.transform.position.x, playerPosition.x) > horizontalDistanceFromBounds) { }
        else if (math.distance(bound.transform.position.x, waypoint.transform.position.x) < horizontalDistanceFromBounds) {
            movePosition.x = 0; // dont move right
        }
    }

    void ClampVertical(GameObject bound) {
        if (math.distance(bound.transform.position.y, playerPosition.y) > verticalDistanceFromBounds) { }
        else if (math.distance(bound.transform.position.y, waypoint.transform.position.y) < verticalDistanceFromBounds) {
            movePosition.y = 0; // dont move right
        }
    }


    // getters
    public Vector3 GetWaypointPosition() { return waypoint.transform.position; }

}

using Unity.Mathematics;
using UnityEngine;

public class CameraWaypointManager : MonoBehaviour {
    public static CameraWaypointManager instance;

    [Header("Bounds")]
    [SerializeField] GameObject rightBounds;
    [SerializeField] GameObject leftBounds;
    [SerializeField] GameObject upBounds;
    [SerializeField] GameObject downBounds;
    [SerializeField] float distanceFromBounds;

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
        WaypointMovement();
    }

    void WaypointMovement() {
        playerPosition = PlayerManager.instance.GetTransform().position;
        if (!PlayerManager.instance.GetMovementController().GetIsGrounded()) {
            // if player is too far, move
            if (math.distance(playerPosition.y, waypoint.transform.position.y) > distanceLimit) playerPosition.y += yOffset;
            if (math.distance(playerPosition.y, waypoint.transform.position.y) > distanceLimit * distanceLimit) playerPosition.y += yOffset * distanceLimit;
            else playerPosition.y = waypoint.transform.position.y;
        }
        else {
            playerPosition.y += yOffset;
        }

        //playerPosition.y += yOffset;
        movePosition = playerPosition - waypoint.transform.position;

        movePosition.x *= XmovePercentage;
        movePosition.y *= YmovePercentage;

        waypoint.transform.position += movePosition;
    }


    // getters
    public Vector3 GetWaypointPosition() { return waypoint.transform.position; }

}

using UnityEngine;

public class CameraWaypointManager : MonoBehaviour {
    public static CameraWaypointManager instance;

    [Header("Waypoints")]
    [SerializeField] GameObject waypoint;

    [Header("Tween")]
    [SerializeField, Range(0, 0.02f)] float movePercentage;
    [SerializeField, Range(0, 0.02f)] float moveTimeIncrements;

    Vector3 playerPosition;
    Vector3 movePosition;

    void Awake() {
        if (instance == null) instance = this;
    }

    void Update() {
        WaypointMovement();
    }

    void WaypointMovement() {
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;

        movePosition = (playerPosition - waypoint.transform.position) * movePercentage;
        waypoint.transform.position += movePosition;
    }

    // getters
    public Vector3 GetWaypointPosition() { return waypoint.transform.position; }

}

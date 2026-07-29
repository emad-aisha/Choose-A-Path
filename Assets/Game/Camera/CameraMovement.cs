using UnityEngine;


public class CameraMovement : MonoBehaviour {
    void Update() {
        Follow();
    }

    void Follow() {
        Vector3 waypointPosition = CameraWaypointManager.instance.GetWaypointPosition();

        transform.position = new Vector3(waypointPosition.x, waypointPosition.y, transform.position.z);
    }

}

using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] float maxSpeed;
    [SerializeField] float speed;
    float internalSpeed;
    [SerializeField] float z;

    Vector3 playerPosition;
    Vector3 lerpedPosition;


    void Start() {
        if (z <= 0) z = transform.position.z;
        internalSpeed = speed;
        lerpedPosition = PlayerManager.instance.GetPlayerTransform().position;
    }

    void Update() {
        playerPosition = PlayerManager.instance.GetPlayerTransform().position;
        if (math.round(transform.position.x) == math.round(playerPosition.x) && math.round(transform.position.y) == math.round(playerPosition.y)) {
            internalSpeed = speed;
        }
        else if (internalSpeed < maxSpeed) {
            internalSpeed += Time.deltaTime;
        }


        Lerp();
        transform.position = lerpedPosition;
    }

    void Lerp() {
        float x = lerpedPosition.x;
        float y = lerpedPosition.y;

        LerpPos(ref x, playerPosition.x, lerpedPosition.x);
        LerpPos(ref y, playerPosition.y, lerpedPosition.y);

        lerpedPosition = new Vector3(x, y, z);
    }

    void LerpPos(ref float xOrY, float playerXorY, float lerpedXorY) {
        float difference = internalSpeed * Time.deltaTime;

        if (xOrY < playerXorY && (lerpedXorY += difference) <= playerXorY) {
            xOrY = lerpedXorY += difference;
        }
        else if (xOrY > playerXorY && (lerpedXorY -= difference) >= playerXorY) {
            xOrY = lerpedXorY -= difference;
        }
    }

}

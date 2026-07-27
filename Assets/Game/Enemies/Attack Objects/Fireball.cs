using System;
using UnityEngine;

public class Fireball : MonoBehaviour {
    Vector3 originalPosition;
    Vector3 direction;
    float speed;

    Vector3 endDirection;

    void Start() {
        originalPosition = transform.position;
    }

    void Update() {
        Vector3 roundedPosition = new Vector3((float)Math.Round(transform.position.x, 2), (float)Math.Round(transform.position.y, 2), (float)Math.Round(transform.position.z, 2));
        if (endDirection == roundedPosition) Destroy(gameObject);

        speed += Time.deltaTime;
        Vector3 endPoint = (direction - originalPosition).normalized * (speed * Time.deltaTime);
        transform.position += endPoint;
    }

    public void ResetOriginalPosition() {
        originalPosition = transform.position;
    }
    public void SetDirection(Vector3 newDirection) {
        direction = newDirection;
    }
    public void SetEndDirection(Vector3 newDirection) {
        endDirection = new Vector3((float)Math.Round(newDirection.x, 2), (float)Math.Round(newDirection.y, 2), (float)Math.Round(newDirection.z, 2));
    }

    public void SetSpeed(float newSpeed) { speed = newSpeed; }
    public void SetDamage(int damage) { GetComponent<Hurtbox>().SetDamage(damage); }
}

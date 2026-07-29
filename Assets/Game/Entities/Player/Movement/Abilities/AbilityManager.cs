using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    public static AbilityManager instance;
    MovementController playerMovementController;
    [SerializeField] GameObject grapple;
    [SerializeField] GameObject top;

    bool hitSomething = false;
    Vector3 originalPosition = Vector3.zero;

    void Awake() {
        if (instance == null) instance = this;
        playerMovementController = GetComponent<MovementController>();
    }

    // HELPERS
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) {
            hitSomething = true;
        }
    }

    bool IsInRange(Vector3 endPoint, Vector3 pointToHit) {
        float radius = playerMovementController.GetComponent<CharacterController>().radius;

        if (endPoint.normalized.x != 0) {
            if (pointToHit.x - transform.position.x < 0 && transform.position.x < pointToHit.x - radius) {
                Debug.Log("X - too far to left");
                return false;
            }
            else if (pointToHit.x - transform.position.x > 0 && transform.position.x > pointToHit.x - radius) {
                Debug.Log("X - too far to right");
                return false;
            }
        }
        else if (endPoint.normalized.y != 0) {
            if (pointToHit.y - transform.position.y < 0 && transform.position.y < pointToHit.y) {
                Debug.Log("Y - too far to down");
                return false;
            }
            else if (pointToHit.y - transform.position.y > 0 && transform.position.y > pointToHit.y) {
                Debug.Log("Y - too far to up");
                return false;
            }
        }

        return true;
    }


    // PUBLIC 
    public void SetStartPoint() { originalPosition = transform.position; }
    public bool MoveToPoint(Vector3 pointToHit, float distance) {
        if (hitSomething) {
            originalPosition = Vector3.zero;
            hitSomething = false;
            return false;
        }
        if (originalPosition == Vector3.zero) {
            //Debug.Log("did you forget to set the original position?");
            return false;
        }

        Vector3 endPoint = (pointToHit - originalPosition) * distance;
        transform.position += endPoint;

        if (!IsInRange(endPoint, pointToHit)) originalPosition = Vector3.zero;
        return true;
    }


    public void SetMaxJumps(int newJumps) { playerMovementController.SetMaxJumps(newJumps); }

    public void SetGrappleAbility(GameObject _grapple) { grapple = _grapple; }
    public void SetTopAbility(GameObject _top) { top = _top; }

    public IEnumerator DisableAbilities(float time) {
        if (grapple && !grapple.activeSelf) yield break;
        if (top && !top.activeSelf) yield break;

        if (grapple) grapple.SetActive(false);
        if (top) top.SetActive(false);
        yield return new WaitForSeconds(time);
        if (grapple) grapple.SetActive(true);
        if (top) top.SetActive(true);
    }

}

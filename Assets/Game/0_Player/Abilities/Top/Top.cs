using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Top : Input {
    public enum Type { Dash, Wings };
    [SerializeField] Type type;

    [Header("Dash Stats")]
    [SerializeField] GameObject distanceVisualizer;
    [SerializeField] float cooldown;
    [SerializeField] float distance;
    [SerializeField, Range(0, 0.1f)] float distanceIncrement;
    bool isDashing;

    InputAction sprintAction;
    Vector3 pointToDash;
    Coroutine dash;

    void Start() {
        if (type == Type.Wings) PlayerManager.instance.SetMaxJumps(2);
        else {
            sprintAction = InputManager.instance.GetAction(actionName, "Sprint");
            isDashing = false;
        }
    }

    void Update() {
        if (sprintAction == null) return;
        distanceVisualizer.transform.position = pointToDash;

        if (!isDashing) {
            pointToDash = transform.position + (FacingDirectionManager.instance.GetFacingDirection() * distance);
            dash = StartCoroutine(Dash());
        }

        if (isDashing) {
            isDashing = PlayerManager.instance.MoveToPoint(pointToDash, distanceIncrement);
            if (!isDashing) {
                PlayerManager.instance.UnlockPlayerMovement();
                StopCoroutine(dash);
            }
        }

    }

    IEnumerator Dash() {
        if (!sprintAction.WasPressedThisFrame()) yield break;
        isDashing = true;
        PlayerManager.instance.LockPlayerMovement();
        yield return new WaitForSeconds(cooldown);
        PlayerManager.instance.UnlockPlayerMovement();
        isDashing = false;
    }



}

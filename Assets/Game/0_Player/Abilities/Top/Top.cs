using System.Collections;
using Unity.VisualScripting;
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
    bool canDash;

    InputAction sprintAction;
    Vector3 pointToDash;


    void Start() {
        if (type == Type.Wings) AbilityManager.instance.SetMaxJumps(2);
        else {
            sprintAction = InputManager.instance.GetAction(actionName, "Sprint");
            isDashing = false;
            canDash = true;
        }
    }

    void Update() {
        if (sprintAction == null) return;
        distanceVisualizer.transform.position = pointToDash;

        if (canDash && !isDashing) {
            pointToDash = transform.position + (FacingDirectionManager.instance.GetHorizontalDirection() * distance);
            StartCoroutine(Dash());
        }

        if (isDashing) {
            isDashing = AbilityManager.instance.MoveToPoint(pointToDash, distanceIncrement);
            if (!isDashing) {
                PlayerManager.instance.UnlockPlayerMovement();
            }
        }

    }

    IEnumerator Dash() {
        if (!sprintAction.WasPressedThisFrame()) yield break;
        AbilityManager.instance.SetStartPoint();
        PlayerManager.instance.LockPlayerMovement();
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(cooldown);
        PlayerManager.instance.UnlockPlayerMovement();
        isDashing = false;
        canDash = true;
    }

}

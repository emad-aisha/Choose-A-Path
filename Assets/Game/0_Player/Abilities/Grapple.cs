using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : Input {

    InputAction interactAction;
    [Header("Interaction")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float cooldown;
    float internalTimer;
    bool canInteract = true;

    [SerializeField] string shoeType = "Dash";

    [Header("Grapple Stats")]
    [SerializeField] float distance;
    [SerializeField] float time;

    bool isGrappling;

    void Start() {
        interactAction = InputManager.instance.GetAction(actionName, "Interact");
        internalTimer = cooldown;
    }

    void Update() {
        if (internalTimer >= cooldown) {
            canInteract = true;
        }
        else if (internalTimer < cooldown) {
            internalTimer += Time.deltaTime;
        }

        Debug.DrawRay(transform.position, FacingDirectionManager.instance.GetFacingDirection() * distance, Color.blue); // dash raycast
        Debug.DrawRay(transform.position, FacingDirectionManager.instance.GetUpwardsDirection() * distance, Color.yellow); // jump raycast
        StartCoroutine(Interact());

        if (isGrappling) {
            PlayerManager.instance.ResetJumpVelocity();
        }
    }

    IEnumerator Interact() {
        if (!interactAction.WasPressedThisFrame()) yield break; // if not interacted
        if (isGrappling || !canInteract) yield break; // if not allowed to interact
        isGrappling = true;

        PlayerManager.instance.LockPlayerMovement();
        RaycastHit hit;
        // if hit smth
        bool hitDash = Physics.Raycast(transform.position, FacingDirectionManager.instance.GetFacingDirection(), out hit, distance, ~playerLayer | ~enemyLayer);
        bool hitJump = Physics.Raycast(transform.position, FacingDirectionManager.instance.GetUpwardsDirection(), out hit, distance, ~playerLayer | ~enemyLayer);

        if (hitDash || hitJump) {
            Debug.Log("shoes Interact");
            //hit.GetComponent<Shoes>().Interact();
        }

        yield return new WaitForSeconds(time);
        PlayerManager.instance.UnlockPlayerMovement();
        isGrappling = false;
        canInteract = false;
    }

}

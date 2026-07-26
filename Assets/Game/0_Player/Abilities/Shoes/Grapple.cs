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

    [Header("Grapple Stats")]
    [SerializeField] float distance;
    [SerializeField] float time;

    Shoes shoes;
    bool isGrappling;

    void Start() {
        interactAction = InputManager.instance.GetAction(actionName, "Interact");
        internalTimer = cooldown;
        shoes = GetComponent<Shoes>();
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
        if (isGrappling || !canInteract || !shoes) yield break; // if not allowed to interact
        isGrappling = true;

        PlayerManager.instance.LockPlayerMovement();
        RaycastHit dashHit, jumpHit;

        // if hit smth
        bool canDash = Physics.Raycast(transform.position, FacingDirectionManager.instance.GetFacingDirection(), out dashHit, distance, ~(playerLayer | enemyLayer));
        bool canJump = Physics.Raycast(transform.position, FacingDirectionManager.instance.GetUpwardsDirection(), out jumpHit, distance, ~(playerLayer | enemyLayer));

        if (shoes.CompareShoeType(Shoes.Type.Dash) && canDash) {
            shoes.Interact(dashHit.point, time);
        }
        else if (shoes.CompareShoeType(Shoes.Type.Jump) && canJump) {
            shoes.Interact(jumpHit.point, time);
        }

        yield return new WaitForSeconds(time);
        PlayerManager.instance.UnlockPlayerMovement();
        isGrappling = false;
        canInteract = false;
    }

}

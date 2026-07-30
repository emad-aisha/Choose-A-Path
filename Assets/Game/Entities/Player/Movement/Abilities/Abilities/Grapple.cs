using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : Input {
    InputAction interactAction;
    [Header("Interaction")]
    [SerializeField] LayerMask ignoreLayer;
    bool canInteract = true;

    [Header("Grapple Stats")]
    [SerializeField] float distance;
    [SerializeField] float pause;
    [SerializeField] float time;

    Shoes shoes;
    bool isGrappling;

    void Start() {
        interactAction = InputManager.instance.GetAction(actionName, "Interact");
        shoes = GetComponent<Shoes>();
    }

    void Update() {
        Debug.DrawRay(transform.position, FacingDirectionManager.instance.GetHorizontalDirection() * distance, Color.blue); // dash raycast
        Debug.DrawRay(transform.position, FacingDirectionManager.instance.GetUpwardsDirection() * distance, Color.yellow); // jump raycast
        StartCoroutine(Interact());

        if (isGrappling) {
            PlayerManager.instance.ResetJumpVelocity();
        }

        if (PlayerManager.instance.IsPlayerGrounded()) canInteract = true;
    }

    IEnumerator Interact() {
        if (!interactAction.WasPressedThisFrame()) yield break; // if not interacted
        if (isGrappling || !canInteract || !shoes) yield break; // if not allowed to interact
        isGrappling = true;
        canInteract = false;

        PlayerManager.instance.LockPlayerMovement();
        RaycastHit dashHit, jumpHit;
        AnimationManager.instance.SetIsGrappling(true);

        // if hit smth
        bool canDash = Physics.Raycast(transform.position, FacingDirectionManager.instance.GetHorizontalDirection(), out dashHit, distance, ~ignoreLayer);
        bool canJump = Physics.Raycast(transform.position, FacingDirectionManager.instance.GetUpwardsDirection(), out jumpHit, distance, ~ignoreLayer);

        if (shoes.CompareShoeType(Shoes.Type.Jump)) {
            AnimationManager.instance.SetIsVertical(true);
            yield return new WaitForSeconds(pause);
            AnimationManager.instance.SetIsVertical(false);
        }
        else {
            AnimationManager.instance.SetIsHorizontal(true);
            yield return new WaitForSeconds(pause);
            AnimationManager.instance.SetIsHorizontal(false);
        }

        AnimationManager.instance.SetIsGrappling(false);
        if (shoes.CompareShoeType(Shoes.Type.Dash) && canDash) {
            shoes.Interact(dashHit.point, time);
        }
        else if (shoes.CompareShoeType(Shoes.Type.Jump) && canJump) {
            shoes.Interact(jumpHit.point, time);
        }

        yield return new WaitForSeconds(time);
        PlayerManager.instance.UnlockPlayerMovement();
        isGrappling = false;
    }

}

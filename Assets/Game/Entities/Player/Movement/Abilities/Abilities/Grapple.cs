using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System;
using UnityEditor.Experimental.GraphView;

public class Grapple : Input {
    InputAction interactAction;
    [Header("Interaction")]
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] RectTransform grappleSprite;
    [SerializeField] RectTransform grappleCursor;
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
        // update grapple start pos

        Debug.DrawRay(transform.position, FacingDirectionManager.instance.GetHorizontalDirection() * distance, Color.blue); // dash raycast
        Debug.DrawRay(transform.position, FacingDirectionManager.instance.GetUpwardsDirection() * distance, Color.yellow); // jump raycast
        StartCoroutine(Interact());

        if (isGrappling) {
            PlayerManager.instance.ResetJumpVelocity();
        }

        if (PlayerManager.instance.GetMovementController().GetIsGrounded()) canInteract = true;
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
            StartCoroutine(VerticalGrappleAnimation(jumpHit.point));
            yield return new WaitForSeconds(pause);
            AnimationManager.instance.SetIsVertical(false);
        }
        else {
            AnimationManager.instance.SetIsHorizontal(true);
            StartCoroutine(HorizontalGrappleAnimation(dashHit.point));
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

    IEnumerator HorizontalGrappleAnimation(Vector3 grapplePoint) {
        bool hitWall = false;
        int safety = 0;
        int xDirection = (int)FacingDirectionManager.instance.GetHorizontalDirection().x;
        Vector3 size;
        if (grapplePoint.x == 0) grapplePoint.x = (distance * xDirection) + PlayerManager.instance.GetTransform().position.x;

        while (!hitWall && safety < 90) {
            size = grapplePoint - PlayerManager.instance.GetTransform().position;
            XCheck(size, grapplePoint, xDirection, ref hitWall);

            if (hitWall) { grappleSprite.sizeDelta = Vector2.zero; }
            safety++;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        grappleSprite.sizeDelta = Vector2.zero;
    }

    IEnumerator VerticalGrappleAnimation(Vector3 grapplePoint) {
        bool hitWall = false;
        int safety = 0;
        Vector3 size;
        if (grapplePoint.y == 0) grapplePoint.y = distance;

        while (!hitWall && safety < 90) {
            size = grapplePoint - PlayerManager.instance.GetTransform().position;
            YCheck(size, grapplePoint, ref hitWall);

            if (hitWall) { grappleSprite.sizeDelta = Vector2.zero; }
            safety++;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        grappleSprite.sizeDelta = Vector2.zero;
    }


    void XCheck(Vector3 size, Vector3 grapplePoint, int direction, ref bool hitWall) {
        float xDistance = math.distance(grapplePoint.x, PlayerManager.instance.GetTransform().position.x);
        grappleSprite.sizeDelta = new Vector2(math.abs(size.x), 0.3f);

        grappleSprite.position = new Vector2((direction * xDistance / 2) + PlayerManager.instance.GetTransform().position.x, grappleSprite.position.y);

        // -0.3 - 0.3  means I hit the wall
        if (math.abs(size).x < 0.3) hitWall = true;
    }

    void YCheck(Vector3 size, Vector3 grapplePoint, ref bool hitWall) {
        float yDistance = math.distance(grapplePoint.y, PlayerManager.instance.GetTransform().position.y);
        grappleSprite.sizeDelta = new Vector2(0.3f, math.abs(size.y));

        grappleSprite.position = new Vector2(grappleSprite.position.x, (yDistance / 2) + PlayerManager.instance.GetTransform().position.y);

        // 0.6  means I hit the wall
        if (math.abs(size).y < 0.6) hitWall = true;
    }

}

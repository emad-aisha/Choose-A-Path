using System.Collections;
using UnityEngine;

public class Shoes : MonoBehaviour {
    public enum Type { Dash, Jump };

    [Header("Powerup Stats")]
    [SerializeField] Type type;
    [SerializeField, Range(0, 0.1f)] float distanceIncrement;

    bool interacted = false;
    Vector3 pointToHit;

    void Update() {
        if (!interacted) {
            AnimationManager.instance.SetHitTarget(false);
            return;
        }
        UseAbility();
    }

    public void Interact(Vector3 _pointToHit, float time) {
        pointToHit = _pointToHit;
        interacted = true;
        AbilityManager.instance.SetStartPoint();
    }

    void UseAbility() {
        interacted = AbilityManager.instance.MoveToPoint(pointToHit, distanceIncrement);
        if (!interacted) {
            AnimationManager.instance.SetHitTarget(true);
            AnimationManager.instance.SetIsGrappling(false);
            interacted = false;
            pointToHit = Vector3.zero;
        }
    }

    IEnumerator Ability(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        interacted = false;
        pointToHit = Vector3.zero;
    }

    public bool CompareShoeType(Type compare) { return type == compare; }
}

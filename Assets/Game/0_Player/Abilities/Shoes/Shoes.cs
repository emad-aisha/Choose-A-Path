using System.Collections;
using UnityEngine;

public class Shoes : MonoBehaviour {
    public enum Type { Dash, Jump };

    [Header("Powerup Stats")]
    [SerializeField] Type shoeType;

    bool interacted = false;
    Vector3 pointToHit;
    float time;

    void Update() {
        if (!interacted) return;
        UseAbility();
    }

    public void Interact(Vector3 _pointToHit, float _time) {
        pointToHit = _pointToHit;
        time = _time;
        interacted = true;
        StartCoroutine(Ability(time));
    }

    void UseAbility() {
        interacted = PlayerManager.instance.MoveToPoint(pointToHit, time);
        if (!interacted) {
            interacted = false;
            pointToHit = Vector3.zero;
            time = 0;
        }
    }

    IEnumerator Ability(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        interacted = false;
        pointToHit = Vector3.zero;
        time = 0;
    }

    public bool CompareShoeType(Type compare) { return shoeType == compare; }
}

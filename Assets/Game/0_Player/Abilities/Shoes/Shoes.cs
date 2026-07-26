using System.Collections;
using UnityEngine;

public class Shoes : MonoBehaviour {
    public enum Type { Dash, Jump };

    [Header("Powerup Stats")]
    [SerializeField] Type shoeType;

    bool interacted = false;
    Vector3 pointToHit;
    float time;

    void Start() {

    }

    void Update() {
        if (!interacted) return;

        switch (shoeType) {
            case Type.Dash: Dash(); break;
            case Type.Jump: Jump(); break;
        }
    }

    public void Interact(Vector3 _pointToHit, float _time) {
        pointToHit = _pointToHit;
        time = _time;
        interacted = true;
    }

    void Dash() {
        PlayerManager.instance.Dash(pointToHit, time);
        StartCoroutine(Ability(time));
    }

    void Jump() {
        //StartCoroutine(PlayerManager.instance.TemporarilyBoostJumpMod(boost, time));
    }

    IEnumerator Ability(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        interacted = false;
        pointToHit = Vector3.zero;
        time = 0;
    }

    public bool CompareShoeType(Type compare) { return shoeType == compare; }
}

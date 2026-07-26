using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    enum Type { Float, Grounded };
    [SerializeField] Type type;
    [SerializeField] float hoverDistance;

    BasicAttack attack;

    void Start() {

    }

    void Update() {
        // slowly float towards player
        // or
        // walks towards player
    }
}

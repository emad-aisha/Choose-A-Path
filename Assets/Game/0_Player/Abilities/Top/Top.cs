using UnityEngine;

public class Top : MonoBehaviour {
    void Start() {
        PlayerManager.instance.SetMaxJumps(2);
    }

}

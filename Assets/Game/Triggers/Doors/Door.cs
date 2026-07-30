using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField] bool isOpen;

    void Update() {
        gameObject.GetComponent<BoxCollider>().isTrigger = isOpen;
        gameObject.GetComponent<MeshRenderer>().enabled = !isOpen;

        if (isOpen) {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }


    public void SetOpen(bool boolValue) { isOpen = boolValue; }
}

using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField] bool isOpen;

    void Update() {
        gameObject.GetComponent<BoxCollider>().isTrigger = isOpen;
        gameObject.GetComponent<MeshRenderer>().enabled = !isOpen;
    }


    public void SetOpen(bool boolValue) { isOpen = boolValue; }
}

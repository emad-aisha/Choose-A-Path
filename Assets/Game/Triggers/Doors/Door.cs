using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField] bool isOpen;

    void Update() {
        gameObject.GetComponent<BoxCollider>().isTrigger = isOpen;
    }


    public void SetOpen(bool boolValue) { isOpen = boolValue; }
}

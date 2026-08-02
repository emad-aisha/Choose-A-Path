using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour {
    [SerializeField] string sceneName;


    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Player Parent")) {
            SceneManager.LoadScene(sceneName);
        }
    }

}

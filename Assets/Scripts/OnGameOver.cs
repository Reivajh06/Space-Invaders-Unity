using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGameOver : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

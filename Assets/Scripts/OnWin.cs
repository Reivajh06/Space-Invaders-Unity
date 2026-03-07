using System;
using TMPro;
using UnityEngine;

public class OnWin : MonoBehaviour {

    public AlienSpawner alienSpawner;
    public TextMeshProUGUI winText;
    public float textPopUpCooldown = 0.5f;

    private void Start() {
        Time.timeScale = 0;
    }

    void Update() {
        if (textPopUpCooldown <= 0) {
            winText.gameObject.SetActive(!winText.gameObject.activeInHierarchy);
            textPopUpCooldown = 0.5f;
        }

        textPopUpCooldown -= 1 * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Return)) {
            Time.timeScale = 1;
            alienSpawner.SetUp();
            gameObject.SetActive(false);
        }
    }
}

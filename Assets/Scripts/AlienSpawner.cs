using System;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpawner : MonoBehaviour {

    public AudioManager audioManager;
    public int alienRows = 5;
    public int aliensInRow;
    public List<Alien> aliens;

    public float moveCooldown = 2f;
    public int direction = 1;
    public float startX = -15f;
    public float startY = 3f;
    private float paddingX = 0.2f;
    private float paddingY = 1f;

    public int remainingAliens;
    public Color[] rowColors;


    public GameObject[] alienPrefabs;
    
    void Start() {
        InstantiateAlienRows();

        remainingAliens = alienRows * aliensInRow;
    }

    void Update() {
        if (moveCooldown <= 0)
        {
            MoveAliens();
            moveCooldown = 3f;
        }

        moveCooldown -= 1 * Time.deltaTime;
    }

    private void InstantiateAlienRows() {
        for(int i = 0; i < alienRows; i++) {
            GameObject alienPrefab = alienPrefabs[Math.Min(i, alienPrefabs.Length - 1)];
            SpriteRenderer sr = alienPrefab.GetComponent<SpriteRenderer>();
            float xOffset = sr.bounds.size.x + paddingX;
            float yOffset = sr.bounds.size.y + paddingY;
            
            for(int j = 0; j < aliensInRow; j++) {
                Alien alien = Instantiate(
                    alienPrefab,
                    new Vector3(startX + xOffset * j, startY + yOffset * i),
                    Quaternion.identity,
                    transform
                ).GetComponent<Alien>();

                alien.name = "Alien" + "(" + i + ", " + j + ")";
                alien.Row = i;
                alien.Column = j;
                alien.audioManager = audioManager;

                if (rowColors.Length != 0) alien.GetComponent<SpriteRenderer>().color = rowColors[i % rowColors.Length];

                aliens.Add(alien);
            }
        }
    }

    public Alien GetAlien(int row, int column) {
        Alien[] aliens = GetComponentsInChildren<Alien>();
        
        for(int i = 0; i < aliens.Length; i++) {
            if (aliens[i].Row == row && aliens[i].Column == column) return aliens[i];
        }
        
        return null;
    }

    public void MoveAliens(bool horizontally=true) {
        foreach(Alien a in aliens) {
            if (horizontally) {
                a.transform.Translate(new Vector3(2 * direction, 0));
                Alien.enableBordersCollision = true;

            } else {

                direction *= -1;
                MoveAliens(); //Undo the last movement if any alien collides with bounds (this part is only called vertically when some alien collides with bounds)
                a.transform.Translate(new Vector3(0, alienPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.y * -1));
                Alien.enableBordersCollision = false;
            }
        }
    }
}

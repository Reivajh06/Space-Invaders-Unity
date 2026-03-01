using System;
using UnityEngine;

public class AlienSpawner : MonoBehaviour {
    
    public int alienRows = 5;
    public int aliensInRow;
    public float startX = -15f;
    public float startY = 2.5f;
    public int remainingAliens;
    public Color[] rowColors;

    private float paddingX = 0.1f;
    private float paddingY = 0.1f;
    
    public GameObject[] alienPrefabs;
    
    void Start() {
        InstantiateAlienRows();

        remainingAliens = alienRows * aliensInRow;
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

                if (rowColors.Length != 0) alien.GetComponent<SpriteRenderer>().color = rowColors[i % rowColors.Length];
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
}

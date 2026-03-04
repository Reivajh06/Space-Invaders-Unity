using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour
{

    public GameObject borders;
    public AudioManager audioManager;
    public int alienRows = 5;
    public int aliensInRow;
    public Alien[] aliens;

    public float spaceShipCooldown = 10f;
    public float moveCooldown = 2f;
    public int direction = 1;
    public float startX = -15f;
    public float startY = 3f;
    private float paddingX = 0.2f;
    private float paddingY = 1f;

    public int remainingAliens;
    public Color[] rowColors;


    public GameObject[] alienPrefabs;
    public GameObject spaceShipPrefab;
    
    void Start() {
        aliens = new Alien[aliensInRow * alienRows];
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

        if (GetComponentInChildren<AlienSpaceShip>() == null) {
            spaceShipCooldown =  Random.Range(9, 16);
        }
        
        if (spaceShipCooldown <= 0) {
            SpawnSpaceShip();
        }
        
        audioManager = GetComponent<AudioManager>();
        spaceShipCooldown -= 1 * Time.deltaTime;
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

                if (rowColors.Length != 0)
                {
                    alien.color = rowColors[i % rowColors.Length];
                    alien.GetComponent<SpriteRenderer>().color = rowColors[i % rowColors.Length];
                }

                aliens[aliensInRow * i + j] = alien;
            }
        }
    }

    public Alien GetAlien(int row, int column) {
        return aliens[aliensInRow * row + column];
    }

    public void Remove(Alien a) {
        aliens[a.Row * aliensInRow + a.Column] = null;
    }

    public void MoveAliens(bool horizontally=true) {
        if (!horizontally) direction *= -1;
        
        foreach(Alien a in aliens) {
            if (a == null) continue;
            
            if (horizontally) {
                a.transform.Translate(new Vector3(2 * direction, 0));
                Alien.enableBordersCollision = true;

            } else {
                a.transform.Translate(new Vector3(2 * direction, 0)); //Undo the last movement if any alien collides with bounds (this part is only called vertically when some alien collides with bounds)
                a.transform.Translate(new Vector3(0, alienPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.y * -1));
                Alien.enableBordersCollision = false;
            }
        }
    }

    private void SpawnSpaceShip() {
        spaceShipCooldown = Random.Range(9, 16);

        Collider2D boundsCollider = borders.GetComponent<Collider2D>();
        Bounds b = boundsCollider.bounds;

        // Get ship size for clean edge placement
        SpriteRenderer shipSr = spaceShipPrefab.GetComponent<SpriteRenderer>();
        float halfW = shipSr ? shipSr.bounds.extents.x : 0f;
        float halfH = shipSr ? shipSr.bounds.extents.y : 0f;

        // If moving left, spawn on the RIGHT side. If moving right, spawn on the LEFT side.
        float spawnX = (AlienSpaceShip.direction < 0) ? (b.max.x + halfW) -3 : (b.min.x - halfW) + 3;

        // Put it visually on the top edge (inside). If you want above the border, use b.max.y + halfH instead.
        float spawnY = b.max.y - halfH - 2;

        GameObject go = Instantiate(spaceShipPrefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity, transform);
    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour {

    [Header("GameObjects References")] 
    public AudioManager audioManager;
    public GameObject borders;
    public Alien[] aliens;
    public GameObject winUI;

    [Header("Alien Spawning Configuration")]
    public int alienRows = 5;
    public int aliensInRow;
    public float spaceShipCooldown = 10f;
    public float moveCooldown = 2f;
    public int direction = 1;
    public float startX = -15f;
    public float startY = 3f;
    private float _paddingX = 0.2f;
    private float _paddingY = 1f;
    private float _timeMultiplier = 1;
    
    public int remainingAliens;
    public Color[] rowColors;
    public GameObject[] alienPrefabs;
    public GameObject spaceShipPrefab;
    
    void Start() {
        SetUp();
    }

    void Update() {
        if (remainingAliens == 0) {
            winUI.SetActive(true);
        }
        
        UpdateAlienSpeedup();
        
        if (moveCooldown <= 0) {
            MoveAliens();
            moveCooldown = 3f;
        }

        moveCooldown -= _timeMultiplier * Time.deltaTime;

        if (GetComponentInChildren<AlienSpaceShip>() == null) {
            if (spaceShipCooldown <= 0)
            {
                SpawnSpaceShip();
                spaceShipCooldown = Random.Range(9, 16);
                
            } else {
                spaceShipCooldown -= 1 * Time.deltaTime;
            }
        }
    }

    public void SetUp() {
        aliens = new Alien[aliensInRow * alienRows];
        InstantiateAlienRows();

        remainingAliens = alienRows * aliensInRow;
    }

    private void InstantiateAlienRows() {
        for(int i = 0; i < alienRows; i++) {
            GameObject alienPrefab = alienPrefabs[Math.Min(i, alienPrefabs.Length - 1)];
            SpriteRenderer sr = alienPrefab.GetComponent<SpriteRenderer>();
            float xOffset = sr.bounds.size.x + _paddingX;
            float yOffset = sr.bounds.size.y + _paddingY;
            
            for(int j = 0; j < aliensInRow; j++) {
                SpawnAlien(alienPrefab, i, j, xOffset, yOffset);
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

    private void SpawnAlien(GameObject alienPrefab, int row, int col, float xOffset, float yOffset) {
        Alien alien = Instantiate(
            alienPrefab,
            new Vector3(startX + xOffset * col, startY + yOffset * row),
            Quaternion.identity,
            transform
        ).GetComponent<Alien>();

        alien.name = "Alien" + "(" + row + ", " + col + ")";
        alien.Row = row;
        alien.Column = col;

        if (rowColors.Length != 0)
        {
            alien.color = rowColors[row % rowColors.Length];
            alien.GetComponent<SpriteRenderer>().color = rowColors[row % rowColors.Length];
        }

        aliens[aliensInRow * row + col] = alien;
    }

    private void SpawnSpaceShip() {
        spaceShipCooldown = Random.Range(20, 30);

        Collider2D boundsCollider = borders.GetComponent<Collider2D>();
        Bounds b = boundsCollider.bounds;

        // Get ship size for clean edge placement
        SpriteRenderer shipSr = spaceShipPrefab.GetComponent<SpriteRenderer>();
        float halfW = shipSr ? shipSr.bounds.extents.x : 0f;
        float halfH = shipSr ? shipSr.bounds.extents.y : 0f;

        // If moving left, spawn on the RIGHT side. If moving right, spawn on the LEFT side.
        float spawnX = (AlienSpaceShip.direction < 0) ? (b.max.x + halfW) -4 : (b.min.x - halfW) + 4;

        // Put it visually on the top edge (inside). If you want above the border, use b.max.y + halfH instead.
        float spawnY = b.max.y - halfH - 2;

        AlienSpaceShip spaceShip = Instantiate(spaceShipPrefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity, transform).GetComponent<AlienSpaceShip>();
    }

    private void UpdateAlienSpeedup() {
        /*
         * -> [100%, 70%] timeMultiplier = 1
         * -> [70%, 50%] timeMultiplier = 2
         * -> [50% -> 30%] timeMultiplier = 3
         * -> [30%, 0%] timeMultiplier = 4
         */
        int totalAliens = alienRows * aliensInRow;
        int remainingPercentage = remainingAliens * 100 / totalAliens;
        
        if (remainingPercentage > 70) return;

        if(remainingPercentage > 50) {
            _timeMultiplier = 1.5f;
            
        } else if(remainingPercentage > 30) {
            _timeMultiplier = 2f;
            
        } else {
            _timeMultiplier = 2.5f;
        }

        foreach(Alien a in aliens) {
            if (a == null) continue;
            
            a.SetAnimationSpeed(_timeMultiplier);
        }

        audioManager.SetMusicPitch(_timeMultiplier - 0.5f);
    }
}
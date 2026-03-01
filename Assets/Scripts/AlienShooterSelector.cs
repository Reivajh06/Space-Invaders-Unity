using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienShooterSelector : MonoBehaviour {

    private AlienSpawner alienSpawner;
    private int maxShooters = 6;
    private float delayPerShot = 1f;
    private List<Alien> shooterAliens = new();
    private float shotCooldown = 6f;

    private void Awake() {
        alienSpawner = transform.parent.GetComponentInChildren<AlienSpawner>(true);
    }

    void Update() {
        if (shotCooldown <= 0) {
            shotCooldown = Random.Range(6, 14);
            Fill();
            Fire();
        }

        shotCooldown -= 1 * Time.deltaTime;
    }

    private void Fill() {
        int lowerRow = alienSpawner.alienRows - 1;
        int shooterAliensLength = Random.Range(1, maxShooters + 1 > alienSpawner.remainingAliens ? alienSpawner.remainingAliens : maxShooters + 1);
        
        while(shooterAliens.Count < shooterAliensLength) {
            Alien alien = alienSpawner.GetAlien(
                lowerRow,
                Random.Range(
                    0,
                    alienSpawner.aliensInRow
                ));
            
            if (!alien) continue;
            
            if(shooterAliens.Contains(alien)) continue;
            shooterAliens.Add(alien);
        }
    }

    private void Fire() {
        foreach (Alien a in shooterAliens) {
            while(delayPerShot > 0) {
                delayPerShot -= 1 * Time.deltaTime;
            }
            
            a.Fire();
            delayPerShot = Random.Range(1, 6);
        }

        shooterAliens.Clear();
    }
}

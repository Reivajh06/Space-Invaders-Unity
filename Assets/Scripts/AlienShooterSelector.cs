using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            SelectShooters();
            Fire();
        }

        shotCooldown -= 1 * Time.deltaTime;
    }

    private void SelectShooters() {
        int lowerRow = alienSpawner.alienRows - 1;
        int shooterAliensLength = Random.Range(1, maxShooters + 1 > alienSpawner.remainingAliens ? alienSpawner.remainingAliens : maxShooters + 1);

        for (int column = 0; column < shooterAliensLength; column++) {
            for(int row = alienSpawner.alienRows - 1; row >= 0; row--) {
                if (shooterAliens.Count + 1 >= shooterAliensLength) return;
                
                Alien a = alienSpawner.GetAlien(row, column);

                if (!a || !IsValid(a)) continue;
            
                shooterAliens.Add(a);
            }   
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

    private bool IsValid(Alien a) {
        for (int i = a.Row - 1; i >= 0; i--) {
            if (alienSpawner.GetAlien(i, a.Column)) return false;
        }

        return true;
    }
}

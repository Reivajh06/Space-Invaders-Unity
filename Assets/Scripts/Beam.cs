using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Beam : MonoBehaviour {
    
    public float speed = 8f;
    public GameObject shooter;

    void Update() {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;

        if(!other.name.Equals("Bounds")) {
            if (other.name.Contains("Alien")) {
                if (shooter.name.Contains("Alien")) return;
                
                if (other.name.Contains("AlienSpaceShip")) {
                    AlienSpaceShip alienSpaceShip = other.GetComponent<AlienSpaceShip>();
                    alienSpaceShip.OnHit();
                    shooter.GetComponent<Player>().UpdateScore(alienSpaceShip.score);
                    Destroy(gameObject);

                } else {
                    Alien a = other.GetComponent<Alien>();
                    a.OnHit();
                    a.GetComponentInParent<AlienSpawner>().Remove(a);
                    shooter.GetComponent<Player>().UpdateScore(a.score);
                    Destroy(gameObject);
                }
                
            } else if (other.name.Equals("Player")) {
                if (shooter.name.Equals("Player")) return;
                
                Player p = other.GetComponent<Player>();

                if (p.isAnimatingHit) return;
                
                p.OnHit();
                Destroy(gameObject);
            }
        }
    }
}

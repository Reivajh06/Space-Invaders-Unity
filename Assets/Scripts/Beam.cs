using UnityEngine;

public class Beam : MonoBehaviour {
    
    public float speed = 8f;
    public string shooterName;

    void Update() {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;

        if(!other.name.Equals("Bounds")) {
            if (other.name.Contains("Alien"))
            {
                if (shooterName.Equals("Alien")) return;
                Alien a = other.GetComponent<Alien>();
                a.OnHit();
                a.GetComponentInParent<AlienSpawner>().aliens.Remove(a);
                
            } if (other.name.Equals("Player")) {
                if (shooterName.Equals("Player")) return;
                
                Player p = other.GetComponent<Player>();
                p.OnHit();
            } 
        }
        
        Destroy(gameObject);
    }
}

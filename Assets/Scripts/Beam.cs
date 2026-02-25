using UnityEngine;

public class Beam : MonoBehaviour {

    public float speed = 5f;

    void Update() {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject other = collision.gameObject;

        if(!other.name.Equals("Borders")) {
            //other.setAnimation("Destroyed")?;
            Destroy(other);
        }
        
        Destroy(gameObject);
    }
}

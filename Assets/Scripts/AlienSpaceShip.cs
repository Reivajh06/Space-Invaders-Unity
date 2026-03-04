using System;
using UnityEngine;

public class AlienSpaceShip : MonoBehaviour {
    
    public static int direction = -1;
    
    public AudioManager audioManager;
    public AudioClip spaceShipHit;

    private Animator animator;
    private float speed = 4f;
    public int score = 100;

    void Start() {
        animator = GetComponent<Animator>();
    }
    void Update() {
        transform.Translate(
            direction < 0 ? Vector3.left * speed * Time.deltaTime : Vector3.right * speed * Time.deltaTime
            );
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (!collision.gameObject.name.Equals("Bounds")) {
            if (collision.gameObject.name.Equals("Player")) {
                Player p =  collision.gameObject.GetComponent<Player>();
                // p.score += score;
            } else {
               if(audioManager) audioManager.PlaySFX(spaceShipHit);

               animator.Play("spaceShip_hit");
            }
        };

        direction *= -1;
        Destroy(gameObject);
    }
}

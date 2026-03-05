using UnityEngine;
using Random = UnityEngine.Random;

public class Alien : MonoBehaviour {

    public static bool enableBordersCollision = true;
    public int score;
    
    public GameObject beamPrefab;
    private Animator animator;
    private int beamSpeed;
    
    public AudioManager audioManager;
    public AudioClip alienHit;
    public int Row { get; set; }
    public int Column { get; set; }
    public Color color;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Fire() {
        beamSpeed = Random.Range(-6, -1);
        Beam beam = Instantiate(beamPrefab, transform.position - transform.up * 1.2f, transform.rotation).GetComponent<Beam>();
        beam.shooterName = nameof(Alien);
        beam.speed = beamSpeed;
        
        if(color != null) beam.GetComponent<SpriteRenderer>().color = color;
    }

    public void OnHit() {
        if(audioManager) audioManager.PlaySFX(alienHit);
        
        animator.Play("alien_hit");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name.Equals("Bounds")) {
            if (enableBordersCollision) GetComponentInParent<AlienSpawner>().MoveAliens(false);
        }
    }
}


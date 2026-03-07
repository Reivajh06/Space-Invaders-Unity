using UnityEngine;
using Random = UnityEngine.Random;

public class Alien : MonoBehaviour {

    public static bool enableBordersCollision = true;
    public int score;
    
    public GameObject beamPrefab;
    private Animator animator;
    private int beamSpeed;
    
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
        beam.shooter = gameObject;
        beam.speed = beamSpeed;
        
        if(color != null) beam.GetComponent<SpriteRenderer>().color = color;
    }

    public void OnHit() {
        AudioManager.Instance.PlaySFX(alienHit);

        GetComponentInParent<AlienSpawner>().remainingAliens--;
        
        animator.Play("alien_hit");
    }

    public void SetAnimationSpeed(float speed) {
        animator.speed = speed;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name.Equals("Bounds")) {
            if (enableBordersCollision) GetComponentInParent<AlienSpawner>().MoveAliens(false);
        }
    }
}


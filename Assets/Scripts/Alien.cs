using UnityEngine;
using Random = UnityEngine.Random;

public class Alien : MonoBehaviour {

    public GameObject beamPrefab;
    private Animator animator;
    private int beamSpeed;
    public int Row { get; set; }
    public int Column { get; set; }

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Fire() {
        beamSpeed = Random.Range(-4, -1);
        Beam beam = Instantiate(beamPrefab, transform.position - transform.up * 1.2f, transform.rotation).GetComponent<Beam>();
        beam.shooterName = nameof(Alien);
        beam.speed = beamSpeed;
    }

    public void OnHit() {
        animator.Play("alien_hit");
    }
}


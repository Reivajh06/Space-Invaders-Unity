using System.Text;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour {
    
    public const float FIRECOOLDOWN = 0.8f;
    
    public Animator animator;

    public GameObject gameOverUI;
    public TextMeshProUGUI tmp;
    public GameObject beamPrefab;
    public AudioClip playerHit;
    public AudioClip beamShot;
    
    public GameObject turretSprite1;
    public GameObject turretSprite2;

    public bool isAnimatingHit = false;

    public int lives = 3;
    public float speed = 5f;
    public int score = 0;

    private float fireBeamTimer;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        float input = Input.GetAxis("Horizontal");
        
        transform.Translate(input * speed * Time.deltaTime, 0, 0);

        if(Input.GetKeyDown(KeyCode.Return)) {
            if (fireBeamTimer <= 0) {
                AudioManager.Instance.PlaySFX(beamShot); 
                    
                fireBeamTimer = FIRECOOLDOWN;
                Beam b = Instantiate(
                    beamPrefab,
                    transform.position + transform.up * 1.5f + transform.right / 2 + new Vector3(-0.25f, 0, 0),
                    Quaternion.identity
                    ).GetComponent<Beam>();
                b.shooter = gameObject;
            }
        }

        if(fireBeamTimer > 0) fireBeamTimer -= 1 * Time.deltaTime;
    }

    public void OnHit() {
        AudioManager.Instance.PlaySFX(playerHit);
        isAnimatingHit = true;
        animator.Play("player_hit");
        AudioManager.Instance.SetMusicPitch(0);
        Time.timeScale = 0;
    }

    public void RemoveSprite() {
        if(lives == 2) Destroy(turretSprite2);
        
        else if(lives == 1) Destroy(turretSprite1);
    }

    public void UpdateScore(int points) {
        score += points;

        tmp.SetText(new StringBuilder().Append(score).ToString());
    }
}

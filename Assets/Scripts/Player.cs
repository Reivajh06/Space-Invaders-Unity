using UnityEngine;

public class Player : MonoBehaviour {
    
    public const float FIRECOOLDOWN = 1f;

    public GameObject beamPrefab;
    public float speed = 5f;

    private float fireBeamTimer;

    void Update() {
        float input = Input.GetAxis("Horizontal");
        
        transform.Translate(input * speed * Time.deltaTime, 0, 0);

        if(Input.GetKeyDown(KeyCode.Return)) {
            if (fireBeamTimer <= 0) {
                fireBeamTimer = FIRECOOLDOWN;
                Instantiate(beamPrefab, transform.position + transform.up * 1.2f, transform.rotation);
            }
        }

        if(fireBeamTimer > 0) fireBeamTimer -= 1 * Time.deltaTime;
    }
}

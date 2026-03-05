using UnityEngine;

public class OnHit : StateMachineBehaviour {
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.GetComponentInParent<Alien>()) {
            Destroy(animator.GetComponentInParent<Alien>().gameObject);
            
        } else if (animator.GetComponentInParent<AlienSpaceShip>()) {
            Destroy(animator.GetComponentInParent<AlienSpaceShip>().gameObject);
            
        } else if (animator.GetComponentInParent<Player>()) {
            Player p = animator.GetComponent<Player>();

            if (p.lives - 1 == 0) {
                //Set Canvas to lose match??
                Destroy(animator.GetComponentInParent<Player>().gameObject);
                
            } else {
                p.lives--;
            }
        }
        
    }
}

using UnityEngine;

public class OnHit : StateMachineBehaviour {
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Destroy(animator.GetComponentInParent<Alien>().gameObject);
    }
}

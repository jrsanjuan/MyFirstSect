using UnityEngine;
using System.Collections;

public class PlayerAnimationsController : MonoBehaviour {
    Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
	}

    public void Walk() {
        animator.SetBool("Moving", true);
    }

    public void Idle() {
        animator.SetBool("Moving", false);
    }
    
    public void RiseArms () {
        animator.Play("RaiseArms");
    }
    
    public void DownArms () {
        animator.Play("DownArms");
    }

    public void ÍnteractWithObject()
    {
        animator.SetTrigger("Interact");
        animator.SetBool("Moving", false);
    }
}

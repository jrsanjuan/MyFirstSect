using UnityEngine;
using System.Collections;

public class MonkHandsUp : MonoBehaviour {
    [SerializeField]
    private Animator monkAnimator;
    
    public void HandsUp () {
		FMODManager.SINGLETON.PlaySound (FMODManager.Sounds.HandsOn);
        monkAnimator.Play("MonkRiseArms");
    }
    
    public void HandsDown () {
		FMODManager.SINGLETON.StopSound (FMODManager.Sounds.HandsOn);
        monkAnimator.Play("MonkDownArms");
        //StartCoroutine(WaitForIdle());
    }
    
    private IEnumerator WaitForIdle () {
        yield return new WaitForSeconds(0.25f);
        monkAnimator.Play("MonkIdle");
    }
}

using UnityEngine;
using System.Collections;

public class PickableObject : ClickableObject{
    
    #region FIELDS
    
    /// <summary>
    /// Sonido ejecutado cuando se coge el objeto
    /// </summary>
    public FMODManager.Sounds interactSound = FMODManager.Sounds.NONE;
    
    #endregion
    
    
    /// <summary>
    /// On click pressed method
    /// </summary>
    public override void OnClickPressed()
    {
        Debug.Log("PICKABLE: Click izquierdo");
        
        FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.PickObject);
        FMODManager.SINGLETON.PlayOneShot(interactSound);
        
		GameManager.SINGLETON.Player.realCarriedObject = gameObject;
		gameObject.SetActive (false);
    }

    /// <summary>
    /// On right click pressed method
    /// </summary>
    public override void OnReleasePressed()
    {
        Debug.Log("PICKABLE: Click derecho");
        
        FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.LeaveObject);
        
		GameManager.SINGLETON.Player.realCarriedObject = null;
		gameObject.SetActive (true);
    }
}


using UnityEngine;
using System.Collections;

public class InteractableObject : ClickableObject {
    
    #region FIELDS
    bool inConflict;
    MonkRequest monkRequest;
    
    /// <summary>
    /// Sonido instantaneo al inicio del conflicto
    /// </summary>
    public FMODManager.Sounds startConflictSound = FMODManager.Sounds.NONE;
    
    /// <summary>
    /// Sonido que se reproduce mientras se reproduce el sonido
    /// </summary>
    public FMODManager.Sounds runningConflictSound = FMODManager.Sounds.NONE;
    
    /// <summary>
    /// Sonido instantaneo al terminar el conflicto
    /// </summary>
    public FMODManager.Sounds endConflictSound = FMODManager.Sounds.NONE;
    
    #endregion

    /// <summary>
    /// On click pressed method
    /// </summary>
    public override void OnClickPressed()
    {
        Debug.Log("On click");
        if (inConflict) {
            EndConflict();
        }
    }

    /// <summary>
    /// On right click pressed method
    /// </summary>
    public override void OnReleasePressed()
    {
        Debug.Log("INTERACTABLE: Click derecho");
    }

    public virtual void StartConflict(MonkRequest monkRequest)
    {
        // Sonido de comienzo y lanzamos el de running
        FMODManager.SINGLETON.PlayOneShot(startConflictSound);
        FMODManager.SINGLETON.PlaySound(runningConflictSound);
        
        this.monkRequest = monkRequest;
        inConflict = true;
    }

    public virtual void EndConflict()
    {
        // Sonido de fin y apagamos el de running
        FMODManager.SINGLETON.PlayOneShot(endConflictSound);
        FMODManager.SINGLETON.StopSound(runningConflictSound);
        
        inConflict = false;
        monkRequest.CompleteRequest(this.gameObject);
    }

}

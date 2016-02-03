using UnityEngine;
using System.Collections;

public class Candle : InteractableObject {
    public ParticleSystem fireParticles;

    void Start() {
        base.Start();
        ParticleSystem.EmissionModule emission = fireParticles.emission;
        emission.enabled = false;
    }

    public override void StartConflict(MonkRequest monkRequest)
    {
        base.StartConflict(monkRequest);
        ParticleSystem.EmissionModule emission = fireParticles.emission;
        emission.enabled = !GameManager.SINGLETON.IsFirstTurn;
    }

    public override void EndConflict()
    {
        ParticleSystem.EmissionModule emission = fireParticles.emission;
        emission.enabled = GameManager.SINGLETON.IsFirstTurn;
        base.EndConflict();
        Debug.Log("End conflict");
    }

}

using UnityEngine;
using System.Collections;

public class MonksHandsUpManager : MonoBehaviour {
    [SerializeField]
    private float minimunTimeBetweenMonksRiseArms;
    
    [SerializeField, RangeAttribute(0, 100)]
    private float monksRiseArmsProbability;
    
    [SerializeField]
    private float timeMonksHoldHandsUp;
    
    [SerializeField]
    private MonkHandsUp[] monks;
    
    private float timeSinceLastHandsUp;
    private float accumulatedTimeHundsUp;
    private bool monksAreRisingArms;


    #region SINGLETON

    // Unica instancia de la clase
    private static MonksHandsUpManager _instance = null;

    /// <summary>
    /// Propiedad para acceder al singleton
    /// </summary>
    public static MonksHandsUpManager SINGLETON
    {
        get
        {
            if (_instance == null)
                Debug.LogError("MonksHandsUpManager no inicializado");

            return _instance;
        }
    }

    #endregion

    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _instance = this;
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    private void OnDestroy()
    {
        _instance = null;
    }

	// Use this for initialization
	void Start () {
	   timeSinceLastHandsUp = 0;
       accumulatedTimeHundsUp = 0;
       monksAreRisingArms = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.SINGLETON.IsFirstTurn)
        {
            if (!monksAreRisingArms && timeSinceLastHandsUp < minimunTimeBetweenMonksRiseArms)
            {
                timeSinceLastHandsUp += Time.deltaTime;
            }
            else
            {
                if (!monksAreRisingArms)
                {
                    monksAreRisingArms = true;
                    timeSinceLastHandsUp = 0;
                    StartCoroutine(MonksHandsUp());
                }
            }
        }
	}
    
    private IEnumerator MonksHandsUp () {
        accumulatedTimeHundsUp = 0;
        foreach (MonkHandsUp monkHandsUp in monks) {
            monkHandsUp.HandsUp();
        }
		bool playerHasRisedArms = false;
        while (accumulatedTimeHundsUp < timeMonksHoldHandsUp) {
			if (GameManager.SINGLETON.Player.HasHandsUp)
				playerHasRisedArms = true;
			else if (playerHasRisedArms) {
				GameManager.SINGLETON.DecreaseLife();
				playerHasRisedArms = false;
			}
				
            accumulatedTimeHundsUp += Time.deltaTime;
            yield return 0;
        }
		if (!GameManager.SINGLETON.Player.HasHandsUp)
			GameManager.SINGLETON.DecreaseLife();
        foreach (MonkHandsUp monkHandsUp in monks) {
            monkHandsUp.HandsDown();
        }
		monksAreRisingArms = false;
    }
}

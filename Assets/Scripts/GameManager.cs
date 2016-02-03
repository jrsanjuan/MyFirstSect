using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region FIELDS
    
    // Vida del jugador
    public int life;

    //Puntos que quitar ante un mal objeto
    public int pointsToRestWithBadObject;

    //Puntos a quitar si time out
    public int pointsToRestWithTimeout;
    
    // Tiempo total del jugador
    public float time;
    
    // Array con las ordenes que pueden mandar los monigotes
    public List<GameObject> orders = null;

    //Puntero al Life monks
    public LifeMonks lifeMonksComponent;
    
    public ParticleSystem invocationSmoke;
    
    public GameObject[] gameOverInvocations;
    
    public float[] scoreNeededForNextInvocation;

    // Array que contiene la informacion de las ordenes que ya han sido mandadas
    private List<bool> avaibleOrders = null;

    // Vida REAL del jugador
    private int _currentLife;

    // Vida REAL del jugador
    private float _currentTime;
    
    private float score;
    
    // Puntero al componente de control del player
    public PlayerActionsController player = null;
    
    private bool _playing = false;

    // Controlador de UI cuando acaba la partida
    public UIGameplayController UIController;
    
    private float _currentMonkParam = 0.0f;

    //Booleano para controlar si es el primer turno de partida
    public bool firstTurn = true;

    //Puntero a las velas para encenderlas en el primr turno
    public GameObject[] candles = null;

    //Array de velas apagadas
    private int unlightedCandles = 0;

    public MonkRequest[] monkRequest = null;

    #endregion
    
    
    #region PROPERTIES
    
    public PlayerActionsController Player { get {return player;} }

    public bool IsFirstTurn { get { return firstTurn; } }
    
    public bool Playing { get { return _playing; }}
    
    #endregion
    
    
    #region SINGLETON

    // Unica instancia de la clase
    private static GameManager _instance = null;

    /// <summary>
    /// Propiedad para acceder al singleton
    /// </summary>
    public static GameManager SINGLETON
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game Manager no inicializado");

            return _instance;
        }
    }

    #endregion

    #region UNITY_METHODS

    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _instance = this;
        score = 0;
        InitOrders();
    }
    
    private void Start () {
        invocationSmoke.gameObject.SetActive(false);
		_currentLife = life;
        
        // FIXME TEST START GAME (BORRAR)
        StartGame();

        //Inicializo el array de ordenes disponibles a true
        for (int i = 0; i < candles.Length; ++i)
        {
            candles[i].GetComponent<InteractableObject>().StartConflict(monkRequest[0]);
        }

    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    private void OnDestroy()
    {
        _instance = null;
        orders = null;
        avaibleOrders = null;
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void Update()
    {
        score += Time.deltaTime;
        _currentTime += Time.deltaTime;
        /*
        if (_currentTime >= time)
        {
            GameOver();
        }*/
        
		if (_playing)
        {
            // Actualizamos el parametro del sonido de entorno y monjes
            _currentMonkParam = Mathf.Lerp(_currentMonkParam, life - _currentLife, 0.05f);
            FMODManager.SINGLETON.ChangeParameterValue(FMODManager.Sounds.Monks, FMODManager.Parameter.Monks, _currentMonkParam);
            float timeParamValue = (2.0f * _currentTime) / scoreNeededForNextInvocation[scoreNeededForNextInvocation.Length-1];
            FMODManager.SINGLETON.ChangeParameterValue(FMODManager.Sounds.Environment, FMODManager.Parameter.Time, timeParamValue);
            
            // Comprobamos final de partida
            if(_currentLife <= 0)
                GameOver ();
        }
			
    }

    #endregion
    
    #region CUSTOM_METHODS

    /// <summary>
    /// Inicializa los arrays que contienen la informacion de las ordenes
    /// que dan los monigotes
    /// </summary>
    private void InitOrders()
    {
        avaibleOrders = new List<bool>();

        //Inicializo el array de ordenes disponibles a true
        for (int i = 0; i < orders.Count; ++i)
        {
            avaibleOrders.Add(true);
        }

        //Todas las velas apagadas
        unlightedCandles = candles.Length;

    }

    /// <summary>
    /// Aumenta la vida(actual) del jugador.
    /// </summary>
    public void IncreaseLife() { _currentLife += 1; }

    /// <summary>
    /// Disminuye la vida(actual) del jugador.
    /// </summary>
    public void DecreaseLife() { 
        _currentLife -= 1; 
		lifeMonksComponent.LifeLost();
         if (_currentLife <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Devuelve a los Monigotes que dan ordenes la siguiente orden a ejecutar
    /// </summary>
    /// <returns></returns>
    public GameObject GetNextOrder(ref int pos)
    {
        if (firstTurn)
        {
            pos = -1;
            return candles[0];
        }
        else
        {
            pos = Random.Range(0, orders.Count - 1);
            while (!avaibleOrders[pos])
            {
                pos = (pos + 1) % orders.Count;
            };
            avaibleOrders[pos] = false;
            return orders[pos];
        }
    }

    /// <summary>
    /// Cancela la orden indicada
    /// </summary>
    /// <param name="pos"></param>
    private void CancelOrder(int pos)
    {
       if(pos != -1) avaibleOrders[pos] = true;
    }


    /// <summary>
    /// Funcion llamada al comenzar una partida
    /// Resetea todo la partida?
    /// </summary>
    private void StartGame()
    {
        _playing = true;
        
        FMODManager.SINGLETON.StopAllSounds();
        // Sonidos durante la partida
        _currentMonkParam = 0.0f;
        FMODManager.SINGLETON.PlaySound(FMODManager.Sounds.Environment, FMODManager.Parameter.Time, 0.0f);
        FMODManager.SINGLETON.PlaySound(FMODManager.Sounds.Monks, FMODManager.Parameter.Monks, 0.0f);
    }


    /// <summary>
    /// Controla el final de partida, playeara la escena de la invocacion 
    /// final
    /// </summary>
    private void GameOver()
    { 
        _playing = false;
        
        FMODManager.SINGLETON.StopAllSounds();
        // Sonidos de fin de partida
        FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.GameOver);
        
        invocationSmoke.gameObject.SetActive(true);
        int i = 0;
        while (i < scoreNeededForNextInvocation.Length && scoreNeededForNextInvocation[i] <= score)
            ++i;
        if (i >= scoreNeededForNextInvocation.Length)
        {
            i = gameOverInvocations.Length - 1;
        }
        gameOverInvocations[i].SetActive(true);
        PlayerPrefs.SetInt("Ending" + i, 1);
       //DESACTIVAR JUGABILIDAD
        /*for (int j = 0 ; j < monkRequest.Length; i++)
        {
            monkRequest[j].GetComponent<MonkRequest>().CancelRequest();
            monkRequest[j].GetComponent<MonkRequest>().enabled = false;
        }*/
        
        switch(i) {
            case 0:
                FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.Cat);
                break;
            case 1:
                FMODManager.SINGLETON.PlaySound(FMODManager.Sounds.Grandma);
                break;
        }        
        
        this.GetComponent <InputManager>().enabled = false;
        this.GetComponent<MonksHandsUpManager>().enabled = false;
        StartCoroutine("TriggerGameOver");
    }

    private IEnumerator TriggerGameOver()
    {
        yield return new WaitForSeconds(4);
        //PARAR PERSONAJES
        UIController.ActiveGameOverUI(score);

    }
    /// <summary>
    /// Funcion a la que llamara al monk request si ha habido timeout
    /// sin exito o si se ha completado la accion
    /// </summary>
    /// <param name="successfully"></param>
    /// <param name="pos"></param>
    public void OrderCompleted(bool successfully, int pos)
    {
        if (firstTurn)
        {
            --unlightedCandles;
            firstTurn = (unlightedCandles > 0);
            Debug.Log("Primer turno: " + firstTurn);

            if (!firstTurn)
            {
                MonksHandsUpManager.SINGLETON.enabled = true;
                for (int i = 0; i < monkRequest.Length; ++i)
                {
                    monkRequest[i].bubble.DisableBubble();
                    monkRequest[i].ResetBehaviourVars();
                }
                return;
            }
        }
        else {
             if (!successfully)
            {
                _currentLife -= pointsToRestWithTimeout;
                lifeMonksComponent.LifeLost();
            }
            else
            {
                if (_currentLife < life)
                {
                    _currentLife += pointsToRestWithBadObject;
                    lifeMonksComponent.LifeGained();
                }
            }

            CancelOrder(pos);
        }
    }

    /// <summary>
    /// Funcion a la que llamara al monk request si se recibe o
    /// se hace una accion que no es
    /// </summary>
    public void BadRequestedReceived()
    {
        if (!firstTurn)
        {
            _currentLife -= pointsToRestWithBadObject;
            lifeMonksComponent.LifeLost();
        }
    }
    
    #endregion
}
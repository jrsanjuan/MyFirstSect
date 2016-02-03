using UnityEngine;
using System.Collections;

public class MonkRequest : MonoBehaviour 
{

	#region FIELDS
    
	/// <summary>
	/// Tiempo minimo y maximo que estara el monje sin pedir cosas
	/// </summary>
	public Vector2 minMaxTimeWithoutRequest = new Vector2(1.0f, 5.0f);
    
    /// <summary>
    /// Tiempo minimo y maximo para cumplir una peticion del monje
    /// </summary>
    public Vector2 minMaxTimeForCompleteRequest = new Vector2(5.0f, 10.0f);

    /// <summary>
    /// Indice del objeto de la orden (para devolverselo al GameManager)
    /// </summary>
    private int _orderIndex = -1;
    
    /// <summary>
    /// Puntero al GameObject de la orden actual
    /// </summary>
    private GameObject _currentOrderGO = null;
    
    /// <summary>
    /// Puntero a la corrutina de comportamiento en ejecucion
    /// </summary>
    private IEnumerator _currentCoroutine = null;
    
	public Bubble bubble;

    InteractableObject _currentOrderGOComponent;


	#endregion
    

    #region UNITY_METHODS

	/// <summary>
    /// Update del componente
    /// </summary>
	private void FixedUpdate () 
    {
        if(_currentCoroutine == null && GameManager.SINGLETON.Playing)
        {
            _currentCoroutine = MonkBehaviour();
            StartCoroutine(_currentCoroutine);
        }
	}
    
    #endregion
    
    
    #region CUSTOM_METHODS
    
    /// <summary>
    /// Avisa al monje de que se debe completar su peticion
    /// </summary>
    /// <param name="resultGO"> Objeto que se pasa al monje para la mision </param>
    public void CompleteRequest(GameObject resultGO)
    {
        // Detenemos el comportamiento actual
        StopCoroutine(_currentCoroutine);
        
        //Si estamos en el primer tunro
        if (_orderIndex == -1)
        {
            Debug.Log("[MonkRequest::MonkBehaviour] Completando peticion: CORRECTO!");
            FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.RequestSuccess);
            GameManager.SINGLETON.OrderCompleted(true, _orderIndex);
            return;
        }

        // Comprobamos resultado
        if(_currentOrderGO!=null && _currentOrderGO == resultGO)
        {
            // Correcto
            Debug.Log("[MonkRequest::MonkBehaviour] Completando peticion: CORRECTO!");
            FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.RequestSuccess);
            GameManager.SINGLETON.OrderCompleted(true, _orderIndex);
            bubble.DisableBubble();
            GameManager.SINGLETON.player.thinkingBubble.SetActive(false);
            ResetBehaviourVars();
            _currentOrderGOComponent = null;
        }
        else if(_currentOrderGOComponent == null)
        {
            // Incorrecto
            Debug.Log("[MonkRequest::MonkBehaviour] Completando peticion: FALLO!");
            FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.RequestFail);
            GameManager.SINGLETON.OrderCompleted(false, _orderIndex);
            GameManager.SINGLETON.BadRequestedReceived();
            bubble.DisableBubble();
            GameManager.SINGLETON.player.thinkingBubble.SetActive(false);
            ResetBehaviourVars();
        }
    } 
    
    /// <summary>
    /// Corrutina del comportamiento del monje
    /// </summary>
    private IEnumerator MonkBehaviour()
    {
        Debug.Log("MONK BEHAVIOUR");

        if (GameManager.SINGLETON.IsFirstTurn)
        {
            // Pedimos la nueva orden
            _currentOrderGO = GameManager.SINGLETON.GetNextOrder(ref _orderIndex);

            bubble.ActivateBubble(_currentOrderGO.GetComponentInChildren<SpriteRenderer>());

            FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.Request);
        }
        else { 
            Debug.Log("[MonkRequest::MonkBehaviour] Esperando");
        
            // Espera hasta pedir nueva orden
            yield return new WaitForSeconds(Random.Range(minMaxTimeWithoutRequest.x, minMaxTimeWithoutRequest.y));
        

            Debug.Log("[MonkRequest::MonkBehaviour] Orden pedida");
            // Pedimos la nueva orden
            _currentOrderGO = GameManager.SINGLETON.GetNextOrder(ref _orderIndex);
            InteractableObject _currentOrderGOComponent = _currentOrderGO.GetComponent<InteractableObject>();
            if (_currentOrderGOComponent != null)
            {
                _currentOrderGOComponent.StartConflict(this);
            }
		    bubble.ActivateBubble (_currentOrderGO.GetComponentInChildren<SpriteRenderer> ());

        
            FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.Request);
        
            // Mostramos la informacion de la orden
            // @TODO
        
            Debug.Log("[MonkRequest::MonkBehaviour] Esperando cumplirla");
            // Contador hasta final de tiempo hasta cumplir la orden
            yield return new WaitForSeconds(Random.Range(minMaxTimeForCompleteRequest.x, minMaxTimeForCompleteRequest.y));

            Debug.Log("[MonkRequest::MonkBehaviour] Orden Timeout");
            // Orden fallida!
            if (_currentOrderGOComponent != null)
            {
                _currentOrderGO = null;
                while (GameManager.SINGLETON.Playing) {
                    FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.RequestFail);
                    GameManager.SINGLETON.DecreaseLife();
                    yield return new WaitForSeconds(Random.Range(minMaxTimeForCompleteRequest.x, minMaxTimeForCompleteRequest.y));
                }
                GameManager.SINGLETON.OrderCompleted(false, _orderIndex);                    
            }else{
                GameManager.SINGLETON.OrderCompleted(false, _orderIndex);
            }
            FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.RequestFail);

            bubble.DisableBubble ();
        
            ResetBehaviourVars();
        }
    }
    
    
    /// <summary>
    /// Resetea las varibles del comportamiento del monje (llamar al terminar comportamientos)
    /// </summary>
    public void ResetBehaviourVars()
    {
        _currentCoroutine = null;
        _currentOrderGO = null;
        _currentOrderGOComponent = null;
    }

    public void CancelRequest() {
        StopAllCoroutines();
    }
    
    #endregion
    
    
    
}

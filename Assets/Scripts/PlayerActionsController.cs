using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerActionsController : MonoBehaviour
{
    #region FIELDS

    //Velocidad del jugador al moverse
    public float speed = 5;
    
    private bool isFacingRight = false;

	private GameObject monkTarget = null;
    
    // Vector usado para rotar el sprite cuando cambia de dirección
    private Vector3 rotationVector = new Vector3(0,180,0);
    
    private bool isRisingArms = false;

    //Controlador de animaciones
    public PlayerAnimationsController playerAnimationsController;

    //Burbuja de pensamiento a quien entregar las cosas
    public GameObject thinkingBubble;

    //Burbuja de pensamiento a quien entregar las cosas
    public Image thinkingBubbleImage;

    // Objeto que lleva el jugador 
    GameObject carriedObject;
    public GameObject CarriedObject
    {
        get
        {
            return carriedObject;
        }
        set
        {
            carriedObject = value;
        }
    }

	public GameObject realCarriedObject = null;
    
    public bool HasHandsUp {
        get { return isRisingArms; }
    }

    //Objetivo del jugador
    Vector2 targetPosition;

    //Objeto a ser recogido
    bool isMovingToCarry;

    #endregion

    #region UNITY_METHODS

	void Start()
	{
		targetPosition = transform.position;
	}

    void Update()
    {
        if (Mathf.Abs(transform.position.x - targetPosition.x) > 0.01)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        }
        else
        {
            transform.position = targetPosition;
            if (!isRisingArms)
            {
                playerAnimationsController.Idle();
            }
            if (monkTarget != null)
            {
				monkTarget.GetComponent<MonkRequest> ().CompleteRequest (realCarriedObject);
				monkTarget = null;
				if (realCarriedObject)
					realCarriedObject.SetActive (true);
				realCarriedObject = null;
			}
            if (carriedObject != null && isMovingToCarry)
            {
                isMovingToCarry = false;
				if (realCarriedObject != null)
					realCarriedObject.SetActive (true);
				
				carriedObject.GetComponent<ClickableObject>().OnClickPressed();
				carriedObject = null;
			
                playerAnimationsController.ÍnteractWithObject();
            }
        }
    }
    #endregion

    #region CUSTOM_METHODS
    //ACCIONES
    public void WalkToPoint(Vector2 targetPoint)
    {
        targetPosition = targetPoint;
        //Call WALK animation
        rotatePlayerIfNeeded();
        if (isRisingArms) {
            playerAnimationsController.DownArms();
            isRisingArms = false; 
        } else playerAnimationsController.Walk();
    }

    public void RiseArms()
    {
        isRisingArms = true;
        ///Call RISEARMS animation
        playerAnimationsController.RiseArms();
    }

    public void InteractWithObject(GameObject clickable)
    {
		if (clickable.tag != "Monk") {
			carriedObject = clickable;
		} else {
			monkTarget = clickable;
		}
		isMovingToCarry = true;

        if (clickable.GetComponent<PickableObject>()) {
            thinkingBubbleImage.sprite = clickable.GetComponentInChildren<SpriteRenderer>().sprite;
            thinkingBubble.SetActive(true);
        }
        
        targetPosition = clickable.GetComponent<ClickableObject>().TakeObjectPosition;
        rotatePlayerIfNeeded();      
        if (isRisingArms) {
            playerAnimationsController.DownArms();
            isRisingArms = false; 
        } else playerAnimationsController.Walk();
        //Call INTERACT animation
    }
    
    private void rotatePlayerIfNeeded () {
        if ((transform.position.x < targetPosition.x) != isFacingRight) {
            transform.Rotate(rotationVector);
            isFacingRight = !isFacingRight;
        }
    }
    
    private IEnumerator downArmsAndWalk () {
        playerAnimationsController.DownArms();
        isRisingArms = false;
        yield return new WaitForSeconds(0.25f);
        playerAnimationsController.Walk();
    }
    
    #endregion

}
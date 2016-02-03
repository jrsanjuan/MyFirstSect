using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region FIELDS

    /// <summary>
    /// Objeto que sobre el que esta el raton en cada momento
    /// </summary>
    private GameObject _currentHoverObject = null;

    #endregion


    #region UNITY_METHODS

    /// <summary>
    /// Update del componente
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                //Debug.Log("LeftClicked: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject.name == "Walkable")
                {
                    GameManager.SINGLETON.Player.WalkToPoint(hit.point);
                }

                if (hit.collider.gameObject == GameManager.SINGLETON.Player.gameObject)
                {
                    GameManager.SINGLETON.Player.RiseArms();
                }

                ClickableObject clickable;

                if (clickable = hit.collider.gameObject.GetComponent<ClickableObject>())
                {
                    if ((GameManager.SINGLETON.IsFirstTurn && hit.collider.gameObject.GetComponent<Candle>())
                         || !GameManager.SINGLETON.IsFirstTurn)
                    {
                        Debug.Log("Interactuando!");
                        GameManager.SINGLETON.Player.InteractWithObject(clickable.gameObject);
                    }
                    else if (GameManager.SINGLETON.IsFirstTurn)
                    {
                        FMODManager.SINGLETON.PlayOneShot(FMODManager.Sounds.RequestFail);
                    }
                }
                // TODO: Llamar a quien le importe CLICK IZQUIERDO
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject carriedObject = GameManager.SINGLETON.Player.CarriedObject;
            if (carriedObject)
                carriedObject.GetComponent<ClickableObject>().OnReleasePressed();

        }
    }

    /// <summary>
    /// FixedUpdate del componente
    /// </summary>
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (_currentHoverObject != hit.collider.gameObject)
            {
                if (_currentHoverObject != null) {
                     _currentHoverObject.BroadcastMessage("DisableOutline",SendMessageOptions.DontRequireReceiver);
                }
                _currentHoverObject = hit.collider.gameObject;
                //Debug.Log("HoverBegin: " + _currentHoverObject.name);
                // TODO: Llamar a quien le importe HoverBEGIN

				_currentHoverObject.BroadcastMessage("EnableOutline",SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if (_currentHoverObject != null)
            {
                // TODO: Llamar a quien le importe HoverEND
				_currentHoverObject.BroadcastMessage("DisableOutline",SendMessageOptions.DontRequireReceiver);
                //Debug.Log("HoverEnd: " + _currentHoverObject.name);
                _currentHoverObject = null;
            }
        }
    }

    #endregion

}

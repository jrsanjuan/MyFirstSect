using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickableObject : MonoBehaviour
{
    #region PARAMETERS
    
    /// <summary>
    /// Object image
    /// </summary>
    public SpriteRenderer image;

    /// <summary>
    /// Posicion en la que colocaremos al player 
    /// para coger el objeto
    /// </summary>
    private Vector2 takeObjectPosition;
    public Vector2 TakeObjectPosition {
        get {
            return takeObjectPosition;
        }
    }
    
    #endregion

    #region UNITY_METHODS

    // Use this for initialization
	protected void Start () {
        takeObjectPosition = transform.FindChild("TakeObjectPosition").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
    }
    
    #endregion

    #region CUSTOM_METHODS

    /// <summary>
    /// On left click pressed method
    /// </summary>
    public virtual void OnClickPressed()
    {
        Debug.Log("On clickable pressed");

    }

    /// <summary>
    /// On right click pressed method
    /// </summary>
    public virtual void OnReleasePressed()
    {

    }

    #endregion

}

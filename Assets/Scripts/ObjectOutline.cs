using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectOutline : MonoBehaviour {
    
    [SerializeField]
    private int outlineColorIndex = 0;
    
    private SpriteRenderer spriteRenderer;
    private OutlineEffect cameraOutlineEffect;

	void Start () {
	   cameraOutlineEffect = OutlineEffect.Instance;
       spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void EnableOutline () {
        cameraOutlineEffect.AddRenderer(spriteRenderer).WithColorFromIndex(outlineColorIndex);
    }	
    
    public void DisableOutline () {
        cameraOutlineEffect.EraseRenderer(spriteRenderer);
    }
}

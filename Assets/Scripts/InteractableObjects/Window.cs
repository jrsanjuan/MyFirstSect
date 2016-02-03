using UnityEngine;
using System.Collections;

public class Window : InteractableObject
{
    public Sprite windowOpen;
    public Sprite windowClose;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        base.Start();
        spriteRenderer = image.GetComponent<SpriteRenderer>();
    }

    public override void StartConflict(MonkRequest monkRequest)
    {
        base.StartConflict(monkRequest);
        spriteRenderer.sprite = windowOpen;
    }

    public override void EndConflict()
    {
        base.EndConflict();
        spriteRenderer.sprite = windowClose;
    }
}
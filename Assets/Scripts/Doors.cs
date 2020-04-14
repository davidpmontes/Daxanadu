﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Doors : MonoBehaviour
{
    public Vector2 playerDestination;
    public Vector2 cameraDestination;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (InputController.Instance.DirectionalInput.y > 0.5f)
        {
            ViewSwitcher.Instance.Teleporting(playerDestination, cameraDestination);
        }
    }
}

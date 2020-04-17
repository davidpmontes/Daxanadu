using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Weapon : MonoBehaviour
{
    private void Update()
    {
        transform.position = Player.Instance.transform.position;
        transform.localScale = new Vector3(Player.Instance.spriteRenderer.flipX ? -1 : 1, 1, 1);
    }
}

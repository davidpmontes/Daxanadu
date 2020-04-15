using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GreenSlime : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player.Instance.OnEnemyCollided(transform.position);
    }
}

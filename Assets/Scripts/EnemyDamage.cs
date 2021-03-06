﻿using System.Collections;
using DarkTonic.MasterAudio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyDamageConfig config = default;
    private SpriteRenderer spriteRenderer;

    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    private int life;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");

        life = config.life;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<Player>().OnReceiveDamage(transform.position);
        }
        else
        {
            OnReceiveDamage(collision.transform.position);
        }
    }

    public void OnReceiveDamage(Vector2 objPosition)
    {
        //velocity.x = Mathf.Sign(transform.position.x - objPosition.x) * 5;
        life -= 1;
        if (life <= 0)
        {
            MasterAudio.PlaySoundAndForget(config.deathSound);
            var coin = Instantiate(config.coinPrefab);
            coin.GetComponent<Coin>().Initialize(transform.position);
            Destroy(gameObject);
        }
        StartCoroutine(FlashRed());
        MasterAudio.PlaySoundAndForget(config.damageSound);
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.material.shader = shaderGUItext;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.shader = shaderSpritesDefault;
        spriteRenderer.color = Color.white;
    }
}

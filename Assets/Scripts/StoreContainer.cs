﻿using UnityEngine;

public class StoreContainer : MonoBehaviour
{
    public static StoreContainer Instance { get; private set; }

    [SerializeField] private GameObject frame = default;
    [SerializeField] private GameObject caretStartPosition = default;

    public Vector2 GetCaretStartPosition() => caretStartPosition.transform.position;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show()
    {
        frame.SetActive(true);
    }

    public void Hide()
    {
        frame.SetActive(false);
    }
}

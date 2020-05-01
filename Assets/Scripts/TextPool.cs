using System;
using System.Collections.Generic;
using UnityEngine;

public class TextPool : MonoBehaviour
{
    public enum TextPools
    {
        ScrollingConversation,
        ChoicePicker
    };

    public GameObject[] prefabs;
    private Dictionary<string, Queue<GameObject>> dictOfPools;
    private Dictionary<string, GameObject> dictOfPrefabs;

    public static TextPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        dictOfPools = new Dictionary<string, Queue<GameObject>>();
        dictOfPrefabs = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in prefabs)
        {
            dictOfPools.Add(prefab.name, new Queue<GameObject>());
            dictOfPrefabs.Add(prefab.name, prefab);
        }

        foreach (TextPools item in Enum.GetValues(typeof(TextPools)))
        {
            GrowPool(item, 3);
        }
    }

    private void GrowPool(TextPools poolName, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var instance = Instantiate(dictOfPrefabs[poolName.ToString()]);
            instance.name = poolName.ToString();
            instance.transform.SetParent(transform);
            DeactivateAndAddToPool(instance);
        }
    }

    public GameObject GetFromPoolActiveSetTransform(TextPools poolName, Transform t)
    {
        Queue<GameObject> pool = dictOfPools[poolName.ToString()];
        if (pool.Count == 0)
            GrowPool(poolName, 3);
        GameObject instance = pool.Dequeue();
        instance.transform.position = t.position;
        instance.transform.rotation = t.rotation;
        instance.SetActive(true);
        return instance;
    }

    public GameObject GetFromPoolInactive(TextPools poolName)
    {
        Queue<GameObject> pool = dictOfPools[poolName.ToString()];
        if (pool.Count == 0)
            GrowPool(poolName, 3);
        var instance = pool.Dequeue();
        return instance;
    }

    public void DeactivateAndAddToPool(GameObject instance)
    {
        if (instance == null)
            return;

        instance.SetActive(false);
        instance.transform.SetParent(transform);
        Queue<GameObject> pool = dictOfPools[instance.name];
        pool.Enqueue(instance);
    }
}

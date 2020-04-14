using System;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetPool : MonoBehaviour
{
    public enum Pools
    {
        _a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k, _l, _m, _n, _o, _p,
        _q, _r, _s, _t, _u, _v, _w, _x, _y, _z,
        _A, _B, _C, _D, _E, _F, _G, _H, _I, _J, _K, _L, _M, _N, _O, _P,
        _Q, _R, _S, _T, _U, _V, _W, _X, _Y, _Z,
        _0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
        _apostrophe, _comma, _doubleQuotes, _exclamationPoint, _hyphen,
        _period, _questionMark, _underscore, _caret
    };

    public GameObject[] prefabs;
    private Dictionary<string, Queue<GameObject>> dictOfPools;
    private Dictionary<string, GameObject> dictOfPrefabs;

    public static AlphabetPool Instance { get; private set; }

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

        foreach (Pools item in Enum.GetValues(typeof(Pools)))
        {
            GrowPool(item, 3);
        }
    }

    private void GrowPool(Pools poolName, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var instance = Instantiate(dictOfPrefabs[poolName.ToString()]);
            instance.name = poolName.ToString();
            instance.transform.SetParent(transform);
            DeactivateAndAddToPool(instance);
        }
    }

    public GameObject GetFromPoolActiveSetTransform(Pools poolName, Transform t)
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

    public GameObject GetFromPoolInactive(Pools poolName)
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

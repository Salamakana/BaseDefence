using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singelton<ObjectPoolManager>
{
    private Dictionary<string, Stack<GameObject>> poolDictionary = new Dictionary<string, Stack<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        GetResources();
    }

    private void GetResources()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs") as GameObject[];

        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabDictionary.Add(prefabs[i].name, prefabs[i]);
        }
    }

    public GameObject GetObjectFromPool(string objectName, Vector3 position, Quaternion rotation, bool isActive = true)
    {
        Stack<GameObject> tempStack;

        if(poolDictionary.TryGetValue(objectName, out tempStack))
        {
            if(tempStack.Count > 0)
            {
                GameObject pooledObject = tempStack.Pop();
                pooledObject.transform.SetPositionAndRotation(position, rotation);
                pooledObject.SetActive(isActive);
                return pooledObject;
            }
            else
            {
                return CreateObject(objectName, position, rotation, isActive);
            }
        }
        else
        {
            poolDictionary.Add(objectName, new Stack<GameObject>());
            return CreateObject(objectName, position, rotation, isActive);
        }
    }

    private GameObject CreateObject(string prefabName, Vector3 position, Quaternion rotation, bool isActive)
    {
        GameObject newInstance;

        if(prefabDictionary.TryGetValue(prefabName, out newInstance))
        {
            newInstance = Instantiate(newInstance, position, rotation);
            newInstance.name = prefabName;
            newInstance.SetActive(isActive);
            return newInstance;
        }

        return null;
    }

    public void ReturnObjectToPool(GameObject instance)
    {
        instance.SetActive(false);

        Stack<GameObject> tempStack;

        if(poolDictionary.TryGetValue(instance.name, out tempStack))
        {
            tempStack.Push(instance);
        }
        else
        {
            Debug.LogError("We did not have stack to store the object");
            poolDictionary.Add(instance.name, new Stack<GameObject>());
            ReturnObjectToPool(instance);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private string _pooledObjectName;
    [SerializeField] private GameObject _pooledObject;
    [SerializeField] protected int _pooledAmount = 40;

    protected int _index = -1;
    protected List<GameObject> _pooledObjectsList;

    private void Awake()
    {
        if (_pooledObject != null)
        {
            _pooledObjectsList = new List<GameObject>();
            for (int i = 0; i < _pooledAmount; i++)
            {
                GameObject obj = Instantiate(_pooledObject, gameObject.transform);
                obj.name = _pooledObjectName + " - " + i;
                obj.SetActive(false);
                obj.transform.localScale = _pooledObject.transform.localScale;

                _pooledObjectsList.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        GameObject obj = _pooledObjectsList[^1];
        _pooledObjectsList.RemoveAt(_pooledObjectsList.Count - 1);
        //obj.SetActive(true);
        return obj;

        //for (int i = 0; i < _pooledAmount; i++)
        //{
        //    if (++_index == _pooledAmount)
        //    {
        //        _index = 0;
        //    }
        //    if (_pooledObjectsList == null || _pooledObjectsList[_index] == null)
        //    {
        //        return null;
        //    }
        //    if (!_pooledObjectsList[_index].activeInHierarchy)
        //    {
        //        return _pooledObjectsList[_index];
        //    }
        //}
        //GameObject obj = Instantiate(_pooledObject, gameObject.transform);
        //obj.name = _pooledObjectName + " - " + _pooledAmount;
        //obj.SetActive(false);
        //_pooledAmount++;
        //_pooledObjectsList.Add(obj);
        //return obj;
    }

    public void Release(GameObject pObject)
    {
        _pooledObjectsList.Add(pObject);
        pObject.SetActive(false);
    }    
}
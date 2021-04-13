using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;


public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

    public static T Instance => LazyInstance.Value;

    private static T CreateSingleton()
    {
        var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
        var instance = ownerObject.AddComponent<T>();
        DontDestroyOnLoad(ownerObject);
        return instance;
    }
}

public class GameManager : Singleton<GameManager>
{
    public GestureRecognition gr;
    public List<string> gestureList;
    private Dictionary<string, Gesture> _gestureData;

    public bool isCalibration = false;

    private int _calPointer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        _gestureData = new Dictionary<string, Gesture>();
        foreach (var g in gestureList)
        {
            _gestureData.Add(g,new Gesture());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCalibration && _calPointer >= gestureList.Count)
        {
            isCalibration = false;
            _calPointer = 0;
        }
    }

    public void SaveGesture()
    {
        if (isCalibration && _calPointer < gestureList.Count)
        {
            _gestureData[gestureList[_calPointer]] = gr.Save();
            _calPointer += 1;
        }
    }
}


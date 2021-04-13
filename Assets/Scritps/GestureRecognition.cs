using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

[Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class GestureRecognition : MonoBehaviour
{
    public GameManager gm;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool isRecognizing = false;
    public float threshold = 0.1f;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    
    
    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRecognizing)
        {
            Gesture currentGesture = Recognize();
            bool hasRecognized = !currentGesture.Equals(new Gesture());

            if (hasRecognized && !currentGesture.Equals(previousGesture))
            {
                Debug.Log("New Gesture Found : " + currentGesture.name);
                previousGesture = currentGesture;
                currentGesture.onRecognized.Invoke();
            }
        }
    }

    public Gesture Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            // Finger position relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        g.fingerDatas = data;
        return g;
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }

                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }

        return currentGesture;
    }
}

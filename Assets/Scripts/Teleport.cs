using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    private float animationTime = 0.1f;
    private float timeStarted;

	// Use this for initialization
	void Start () {
        timeStarted = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - timeStarted > animationTime)
            Destroy(gameObject);
	}
}

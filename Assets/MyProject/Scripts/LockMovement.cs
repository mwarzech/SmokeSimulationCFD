using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMovement : MonoBehaviour {

    private FlyCamera flyCam;

	// Use this for initialization
	void Start () {
        flyCam = GetComponent<FlyCamera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flyCam.enabled = !flyCam.enabled;
        }
	}
}

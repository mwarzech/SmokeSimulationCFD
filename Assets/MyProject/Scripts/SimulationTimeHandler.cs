using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationTimeHandler : MonoBehaviour {

    public SmokeManager smokeManager;
    public SmokeEmmiter emmiter;
    public BorderWindow windowBoundry;
    public VelocityEmmiter windowVelocity;
    public Animator windowAnim;

    public float stopEmmitTime = 35f;
    public float openWindowTime = 20f;

	// Update is called once per frame
	void Update () {
        emmiter.enable = smokeManager.simulationTime < stopEmmitTime;
        windowBoundry.enable = smokeManager.simulationTime > openWindowTime;
        windowVelocity.enable = smokeManager.simulationTime > openWindowTime;
        windowAnim.SetBool("IsOpened", smokeManager.simulationTime > openWindowTime);
    }
}

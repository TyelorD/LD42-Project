using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCRunningState : IState<GameController> {

    private const string STATE_NAME = "Running";

    public void EnterState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Enter");
    }

    public void ExecuteState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Execute");
    }

    public void ExitState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Exit");
    }
}

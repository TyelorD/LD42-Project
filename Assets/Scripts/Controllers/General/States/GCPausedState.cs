using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCPausedState : IState<GameController> {

    private const string STATE_NAME = "Paused";

    public void EnterState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Enter");

        MainMenu.SetPauseOverlayActive(true);
        Time.timeScale = 0;
        //Debug.Log(STATE_NAME + ": Game Paused");
    }

    public void ExecuteState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Execute");
    }

    public void ExitState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Exit");

        MainMenu.SetPauseOverlayActive(false);
        Time.timeScale = 1;
        //Debug.Log(STATE_NAME + ": Game Unpaused");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The following code was made with help found on the Unity Forums at: https://forum.unity.com/threads/c-proper-state-machine.380612/
// I found the solution by KelsoMRK to be very robust and decided to iterate upon it for my game. (I also found https://blog.playmedusa.com/a-finite-state-machine-in-c-for-unity3d/ useful).
public class StateMachine<E> {

    public IState<E> currentState { get; private set; }
    private E controller;


    public StateMachine(E controller)
    {
        this.controller = controller;
    }

    public void ChangeState(IState<E> newState)
    {
        if (newState != null)
        {
            if (currentState != null)
                currentState.ExitState(controller);

            currentState = newState;
            currentState.EnterState(controller);
        }
    }

	public void OnUpdate()
    {
        if (currentState != null)
            currentState.ExecuteState(controller);
	}
}

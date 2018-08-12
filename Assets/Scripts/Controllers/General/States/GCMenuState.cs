using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCMenuState : IState<GameController> {

    private const string STATE_NAME = "Menu";
    
    private GameObject menu, startGameBtn;

    public GCMenuState()
    {
        menu = GameObject.Find("MainMenu");
        startGameBtn = GameObject.Find("StartGameButton");
    }

    public void EnterState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Enter");

        menu.SetActive(true);

        MenuSelector.SelectedObject = startGameBtn;

        MainMenu.mainMenu.OnEnterMainMenu();
    }

    public void ExecuteState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Execute");
    }

    public void ExitState(GameController controller)
    {
        //Debug.Log(STATE_NAME + ": Exit");

        menu.SetActive(false);

        //GameController.StartGame();
    }
}

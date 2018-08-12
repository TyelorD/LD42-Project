using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    //public static bool UsingController { get; private set; }

    private static Vector2 move = Vector2.zero, mousePos = Vector2.zero, mousePosLast = Vector2.zero;

    private static PlayerController player;
    private static new Transform transform;
    
    private static float hori, horiKB, vert, vertKB, mouseX, mouseY, swapAxis;
    private static bool isSwapPlayer;

    public static bool UsedControllerLast { get; private set; }

    public static bool UsingController
    {
        get
        {
            return (UsedControllerLast || !Mathf.Approximately(hori, 0f) || !Mathf.Approximately(vert, 0f) || Input.GetButtonUp("Menu") || Input.GetButtonUp("SwapPlayer"));
        }
    }

    public static bool UsingKeyboard
    {
        get
        {
            return (!UsedControllerLast || !Mathf.Approximately(horiKB, 0f) || !Mathf.Approximately(vertKB, 0f) || !mousePos.Equals(mousePosLast) || Input.GetButtonUp("MenuKB") || Input.GetButtonUp("SwapPlayerKB"));
        }
    }

    private void Update()
    {

        GetInputs();

        if (Input.GetKeyUp(KeyCode.F8))
            LogController.SetActive(!LogController.debugOn);

        if (Input.GetButtonUp("Cancel"))
            MainMenu.mainMenu.OnInputCancel();

        if (player != null && (Input.GetButtonUp("MenuKB") || Input.GetButtonUp("Menu")))
            GameController.PauseGame(GameController.IsRunning);

        if (GameController.IsRunning && player != null && GameController.notExitingLevel && GameController.PlayerNotDying)
        {
            if (UsingController)
            {
                move.x = hori;
                move.y = vert;

                swapAxis = Input.GetAxis("SwapPlayer");
            }
            else if(UsingKeyboard)
            {
                move.x = horiKB;
                move.y = vertKB;

                swapAxis = Input.GetAxis("SwapPlayerKB");
            }

            player.MovePlayer(move);

            GameController.SwapPlayer(isSwapPlayer, swapAxis);
        }

        mousePosLast = mousePos;
    }
    
    public static void SetPlayer(PlayerController player)
    {
        InputController.player = player;
        transform = player.transform;

        move = Vector2.zero;
    }

    private static void GetInputs()
    {
        hori = Input.GetAxis("Horizontal");
        horiKB = Input.GetAxis("HorizontalKB");
        vert = Input.GetAxis("Vertical");
        vertKB = Input.GetAxis("VerticalKB");
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
        swapAxis = 0f;

        isSwapPlayer = Input.GetButtonDown("SwapPlayerKB");
        isSwapPlayer = isSwapPlayer || Input.GetButtonDown("SwapPlayer");

        mousePos.x = mouseX;
        mousePos.y = mouseY;
        move = Vector2.zero;

        if (UsingKeyboard)
            UsedControllerLast = false;
        if (UsingController)
            UsedControllerLast = true;

        OnUsingController(UsedControllerLast);
    }

    private static void OnUsingController(bool usingController)
    {
        Cursor.visible = !usingController;

        if (!usingController)
            MenuSelector.UnselectObject();
    }
}

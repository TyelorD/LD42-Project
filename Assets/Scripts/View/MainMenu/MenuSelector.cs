using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventSystem))]
public class MenuSelector : MonoBehaviour {

    private static EventSystem eventSystem;

    private static bool buttonSelected;

    private static GameObject _SelectedObject;
    public static GameObject SelectedObject
    {
        get
        {
            return _SelectedObject;
        }

        set
        {
            _SelectedObject = value;

            if(buttonSelected)
                eventSystem.SetSelectedGameObject(_SelectedObject);
        }
    }

    /*private static bool IsUsingController
    {
        get
        {
            return Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0 || Input.GetButton("Submit") || Input.GetButton("Menu");
        }
    }*/

    private static MenuSelector _menuSelector;
    public static MenuSelector menuSelector
    {
        get
        {
            if (_menuSelector == null)
                _menuSelector = FindObjectOfType<MenuSelector>();

            return _menuSelector;
        }
    }

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (InputController.UsingController && buttonSelected == false) 
        {
            eventSystem.SetSelectedGameObject(SelectedObject);
            buttonSelected = true;
            //Cursor.visible = false;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
        //Cursor.visible = true;
    }

    public static void UnselectObject()
    {
        if(buttonSelected)
        {
            eventSystem.SetSelectedGameObject(null);
            buttonSelected = false;
        }
    }

}
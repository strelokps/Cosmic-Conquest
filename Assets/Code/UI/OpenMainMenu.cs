using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMainMenu : MonoBehaviour
{
    [SerializeField] private Canvas canvasMainMenu;

    private void Awake()
    {
        CloseCanvasMainMenu();
    }

    public void OpenCanvasMainMenu()
    {
        canvasMainMenu.enabled = true;
    }

    public void CloseCanvasMainMenu()
    {
        canvasMainMenu.enabled = false;
    }
}

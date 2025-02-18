using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsDisplayManager : MonoBehaviour
{
    public GameObject controlsPanel; // Reference to the ControlsPanel
    private bool controlsVisible = false;

    private PlayerControls controls;
    private bool isGamePaused;
    private bool isGameOver;
    private bool isMainMenu;
    private bool isLevelSelector;

    void Awake()
    {
        controls = new PlayerControls();

        // Bind the input action for showing controls
        controls.UI.ShowControls.performed += ctx => ToggleControls();
    }

    void OnEnable()
    {
        controls.UI.Enable();
    }

    void OnDisable()
    {
        controls.UI.Disable();
    }

    public void SetGamePaused(bool state)
    {
        isGamePaused = state;
    }

    public void SetGameOver(bool state)
    {
        isGameOver = state;
    }

    public void SetMainMenu(bool state)
    {
        isMainMenu = state;
    }

    public void SetLevelSelector(bool state)
    {
        isLevelSelector = state;
    }

    void ToggleControls()
    {
        if (isGamePaused || isGameOver || isMainMenu || isLevelSelector)
        {
            return;
        }

        controlsVisible = !controlsVisible;
        controlsPanel.SetActive(controlsVisible);
    }
}

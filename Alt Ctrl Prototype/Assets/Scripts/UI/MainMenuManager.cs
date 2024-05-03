using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Panel")]
    public GameObject PNL_MainMenu;

    [Header("Settings Panel")]
    public GameObject PNL_Settings;
    public TMP_Dropdown DDL_COMPort;

    private List<string> _COMPortNames;

    public void BTN_Play()
    {
        // TODO: load level scene
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    public void BTN_Settings()
    {
        PNL_MainMenu.SetActive(false);
        PNL_Settings.SetActive(true);
    }

    public void BTN_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RefreshCOMPorts()
    {
        DDL_COMPort.ClearOptions();
        _COMPortNames = InputHandler.GetCOMPortNames();
        DDL_COMPort.AddOptions(_COMPortNames);
    }

    public void DDL_COMPort_Changed()
    {
        InputHandler.SetCOMPort(_COMPortNames[DDL_COMPort.value]);
    }

    public void BTN_CloseSettings()
    {
        PNL_Settings.SetActive(false);
        PNL_MainMenu.SetActive(true);
    }

}

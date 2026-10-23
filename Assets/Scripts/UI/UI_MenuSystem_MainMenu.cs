using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_MenuSystem_MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;

    public void SetLanguage(int i) => UI_Language.Set(i);

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
using UnityEngine;
//usar el scene management para cargar escenas
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuSettings;
    public GameObject menuMain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        // Cargar la escena actual + 1
        Debug.Log("Play Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        // Salir del juego
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void OpenSettings()
    {
        // Aquí puedes implementar la lógica para abrir el menú de configuración
        Debug.Log("Open Settings");

        menuSettings.SetActive(true);
        menuMain.SetActive(false);
    }

    public void CloseSettings()
    {
        menuSettings.SetActive(false);
        menuMain.SetActive(true);


    }
}

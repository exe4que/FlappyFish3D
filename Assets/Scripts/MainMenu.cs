using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //Load SampleScene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}

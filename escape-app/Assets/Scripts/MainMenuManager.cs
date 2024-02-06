using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public async void ContinueLevel()
    {
        LoadingScreenManager.Instance.OpenLoadingScreen();
        await Task.Delay(500);
        SceneManager.LoadScene(1);
    }
}

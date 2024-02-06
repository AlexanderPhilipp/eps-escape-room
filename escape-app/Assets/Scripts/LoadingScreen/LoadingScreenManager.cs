using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
     public static LoadingScreenManager Instance;
     
     [Header("Components")] public RectTransform loadingBlocker;
     [Header("Settings")] public float loadingScreenTransitionDuration = 1f;
     
     void Awake()
     {
          if(Instance != null)
               Destroy(this.gameObject);
          else
          {
               Instance = this;
               DontDestroyOnLoad(this.gameObject);
          }
     }

     private void Update()
     {
          if(Input.GetKeyDown(KeyCode.A))
               OpenLoadingScreen();
          else if(Input.GetKeyDown(KeyCode.S))
               CloseLoadingScreen();
     }

     public void OpenLoadingScreen()
     {
          SceneManager.sceneLoaded += OnSceneLoaded;
          loadingBlocker.LeanMoveLocalX(0, loadingScreenTransitionDuration).setEaseOutQuad();
     }

     public async void CloseLoadingScreen()
     {
          SceneManager.sceneLoaded -= OnSceneLoaded;
          loadingBlocker.LeanMoveLocalX(-Screen.width - 1000, loadingScreenTransitionDuration).setEaseInQuad();
          await Task.Delay(Mathf.RoundToInt(loadingScreenTransitionDuration * 1000));
          loadingBlocker.anchoredPosition = new Vector2(Screen.width + 1000, 0);
     }

     public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
     {
          CloseLoadingScreen();
     }
}

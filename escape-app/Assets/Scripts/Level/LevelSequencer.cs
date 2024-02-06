using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LevelSequencer : MonoBehaviour
{
    [Header("Components")] 
    public VideoPlayer videoPlayer;
    public RectTransform videoRenderer;
    public Image imageDisplay;
    public CanvasGroup sequenceFader;
    
    
    [Header("Level Sequence")] 
    public Sequence[] sequences;

    [Header("Next Level Logic")] 
    public int nextLevelIndex;

    private int _currentSequence;

    public async void SetSequence(int index)
    {
        for (int i = 0; i < sequences.Length; i++)
        {
            if(sequences[i].sequenceButtonPage == null)
                continue;
            
            if(i == index)
                sequences[i].sequenceButtonPage.SetActive(true);
            else
                sequences[i].sequenceButtonPage.SetActive(false);
        }
        
        _currentSequence = index;

        if (sequences[index].sequenceType == Sequence.SequenceType.Video)
        {
            EnterSequenceTransition();
            await Task.Delay(200);
            sequences[index].StartSequence(this);
        }
        else
        {
            sequences[index].StartSequence(this);
        }
    }
    
    private void Start()
    {
        if (sequences != null && sequences.Length > 1)
        {
            sequences[0].StartSequence(this);
        }
    }
    void Update()
    {
          if(imageDisplay.sprite == null)
              imageDisplay.gameObject.SetActive(false);
          else
              imageDisplay.gameObject.SetActive(true);
          
          if(videoPlayer.clip == null)
              videoRenderer.gameObject.SetActive(false);
          else
              videoRenderer.gameObject.SetActive(true);
    }

    void EnterSequenceTransition()
    {
        sequenceFader.LeanAlpha(1, 0.2f);
    }

    void ExitSequenceTransition()
    {
        sequenceFader.LeanAlpha(0, 0.2f);
    }

    public async void LoadNextLevel()
    {
        LoadingScreenManager.Instance.OpenLoadingScreen();
        await Task.Delay(550);
        SceneManager.LoadScene(nextLevelIndex);
    }
    

    [System.Serializable]
    public class Sequence
    {
        public enum SequenceType
        {
            Video, Image, Text
        }

        
        [Header("General")]
        public string sequenceName;
        public SequenceType sequenceType;

        [Header("Components")] 
        public GameObject sequenceButtonPage;
        
        [Header("Events")] 
        public UnityEvent onVideoSequenceFinished;
        
        [Header("Data")] 
        public Sprite imageData;
        public VideoClip videoData;
        public string textData;
        
        public async void StartSequence(LevelSequencer sequencer)
        {
            sequencer.videoPlayer.clip = null;
            sequencer.imageDisplay.sprite = null;
            
            switch (sequenceType)
            {
                case SequenceType.Video:
                    sequencer.videoPlayer.clip = videoData;
                    sequencer.videoPlayer.Prepare();
                    sequencer.videoPlayer.prepareCompleted += OnVideoPlayerPreLoaded;
                    await Task.Delay(Mathf.RoundToInt((float)videoData.length * 1000));
                    onVideoSequenceFinished.Invoke();
                    break;
                case SequenceType.Image:
                    sequencer.imageDisplay.sprite = imageData;
                    break;
                case SequenceType.Text:
                    break;
            }
        }

        public void OnVideoPlayerPreLoaded(VideoPlayer p)
        {
            
        }
    }
}

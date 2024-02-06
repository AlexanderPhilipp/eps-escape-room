using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class CustomMenuPage : MonoBehaviour
{
     [Header("Base Components")]
    public CanvasGroup pageBackground;
    public PageGroup[] pageGroups;

    [Header("Base Settings")]
    public string pageIdentifier;

    [Header("Base Animations")]
    public float backgroundFadeSpeed = 0.2f;
    public float elementFadeSpeed = 0.2f;
    public float elementFadeDelay = 0.05f;
    public float elementGroupDelay = 0.05f;
    
    [Header("Runtime")]
    public bool isOpen = false;
    public bool isTransitioning = false;


    public virtual async void OpenPage()
    {
        if (isOpen || isTransitioning)
            return;

        isTransitioning = true;
        transform.SetAsLastSibling();
        
        foreach (PageGroup group in pageGroups)
        {
            foreach (CanvasGroup e in group.groupElements)
            {
                e.alpha = 0;
                e.interactable = true;
                e.blocksRaycasts = true;
                e.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        if(pageBackground != null)
        {
            pageBackground.alpha = 0;

            pageBackground.interactable = true;
            pageBackground.blocksRaycasts = true;
            pageBackground.LeanAlpha(1, backgroundFadeSpeed);

            await Task.Delay(Mathf.RoundToInt(backgroundFadeSpeed * 1000));
        }

        OpenAnimateElements();
        isOpen = true;
        isTransitioning = false;
    }

    public virtual async void OpenAnimateElements()
    {
        foreach (PageGroup group in pageGroups)
        {
            foreach(CanvasGroup e in group.groupElements)
            {
                e.LeanAlpha(1, elementFadeSpeed);
                e.transform.LeanScale(Vector3.one, elementFadeSpeed).setEaseOutBack();
                await Task.Delay(Mathf.RoundToInt(elementFadeDelay * 1000));
            }
            await Task.Delay(Mathf.RoundToInt(elementGroupDelay * 1000));
        }

        OnElementsOpenAnimated();
    }

    public virtual void OnElementsOpenAnimated()
    {

    }

    public virtual async void ClosePage()
    {
        if (!isOpen ||isTransitioning)
            return;

        isTransitioning = true;

        CloseAnimateElements();

        if (pageBackground != null)
        {
            pageBackground.alpha = 1;
            pageBackground.LeanAlpha(0, backgroundFadeSpeed);

            await Task.Delay(Mathf.RoundToInt(backgroundFadeSpeed * 1000));

            pageBackground.interactable = false;
            pageBackground.blocksRaycasts = false;
        }

        transform.SetAsFirstSibling();
        isOpen = false;

        isTransitioning = false;
    }

    public virtual async void CloseAnimateElements()
    {
        foreach (PageGroup group in pageGroups)
        {
            foreach (CanvasGroup e in group.groupElements)
            {
                e.alpha = 1;
                e.transform.localScale = new Vector3(1f, 1f, 1f);

                e.LeanAlpha(0, elementFadeSpeed);
                e.transform.LeanScale(Vector3.one / 2, elementFadeSpeed).setEaseInBack();
            }
        }

        await Task.Delay(Mathf.RoundToInt(elementFadeSpeed * 1000));

        foreach (PageGroup group in pageGroups)
        {
            foreach (CanvasGroup e in group.groupElements)
            {
                e.interactable = false;
                e.blocksRaycasts = false;
            }
        }

        OnElementsCloseAnimated();
    }
    public virtual void OnElementsCloseAnimated()
    {
        isTransitioning = false;
    }


    [System.Serializable]
    public class PageGroup
    {
        public CanvasGroup[] groupElements;
    }
}

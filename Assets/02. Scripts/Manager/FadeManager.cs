using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum FadeType
{
    IN = 0,
    OUT
}

public class FadeManager : MonoBehaviour
{
    [Header("Fade 애니메이터")]
    [SerializeField]
    Animator uiAnimator;

    [Header("로딩 UI 오브젝트")]
    [SerializeField]
    GameObject loadingUI;

    Action OnEndFade;   // 페이드 코루틴 종료 후 초기화 액션

    public void Fade(FadeType fadeType)
    {
        //Time.timeScale = 0f;
        switch(fadeType)
        {
            case FadeType.IN:
                uiAnimator.SetTrigger("FadeIn");
                break;
            case FadeType.OUT:
                uiAnimator.SetTrigger("FadeOut");
                break;
            default:
                break;
        }
    }
    public void LoadingUI(bool isPlay)
    {
        if (isPlay)
            loadingUI.SetActive(true);
        else
            loadingUI.SetActive(false);
    }
}

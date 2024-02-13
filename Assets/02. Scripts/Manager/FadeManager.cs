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
    [Header("Fade �ִϸ�����")]
    [SerializeField]
    Animator uiAnimator;

    [Header("�ε� UI ������Ʈ")]
    [SerializeField]
    GameObject loadingUI;

    Action OnEndFade;   // ���̵� �ڷ�ƾ ���� �� �ʱ�ȭ �׼�

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

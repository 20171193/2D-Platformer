using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    [Header("ÇÃ·¹ÀÌ¾î ¼ÒÁö °ñµå")]
    [SerializeField]
    float ownScore;
    public event UnityAction<float> OnOwnScoreChanged;
    public float OwnScore
    {
        set
        {
            ownScore = value;
            OnOwnScoreChanged?.Invoke(ownScore);
        }
        get { return ownScore; }
    }

    [Header("ÀåÅº ¼ö")]
    int maxBullet;
    public int MaxBullet { set { maxBullet = value; } get { return maxBullet; } }

    [Header("ÇÃ·¹ÀÌ¾î ÀÜÅº ¼ö")]
    [SerializeField]
    int ownBullet;
    public event UnityAction<int> OnOwnBulletChanged;   // ÀÜÅº ¼ö º¯°æ
    public event UnityAction OnOwnBulletExhausted; // ÃÑ¾Ë ¼ÒÁø
    public int OwnBullet
    {
        set
        {
            ownBullet = value;
            OnOwnBulletChanged?.Invoke(ownBullet);
        }
        get { return ownBullet; }
    }
}

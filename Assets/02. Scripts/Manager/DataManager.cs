using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    [Header("�÷��̾� ���� ���")]
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

    [Header("��ź ��")]
    int maxBullet;
    public int MaxBullet { set { maxBullet = value; } get { return maxBullet; } }

    [Header("�÷��̾� ��ź ��")]
    [SerializeField]
    int ownBullet;
    public event UnityAction<int> OnOwnBulletChanged;   // ��ź �� ����
    public event UnityAction OnOwnBulletExhausted; // �Ѿ� ����
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

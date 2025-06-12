using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlendermanEnemy", menuName = "Characters/SlendermanEnemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 15f;    //  �߰� ���� �Ÿ�
    [field: SerializeField] public float AttackRange { get; private set; } = 2.5f;          //  �Ҹ����� ��Ÿ�

    //[field:SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field:SerializeField][field:Range(0f,3f)] public float ForceTransitionTime { get; private set; }
    [field:SerializeField][field:Range(-10f, 10f)] public float Force { get; private set; }
    [field: SerializeField] public int Damage;
    [field: SerializeField][field:Range(0f,1f)] public float Scream_Start_TransitionTime { get; private set; }
    [field:SerializeField][field:Range(0f,1f)] public float Scream_End_TransitionTime { get; private set; }

    //  ���� ȿ���� �߰� ������
    [field:SerializeField] public AudioClip ScreamClip { get; private set; }
    [field:SerializeField] public float FearEffect { get; private set; }    //  ����) ���ŷ� ����
}

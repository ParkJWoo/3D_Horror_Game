using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "SlendermanEnemy", menuName = "Characters/SlendermanEnemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 5f;         //  추격 시작 거리
    [field: SerializeField] public float AttackRange { get; private set; } = 2.5f;              //  소리 공격 사거리
    [field: SerializeField] public float WalkSpeed { get; private set; } = 3f;                  //  추격 이동 거리
    [field: SerializeField] public int Damage { get; private set; } = 100;                      //  공격력

    [field: SerializeField][field:Range(0f,3f)] public float ForceTransformTime { get; private set; }
    [field: SerializeField][field:Range(0f,1f)] public float Scream_Start_TransitionTime { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Scream_End_TransitionTime { get; private set; }

    [field: SerializeField] public AudioClip ScreamClip; //  공격 효과음
}

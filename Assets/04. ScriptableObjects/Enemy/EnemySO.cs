using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlendermanEnemy", menuName = "Characters/SlendermanEnemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 5f;    //  추격 시작 거리
    [field: SerializeField] public float AttackRange { get; private set; } = 2.5f;          //  소리공격 사거리

    //[field:SerializeField] public PlayerGroundData GroundData { get; private set; }       //  플레이어가 어떻게 구현됐는가에 따라 넣을지말지 검토할 것
    [field:SerializeField][field:Range(0f,3f)] public float ForceTransitionTime { get; private set; }
    [field:SerializeField][field:Range(-10f, 10f)] public float Force { get; private set; }
    [field: SerializeField] public int Damage;
    [field: SerializeField][field:Range(0f,1f)] public float Scream_Start_TransitionTime { get; private set; }
    [field:SerializeField][field:Range(0f,1f)] public float Scream_End_TransitionTime { get; private set; }

    //  공포 효과용 추가 변수들
    [field:SerializeField] public AudioClip ScreamClip { get; private set; }
    [field:SerializeField] public float FearEffect { get; private set; }    //  예시) 정신력 감소
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlendermanSpawnTrigger : MonoBehaviour
{
    public GameObject slenderman;       //  에디터에서 슬랜더맨 프리팹 추가
    public float spawnDistance = 5f;    //  플레이어 기준 생성 거리
    public float minAngle = 60f;        //  플레이어 기준 양 옆 뒤쪽 등 랜덤 스폰을 위한 각도
    public float maxAngle = 120f;   

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        //  플레이어만 트리거
        if(!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            //  플레이어 Transform
            Transform playerTransform = other.transform;
            Vector3 playerPos = playerTransform.position;
            Vector3 forward = playerTransform.forward;

            //  스폰 위치 계산
            float angle = Random.Range(minAngle, maxAngle);

            //  렌덤하게 좌/우
            if(Random.value < 0.5f)
            {
                angle = -angle;
            }

            Vector3 spawnDir = Quaternion.Euler(0, angle, 0) * forward;
            Vector3 spawnPos = playerPos + spawnDir.normalized * spawnDistance;

            //  슬랜더맨 위치/회전 세팅
            slenderman.transform.position = spawnPos;

            //  수평 회전만 적용(y축)
            Vector3 lookAtPos = new Vector3(playerPos.x, slenderman.transform.position.y, playerPos.z);
            slenderman.transform.LookAt(lookAtPos);

            //  활성화
            slenderman.SetActive(true);

            //  Enemy FSM → 추격 상태 전환
            var enemy = slenderman.GetComponent<Enemy>();

            if(enemy != null && enemy.StateMachine != null)
            {
                enemy.StateMachine.ChangeState(enemy.StateMachine.ChasingState);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlendermanSpawner : MonoBehaviour
{
    public GameObject slenderman;

    [Header("Spawn Distance")]
    public float minSpawnDistance = 7f;
    public float maxSpawnDistance = 12f;

    [Header("Spawn Angle")]
    public float minAngle = 60f;
    public float maxAngle = 120f;

    [Header("NavMesh Sample")]
    public float sampleRadius = 3f;
    public int maxAttempts = 10;

    public void Spawn(Transform player)
    {
        if (slenderman == null || player == null)
        {
            Debug.LogWarning("[SlendermanSpawner] 슬렌더맨 또는 플레이어가 null입니다.");
            return;
        }

        Vector3 playerPos = player.position;
        Vector3 forward = player.forward;

        for (int i = 0; i < maxAttempts; i++)
        {
            float angle = Random.Range(minAngle, maxAngle);

            if (Random.value < 0.5f) 
            {
                angle = -angle;
            }

            Vector3 spawnDir = Quaternion.Euler(0, angle, 0) * forward;
            float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 targetPos = playerPos + spawnDir.normalized * spawnDistance;

            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            {
                float finalDistance = Vector3.Distance(playerPos, hit.position);
                if (finalDistance >= minSpawnDistance)
                {
                    // 위치 세팅
                    slenderman.transform.position = hit.position;
                    slenderman.transform.LookAt(new Vector3(playerPos.x, hit.position.y, playerPos.z));

                    // 슬렌더맨 상태 세팅
                    var enemy = slenderman.GetComponent<Enemy>();
                    if (enemy?.StateMachine != null)
                    {
                        // 강제 추격 설정
                        enemy.StateMachine.StartForcedChase();
                    }

                    slenderman.SetActive(true);

                    // 강제 상태 전이 보장 (OnEnable 보장 못할 시 대비)
                    if (enemy?.StateMachine?.CurrentState != enemy.StateMachine.ChasingState)
                    {
                        enemy.StateMachine.ChangeState(enemy.StateMachine.ChasingState);
                    }

                    Debug.Log("[SlendermanSpawner] 슬렌더맨 소환 및 추격 시작");
                    return;
                }
            }
        }

        Debug.LogWarning("[SlendermanSpawner] 유효한 NavMesh 위치를 찾지 못해 슬렌더맨 소환 실패");
    }

    public void DeactivateSlenderman()
    {
        if (slenderman != null && slenderman.activeSelf)
        {
            var enemy = slenderman.GetComponent<Enemy>();

            if (enemy?.StateMachine != null)
            {
                enemy.StateMachine.StopForcedChase();
                enemy.StateMachine.ChangeState(enemy.StateMachine.IdleState);
            }

            slenderman.SetActive(false);
            Debug.Log("[SlendermanSpawner] 슬렌더맨 비활성화 완료");
        }
    }
}
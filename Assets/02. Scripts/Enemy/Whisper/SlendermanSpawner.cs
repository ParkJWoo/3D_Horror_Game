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
        if (slenderman == null || player == null) return;

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
                    slenderman.transform.position = hit.position;
                    slenderman.transform.LookAt(new Vector3(playerPos.x, hit.position.y, playerPos.z));
                    slenderman.SetActive(true); // 여기서 OnEnable이 실행됨

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
            slenderman.SetActive(false);
        }
    }
}

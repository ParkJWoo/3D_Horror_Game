using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlendermanSpawner : MonoBehaviour
{
    public GameObject slenderman;
    public float spawnDistance = 5f;
    public float minAngle = 60f;
    public float maxAngle = 120f;

    public void Spawn(Transform player)
    {
        if (slenderman == null || player == null)
        {
            return;
        }

        Vector3 playerPos = player.position;
        Vector3 forward = player.forward;
        float angle = Random.Range(minAngle, maxAngle);

        if(Random.value < 0.5f)
        {
            angle = -angle;
        }

        Vector3 spawnDir = Quaternion.Euler(0, angle, 0) * forward;
        Vector3 spawnPos = playerPos + spawnDir.normalized * spawnDistance;

        slenderman.transform.position = spawnPos;
        slenderman.transform.LookAt(new Vector3(playerPos.x, slenderman.transform.position.y, playerPos.z));
        slenderman.SetActive(true);

        var enemy = slenderman.GetComponent<Enemy>();

        if (enemy?.StateMachine != null)
        {
            enemy.StateMachine.ChangeState(enemy.StateMachine.ChasingState);
        }
    }

    public void DeactivateSlenderman()
    {
        if (slenderman != null && slenderman.activeSelf)
        {
            slenderman.SetActive(false);
        }
    }
}

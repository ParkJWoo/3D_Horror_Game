using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDie;

    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        IsDead = false;
    }

    // 즉사 로직: 데미지 받으면 바로 사망 처리!
    public void Kill()
    {
        if (!IsDead)
        {
            IsDead = true;
            OnDie?.Invoke();
            Debug.Log("플레이어 즉사!");
        }
    }
}

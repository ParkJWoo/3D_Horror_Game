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

    // ��� ����: ������ ������ �ٷ� ��� ó��!
    public void Kill()
    {
        if (!IsDead)
        {
            IsDead = true;
            OnDie?.Invoke();
            Debug.Log("�÷��̾� ���!");
        }
    }
}

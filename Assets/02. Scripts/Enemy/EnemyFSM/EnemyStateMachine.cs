using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<Enemy>
{
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChasingState ChasingState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public Health Target { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        IdleState = new EnemyIdleState(this);
        ChasingState = new EnemyChasingState(this);
        AttackState = new EnemyAttackState(this);

        MovementSpeed = 1f;

        //MovementSpeed = Context.Data.GroundData.BaseSpeed;
        //RotationDamping = Context.Data.GrondData.BaseRotationDamping;
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {
        //Debug.Log("[FSM] Update »£√‚");

        if (Target.IsDead && !(CurrentState is EnemyIdleState))
        {
            ChangeState(IdleState);
            return;
        }

        UpdateState();
    }

    private void FixedUpdate()
    {
        PhysicsUpdateState();
    }
}

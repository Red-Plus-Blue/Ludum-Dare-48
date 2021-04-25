using UnityEngine;

using System;

using Random = UnityEngine.Random;

public class EnemyComponent : MonoBehaviour
{
    [SerializeField]
    protected float _moveSpeed = 2.0f;

    protected Action _state;

    protected BlockComponent _target;
    protected float _nextMineTime;

    private void Awake()
    {
        var axis = (Random.Range(0, 2) == 0) ? transform.up : transform.right;
        var sign = (Random.Range(0, 2) == 0) ? 1 : -1;
        transform.up = axis * sign;
        
        _state = StatePatrol;
    }

    private void FixedUpdate()
    {
        _state?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerControllerComponent>();
        if(!player) { return; }
        player.Die();
        Debug.Log("Game Over");
    }

    protected void StatePatrol()
    {
        var hit = Physics2D.Raycast(transform.position, transform.up, 0.5f);
        if(hit.collider)
        {
            var exit = hit.collider.GetComponent<ExitComponent>();
            if(exit)
            {
                TurnAround();
                return;
            }

            var elevator = hit.collider.GetComponent<ElevatorComponent>();
            if(elevator)
            {
                TurnAround();
                return;
            }


            var block = hit.collider.GetComponent<BlockComponent>();
            if(block)
            {
                if(block.Impassable)
                {
                    TurnAround();
                    return;
                }
                _state = StateMine;
                _target = block;
            }
            return;
        }

        transform.position += transform.up * _moveSpeed * Time.fixedDeltaTime;
    }

    protected void StateMine()
    {
        if(Time.time > _nextMineTime)
        {
            if(!_target)
            {
                TurnAround();
                _state = StatePatrol;
                return;
            }

            _target.TakeDamge(3, false);
            _nextMineTime = Time.time + 0.3f;
        }
    }

    protected void TurnAround()
    {
        transform.up = -transform.up;
    }

}

using UnityEngine;

using System;

public class BlockComponent : MonoBehaviour
{
    public int Money;
    public float Fuel;
    
    [SerializeField]
    protected bool _impassable;

    protected Block _block;

    public void TakeDamge(int amount)
    {
        if(_impassable) { return; }
        if(_block == null)
        {
            _block = new Block();
            _block.OnDeath += OnBlockDeath;
        }
        _block.TakeDamage(amount);

        if(!_block.Dead)
        {
            transform.localScale = (0.3f + (0.7f * (Convert.ToSingle(_block.Health) / _block.HealthMax))) * Vector3.one;
        }
    }

    protected void OnBlockDeath()
    {
        var player = FindObjectOfType<PlayerControllerComponent>();
        player.AddMoney(Money);
        player.AddFuel(Fuel);
        Destroy(gameObject);
    }

}

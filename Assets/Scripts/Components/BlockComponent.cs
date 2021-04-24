using UnityEngine;

using System;

using Random = UnityEngine.Random;

public class BlockComponent : MonoBehaviour
{
    public int Money;
    public float Fuel;
    
    [SerializeField]
    protected bool _impassable;
    [SerializeField]
    protected Sprite _damagedSprite;

    protected SpriteRenderer _renderer;
    protected Block _block;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.flipX = Random.Range(0, 2) == 0;
        _renderer.flipY = Random.Range(0, 2) == 0;
    }

    public void TakeDamge(int amount)
    {
        if(_impassable) { return; }
        if(_block == null)
        {
            TakeFirstDamage();
        }
        _block.TakeDamage(amount);

        if(!_block.Dead)
        {
            transform.localScale = (0.3f + (0.7f * (Convert.ToSingle(_block.Health) / _block.HealthMax))) * Vector3.one;
        }
    }

    protected void TakeFirstDamage()
    {
        _renderer.flipX = false;
        _renderer.flipY = false;
        _renderer.sprite = _damagedSprite;
        _block = new Block();
        _block.OnDeath += OnBlockDeath;
    }

    protected void OnBlockDeath()
    {
        var player = FindObjectOfType<PlayerControllerComponent>();
        player.AddMoney(Money);
        player.AddFuel(Fuel);
        Destroy(gameObject);
    }

}

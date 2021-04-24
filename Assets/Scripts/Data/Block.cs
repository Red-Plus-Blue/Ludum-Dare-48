using System;

public class Block
{
    public Action OnDeath;
    public int HealthMax { get; protected set; } = 10;
    public int Health { get; protected set; } = 10;
    public bool Dead { get; protected set; }

    public void TakeDamage(int amount) {
        Health -= amount;
        if(Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(Dead) { return; }
        Dead = true;
        OnDeath?.Invoke();
    }
}

using UnityEngine;

using System.Linq;
using System.Collections.Generic;

public class PlayerControllerComponent : MonoBehaviour
{
    public bool InputLocked;

    [SerializeField]
    protected float _speed = 3f;
    [SerializeField]
    protected float _rotationSpeed = 90f;
    [SerializeField]
    protected GameObject _rockEffect;
    [SerializeField]
    protected List<ParticleSystem> _drillParticles;

    protected float _drillDelay = 0.15f;

    protected float _nextDrillTime;
    protected bool _outOfFuel;

    protected float _currentFuel = 100f;
    protected float _maxFuel = 100f;

    protected int _money;

    protected Rigidbody2D _rigidbody2D;
    protected UIComponent _ui;

    public void AddMoney(int amount)
    {
        _money += amount;
        _ui.SetMoney(_money);
    }

    public void AddFuel(float amount)
    {
        _currentFuel = Mathf.Min(_maxFuel, _currentFuel + amount);
        _ui.SetFuelLevel(_currentFuel / _maxFuel);
    }

    private void Awake()
    {
        _ui = FindObjectOfType<UIComponent>();
    }

    private void Update()
    {
        if(!_outOfFuel && (_currentFuel <= 0f))
        {
            Debug.Log("Out of fuel");
            _outOfFuel = true;
        }
    }

    private void FixedUpdate()
    {
        if(InputLocked || _outOfFuel) { return; }

        var horizontal = -Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if((vertical != 0f) || (horizontal != 0f))
        {
            _currentFuel -= Time.fixedDeltaTime;
            _ui.SetFuelLevel(_currentFuel / _maxFuel);
        }

        transform.position += transform.up * vertical * _speed * Time.fixedDeltaTime;
        transform.rotation *= Quaternion.Euler(0f, 0f, horizontal * _rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var block = other.GetComponent<BlockComponent>();
        if (!block) { return; }

        _drillParticles
            .Where(particle => particle.isStopped)
            .ToList()
            .ForEach(particle => particle.Play());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var block = other.GetComponent<BlockComponent>();
        if(!block) { return; }

        _drillParticles
            .Where(particle => particle.isStopped)
            .ToList()
            .ForEach(particle => particle.Play());

        if (Time.time >= _nextDrillTime)
        {
            block.TakeDamge(2, true);
            var effect = Instantiate(_rockEffect, block.transform);
            Destroy(effect, 0.6f);
            _nextDrillTime = Time.time + _drillDelay;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var block = other.GetComponent<BlockComponent>();
        if (!block) { return; }

        _drillParticles.ForEach(particle => particle.Stop());
    }

    public void Die()
    {
        gameObject.SetActive(false);
        InputLocked = true;
    }

}

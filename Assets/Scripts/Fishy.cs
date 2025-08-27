using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fishy : MonoBehaviour
{
    public Rigidbody Body;
    public InputActionReference JumpAction;
    public float Gravity = -10f;
    public float Speed = 20f;
    public float JumpForce = 5f;
    public float CeilingHeight = 10f;
    public Transform AnimatedChild;
    public TrailRenderer Trail;
    
    private GameManager _gameManager;
    private Vector3 _velocity;
    private bool _isDead = false;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        // Check for jump input
        if (JumpAction.action.triggered)
        {
            _velocity.y = JumpForce;
        }
        
        AnimatedChild.rotation = Quaternion.LookRotation(_velocity);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_isDead) return;
        if (!_gameManager.IsGameRunning()) return;
        
        // Apply forward movement
        _velocity.z = Speed;
        
        // Apply gravity
        _velocity.y += Gravity * Time.fixedDeltaTime;
        
        // Move the fishy
        Body.MovePosition(transform.position + _velocity * Time.fixedDeltaTime);

        // Clamp the position to prevent going too high
        if(transform.position.y > CeilingHeight)
        {
            Vector3 pos = transform.position;
            pos.y = CeilingHeight;
            transform.position = pos;
            _velocity.y = 0;
        }
        
        // Die if it falls below the ground
        if (transform.position.y < 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Die();
    }

    private void Die()
    {
        Debug.Log("Fishy has died!");
        _isDead = true;
        _gameManager.OnPlayerDeath();
    }

    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void Reset()
    {
        transform.position = _startPosition;
        AnimatedChild.rotation = Quaternion.identity;
        _velocity = Vector3.zero;
        _isDead = false;
        Trail.Clear();
    }
}

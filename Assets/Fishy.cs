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
    
    private GameManager _gameManager;
    private Vector3 _velocity;
    private bool _isDead = false;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_isDead) return;
        
        // Apply forward movement
        _velocity.z = Speed;
        
        // Apply gravity
        _velocity.y += Gravity * Time.fixedDeltaTime;

        // Check for jump input
        if (JumpAction.action.WasPressedThisFrame())
        {
            Debug.Log("key pressed");
            _velocity.y = JumpForce;
        }

        // Move the fishy
        //transform.position += _velocity * Time.fixedDeltaTime;
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
}

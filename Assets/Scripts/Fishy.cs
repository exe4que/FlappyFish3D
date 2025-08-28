using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fishy : MonoBehaviour
{
    #region NotReset
    public Rigidbody Body;
    public InputActionReference JumpAction;
    public float Gravity = -10f;
    public float Speed = 20f;
    public float JumpForce = 5f;
    public float CeilingHeight = 10f;
    public Transform AnimatedChild;
    public TrailRenderer Trail;
    public AudioSource JumpSound;
    public AudioSource DeathSound;
    
    private GameManager _gameManager;
    private Vector3 _velocity;
    private bool _isDead = false;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }
    #endregion
    private void Update()
    {
        #region NotReset
        if (_isDead) return;
        #endregion
        if (!_gameManager.IsGameRunning()) return;
        #region NotReset
        // Check for jump input
        if (JumpAction.action.triggered)
        {
            _velocity.y = JumpForce;
            JumpSound.Play();
        }
        
        AnimatedChild.rotation = Quaternion.LookRotation(_velocity);
        #endregion
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        #region NotReset
        if (_isDead) return;
        #endregion
        if (!_gameManager.IsGameRunning()) return;
        #region NotReset
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
        #endregion
    }
    #region NotReset
    private void OnCollisionEnter(Collision other)
    {
        Die();
    }
    #endregion
    #region UI
    private void Die()
    {
        Debug.Log("Fishy has died!");
        _isDead = true;
        DeathSound.Play();
        _gameManager.OnPlayerDeath();
    }

    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    #endregion
    public void Reset()
    {
        transform.position = _startPosition;
        AnimatedChild.rotation = Quaternion.identity;
        _velocity = Vector3.zero;
        _isDead = false;
        Trail.Clear();
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public GameObject[] Obstacles;
    public int ObstaclesCountAtSameTime = 3;
    public float StartingSpawnDistance = 10f;
    public float ObstaclesSeparation = 5f;
    public float GameInactiveDuration = 3f;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestScoreText;
    public TextMeshProUGUI CountdownText;
    public Fishy Fishy;
    public InputActionReference GoToMenuAction;
    
    private Queue<GameObject> _obstacles = new ();
    private float _lastZSpawnPosition;
    private int _score;
    private int _bestScore;
    private float _gameStartTime = float.MaxValue;
    private float _countdownTime = 0f;

    private void Start()
    {
        Fishy.SetGameManager(this);
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (_obstacles.Count < ObstaclesCountAtSameTime)
        {
            SpawnObstacle();
        }
        
        // Remove obstacles that are too far behind the player
        if (Player.position.z > _obstacles.Peek().transform.position.z + 1)
        {
            GameObject obstacle = _obstacles.Dequeue();
            Destroy(obstacle);
            _score++;
            ScoreText.text = "Puntaje: " + _score;
        }
        
        //update countdown
        if (_countdownTime > 0)
        {
            CountdownText.text = Mathf.CeilToInt(_countdownTime).ToString();
            _countdownTime -= Time.deltaTime;
            if (_countdownTime <= 0)
            {
                CountdownText.text = "";
            }
        }
        
        if (GoToMenuAction.action.triggered)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
    
    private void SpawnObstacle()
    {
        GameObject obstaclePrefab = Obstacles[Random.Range(0, Obstacles.Length)];
        Vector3 spawnPosition = new Vector3(0, 0, _lastZSpawnPosition + ObstaclesSeparation);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        _obstacles.Enqueue(obstacle);
        _lastZSpawnPosition = spawnPosition.z;
    }

    public void OnPlayerDeath()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            PlayerPrefs.Save();
        }

        Reset();
    }

    private void Reset()
    {
        // Reset player
        Fishy.Reset();
        
        // Clear obstacles
        while (_obstacles.Count > 0)
        {
            GameObject obstacle = _obstacles.Dequeue();
            Destroy(obstacle);
        }
        
        // Reset spawn position and score
        _lastZSpawnPosition = transform.position.z + StartingSpawnDistance;
        _score = 0;
        ScoreText.text = "Puntaje: " + _score;
        BestScoreText.text = "Mejor Puntaje: " + _bestScore;
        _gameStartTime = Time.time + GameInactiveDuration;
        _countdownTime = GameInactiveDuration;
    }

    public bool IsGameRunning()
    {
        return Time.time >= _gameStartTime;
    }
}

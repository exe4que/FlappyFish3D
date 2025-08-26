using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public GameObject[] Obstacles;
    public int ObstaclesCountAtSameTime = 3;
    public float StartingSpawnDistance = 10f;
    public float ObstaclesSeparation = 5f;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestScoreText;
    public Fishy Fishy;
    
    private Queue<GameObject> _obstacles = new ();
    private float _lastZSpawnPosition;
    private int _score;
    private int _bestScore;

    private void Start()
    {
        _lastZSpawnPosition = transform.position.z + StartingSpawnDistance;
        Fishy.SetGameManager(this);
        _score = 0;
        ScoreText.text = "Puntaje: " + _score;
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        BestScoreText.text = "Mejor Puntaje: " + _bestScore;
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
        }
    }
}

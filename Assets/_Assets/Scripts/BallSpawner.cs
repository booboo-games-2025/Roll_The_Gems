using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 2.1f;
    Coroutine _spawnRoutine;
    public List<Ball> _ballsList;
    [SerializeField] private UpgradeUi upgradeUi;
    
    private bool _isSpawning;
    private int ballSpawnerIndex;
    
    private int _currentBallCount;
    public int CurrentBallCount
    {
        get
        { 
           return _currentBallCount;
        } 
        set
        {
            _currentBallCount = value;
            upgradeUi.HandleBallCount(value);
        } 
    }

    private void Awake()
    {
        for (int i = 0; i < _ballsList.Count; i++)
        {
            _ballsList[i]._OnBallDisable = DecreaseBallCount;
        }
    }

    public void SetBallIndex(int index)
    {
        ballSpawnerIndex = index;
        foreach (Ball ball in _ballsList)
        {
            ball.BallIndex = index;
        }
    }

    public void Spawn()
    {
        if(_isSpawning) return;
        
        _isSpawning = true;
        _spawnRoutine = StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            Ball newBall = GetActiveBall();
            float spawnSpeedUpgradeVal = spawnDelay * (PowerupsManager.instance.GetLevel(ballSpawnerIndex, PowerType.BallCreationSpeed)/10f);
            if (newBall != null)
            {
                newBall.gameObject.SetActive(true);
                newBall.Init();
                CurrentBallCount++;
                upgradeUi.HandleBallFill(spawnDelay - spawnSpeedUpgradeVal);
            }
            yield return new WaitForSeconds(spawnDelay - spawnSpeedUpgradeVal);
        }
    }

    public Ball GetActiveBall()
    {
        for (int i = 0; i < _ballsList.Count; i++)
        {
            if (!_ballsList[i].isActive)
            {
                return _ballsList[i];
            }
        }
        return null;
    }

    public void StopSpawning()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
        foreach (Ball ball in _ballsList)
        {
            ball.isActive = false;
        }
        CurrentBallCount = 0;
        _isSpawning = false;
    }

    void DecreaseBallCount()
    {
        CurrentBallCount--;
        if (CurrentBallCount < 0)
        {
            CurrentBallCount = 0;
        }
    }
}

public class BallData
{
    public double SpawnDelay;
    public int CriticalChance;
    public float Speed;
    public float Multiplier;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    Coroutine _spawnRoutine;
    public List<Ball> _ballsList;
    [SerializeField] private Tab tabUi;
    
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
            tabUi.HandleBallCount(value);
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
        if(_isSpawning || PowerupsManager.instance.GetLevel(ballSpawnerIndex,UpgradeType.Income) < 1) return;
        
        _isSpawning = true;
        _spawnRoutine = StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            Ball newBall = GetActiveBall();
            float finalDelay = (float)PowerupsManager.instance.GetValue(ballSpawnerIndex, UpgradeType.BallCreationSpeed);
            if (TwoXBallCreationRv.IsActive)
            {
                finalDelay /= 2;
            }
            finalDelay = Mathf.Clamp(finalDelay, 0.5f, 10);
            if (newBall != null)
            {
                newBall.gameObject.SetActive(true);
                newBall.Init();
                CurrentBallCount++;
                tabUi.HandleBallFill(finalDelay);
                Achievements.OnAchievementsUpdated?.Invoke(1,AchievementType.CreateBalls);
            }
            yield return new WaitForSeconds(finalDelay);
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

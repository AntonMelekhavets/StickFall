﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public enum PersonageType
{
    None = 0,
    Default = 1,
}

public enum StopPlayerAt
{
    None = 0,
    Platform = 1,
    Stick = 2,
}

public class Player : BaseObject
{
    #region Fields
    
    [SerializeField] PersonageType _type;

    StopPlayerAt stopPlayerAt;

    bool allowToMove = true;

    #endregion

    #region Properties

    public PersonageType Type
    {
        get
        {
            return _type;
        }
    }

    #endregion


    #region Unity lifecycle

    private void OnEnable()
    {
        GameManager.OnPlayerStartMove += GameManager_OnPlayerStartMove;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerStartMove -= GameManager_OnPlayerStartMove;
    }

    #endregion


    #region Public methods

    public void Respawn()
    {
        transform.localPosition = Vector3.zero;
    }

    #endregion
    

    #region Event handlers

    private void GameManager_OnPlayerStartMove()
    {
        float speed = 900;

        LevelPlatform nextPlatform = LevelManager.Instance.LastBlockForUser;
        Vector3 endStickPosition = LevelManager.Instance.CurrentBlockForUser.StickController.EndStickPosition;

        if (nextPlatform.transform.position.x - nextPlatform.Width * 0.5f > endStickPosition.x)
        {
            Debug.Log("Nedohod");
            stopPlayerAt = StopPlayerAt.Stick;
        }
        else if (nextPlatform.transform.position.x + nextPlatform.Width * 0.5f < endStickPosition.x)
        {
            Debug.Log("Perehod");
            stopPlayerAt = StopPlayerAt.Stick;
        }
        else
        {
            Debug.Log("Perfect");
            stopPlayerAt = StopPlayerAt.Platform;
        }
        float distance = 0f;
        switch (stopPlayerAt)
        {
            case StopPlayerAt.Platform:
                distance = Mathf.Abs((transform.position - nextPlatform.transform.position).x);
                break;

            case StopPlayerAt.Stick:
                distance = Mathf.Abs((transform.position - endStickPosition).x);
                break;

        }
        

        float time = distance / speed;

        allowToMove = false;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Vector2 position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        transform.DOMove(position, time).OnComplete(() =>
        {
            if(stopPlayerAt == StopPlayerAt.Stick)
            {
                GameManager.Instance.TryKillPlayer();
            }
            else
            {
                allowToMove = true;

                GameManager.Instance.PlayerStoped();
            }

        });
    }

    #endregion
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    #region Fields

    [SerializeField] GameObject _platformPrefab;

    List<LevelPlatform> _platformsOnScreen;

    [SerializeField] AnimationCurve _fallStickCurve;
    [SerializeField] float _fallStickDuaration;
    #endregion


    #region Properties

    public LevelPlatform LastBlockForUser
    {
        get
        {
            return _platformsOnScreen[_platformsOnScreen.Count - 1];
        }
    }
    public LevelPlatform CurrentBlockForUser
    {
        get
        {
            return _platformsOnScreen[_platformsOnScreen.Count - 2];
        }
    }

    public AnimationCurve FallStickCurve
    {
        get
        {
            return _fallStickCurve;
        }
    }
    public float FallStickDuaration
    {
        get
        {
            return _fallStickDuaration;
        }
    }


    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        _platformsOnScreen = new List<LevelPlatform>();
    }

    private void OnEnable()
    {

    }

    void Update()
    {

    }

    void Start()
    {
    }


    #endregion


    #region Public methods

    public void GenerateStartLevel()
    {
        ClearLevel();

        GenerateFirstPlatform();

        float distance = Random.Range(50f, 300f);
        float width = Random.Range(150f, 400f);

        GenerateNextPlatform(distance, width);

    }

    #endregion


    #region Private methods

    private void GenerateFirstPlatform()
    {
        float width = 200;
        LevelPlatform platfom = ObjectCreator.CreateObject(_platformPrefab, transform).GetComponent<LevelPlatform>();
        platfom.Init(width);
        platfom.IsActive = true;
        _platformsOnScreen.Add(platfom);
    }

    public void GenerateNextPlatform(float distance, float width)
    {
        var lastPlatform = _platformsOnScreen[_platformsOnScreen.Count - 1];

        lastPlatform.IsActive = true;

        LevelPlatform platfom = ObjectCreator.CreateObject(_platformPrefab, transform).GetComponent<LevelPlatform>();
        platfom.Init(width);
        _platformsOnScreen.Add(platfom);

        float newXCoordinate = lastPlatform.Width * 0.5f + lastPlatform.transform.localPosition.x + distance + width * 0.5f;

        platfom.transform.localPosition = new Vector3(newXCoordinate, platfom.transform.localPosition.y, platfom.transform.localPosition.z);

        if(_platformsOnScreen.Count > 2)
        {
            _platformsOnScreen[0].gameObject.ReturnToPool();
            _platformsOnScreen.Remove(_platformsOnScreen[0]);
        }
    }

    void ClearLevel()
    {
        _platformsOnScreen.ForEach((elem) =>
        {
            elem.gameObject.ReturnToPool();
        });

        _platformsOnScreen.Clear();
    }

    #endregion
    
}

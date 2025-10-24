using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public GameObject CurrentPoint;
    private TMP_Text _currentPointText;

    public GameObject CurrentCircle;
    private TMP_Text _CurrentCircleText;

    public GameObject FinishMessage;

    public GameObject Player;
    private CarController _playerController;


    [SerializeField]
    private List<GameObject> _points = new();
    [SerializeField]
    private bool _isCircle;
    [SerializeField]
    private int _countCircle;

    private int _currentPoint;
    private int _currentCircle;

    void Start()
    {
        _playerController = Player.GetComponent<CarController>();
        _playerController.IsControl = true;

        _currentPointText = CurrentPoint.GetComponent<TMP_Text>();
        _CurrentCircleText = CurrentCircle.GetComponent<TMP_Text>();

        CurrentPoint.SetActive(true);
        CurrentCircle.SetActive(true);
        FinishMessage.SetActive(false);

        foreach (var point in _points)
        {
            point.SetActive(false);
        }
        _currentPoint = 0;
        _points[_currentPoint].SetActive(true);
    }

    void Update()
    {
        UpdateUI();
    }

    public void PointActivated()
    {
        _points[_currentPoint].SetActive(false);
        _currentPoint++;
        if (_currentPoint == _points.Count)
        {
            _currentPoint = 0;
            if (_isCircle)
            {
                _currentCircle++;
                if (_currentCircle > _countCircle)
                {
                    Finished();
                }
            }
            else
            {
                Finished();
            }
        }
        _points[_currentPoint].SetActive(true);
    }

    private void Finished()
    {
        CurrentPoint.SetActive(false);
        CurrentCircle.SetActive(false);
        FinishMessage.SetActive(true);

        _playerController.IsControl = false;
    }
    
    void UpdateUI()
    {
        _currentPointText.text = _currentPoint.ToString() + "/" + (_points.Count).ToString();
        _CurrentCircleText.text = _currentCircle.ToString() + "/" + _countCircle.ToString();
    }
}

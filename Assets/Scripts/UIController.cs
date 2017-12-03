using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text ArrowText;
    public Text ClockText;
    public Text CollectedText;
    public Text RemainingText;

    public GameObject PainterReference;
    private DirectionPainter _painter;

    void Awake()
    {
        _painter = PainterReference.GetComponent<DirectionPainter>();

        UpdateTexts();
    }

    void Start() 
    {
        
    }
    
    void Update()
    {
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        ArrowText.text = "x" + _painter.Storage;

        // Timer stuff
        var time = GameManager.Instance.LevelTimer;
        var minutes = Mathf.FloorToInt(time / 60f);
        var seconds = Mathf.FloorToInt(time % 60f);
        var hundreds = Mathf.FloorToInt((time - minutes*60f - seconds)*100);

        ClockText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + "." + hundreds.ToString("00");

        // Goal text
        CollectedText.text = GameManager.Instance.Collected.ToString();
        RemainingText.text = GameManager.Instance.RequiredAmount.ToString();
    }

    public void ArrowsResetButtonClicked()
    {
        _painter.Reset();
    }
}

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text ArrowText;
    public Text ClockText;
    public Text CollectedText;
    public Text RemainingText;

    public Image FadeIn;
    public Text FadeInText;
    private float _fadeInTimer;

    public GameObject PainterReference;
    private DirectionPainter _painter;

    private string[] _numberStringCache;

    void Awake()
    {
        FadeIn.enabled = true;

        _painter = PainterReference.GetComponent<DirectionPainter>();

        // Generate string cahces, because ToString generates garbage
        // We only need to generate up to MaxStorage, Required, or 99 (for hundreds)
        var maxCache = Mathf.Max(GameManager.Instance.RequiredAmount, GameManager.Instance.ArrowStorage, 99);
        _numberStringCache = new string[maxCache + 1];
        for (var i = 0; i <= maxCache; i++)
        {
            _numberStringCache[i] = i.ToString();
        }

        UpdateTexts();
    }

    void Start() 
    {
        
    }
    
    void Update()
    {
        UpdateTexts();

        var dt = Time.deltaTime;

        _fadeInTimer += dt;

        var ratio = 1f - Mathf.Clamp(_fadeInTimer / 3f, 0f, 1f);
        FadeIn.color = new Color(0f, 0f, 0f, ratio);

        if (_fadeInTimer < 1f)
        {
            var textRatio = Mathf.Clamp(_fadeInTimer / 1f, 0f, 1f);
            FadeInText.color = new Color(1f, 1f, 1f, textRatio);
        }
        else
        {
            var textRatio = 1f - Mathf.Clamp((_fadeInTimer - 1f) / 3f, 0f, 1f);
            FadeInText.color = new Color(1f, 1f, 1f, textRatio);
        }

        

        if (_fadeInTimer > 4f)
        {
            FadeIn.enabled = false;
            FadeInText.enabled = false;
            _fadeInTimer = 6f; //hackety hack
            GameManager.Instance.LevelLoadingDone = true;
        }

        if (GameManager.Instance.LevelCleared && _fadeInTimer > 5f)
        {
            _fadeInTimer = 0f;
        }

        if (GameManager.Instance.LevelCleared)
        {
            FadeIn.enabled = true;
            FadeInText.enabled = true;

            ratio = Mathf.Clamp(_fadeInTimer / 1.5f, 0f, 1f);
            FadeIn.color = new Color(0f, 0f, 0f, ratio);
            FadeInText.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void UpdateTexts()
    {
        ArrowText.text = "x" + _numberStringCache[_painter.Storage];

        // Timer stuff
        var time = GameManager.Instance.LevelTimer;
        var minutes = Mathf.FloorToInt(time / 60f);
        var seconds = Mathf.FloorToInt(time % 60f);
        var hundreds = Mathf.FloorToInt((time - minutes*60f - seconds)*100);

        var text = "";

        if (minutes < 10)
        {
            text += _numberStringCache[0];
        }

        text += _numberStringCache[minutes] + ":";

        if (seconds < 10)
        {
            text += _numberStringCache[0];
        }

        text += _numberStringCache[seconds] + ".";

        if (hundreds < 10)
        {
            text += _numberStringCache[0];
        }

        text += _numberStringCache[hundreds];

        ClockText.text = text;

        // Goal text
        CollectedText.text = _numberStringCache[GameManager.Instance.Collected];
        RemainingText.text = _numberStringCache[GameManager.Instance.RequiredAmount];
    }

    public void ArrowsResetButtonClicked()
    {
        if (GameManager.Instance.LevelCleared) return;

        _painter.Reset();
    }
}

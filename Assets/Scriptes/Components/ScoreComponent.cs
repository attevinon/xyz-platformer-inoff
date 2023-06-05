using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreComponent : MonoBehaviour
{
    private static int _score;
    [SerializeField] public Text scoreText;
    private void Start()
    {
        _score = 0;
    }
    public void IncreaseScore(int points)
    {
        _score += points;
        Debug.Log("Score: " + _score);

        if (scoreText != null)
            scoreText.text = _score.ToString();
    }

}

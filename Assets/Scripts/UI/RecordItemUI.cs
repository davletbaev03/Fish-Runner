using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;

    public void Setup(int place, ScoreEntry data)
    {
        placeText.text = place.ToString();
        nameText.text = data.name.ToString();
        scoreText.text = data.score.ToString();
        distanceText.text = $"{data.distance:F1}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

[System.Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
    public float distance;

    public ScoreEntry() { }

    public ScoreEntry(string name, int score, float distance)
    {
        this.name = name;
        this.score = score;
        this.distance = distance;
    }
}

[System.Serializable]
public class ScoreData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
    public ScoreEntry playerData = null;
}

public class RecordsManager : MonoBehaviour
{
    private string _filePath;
    public ScoreData scoreData;

    private string FILE_NAME = "scores.json";
    private const int MAX_SCORES = 5;

    private ISaveLoadService _saveLoadService;
    public List<ScoreEntry> GetScores
    {
        get { return scoreData.scores; }
        set { scoreData.scores = value; }
    }

    private void Start()
    {
        _saveLoadService = ServiceLocator.Get<ISaveLoadService>();

        _filePath = Path.Combine(Application.persistentDataPath, "scores.json");
        //Clear();
        Load();
        InitPlayer();

        EventBus.OnSessionStarted?.Invoke();
        EventBus.OnRunStarted?.Invoke();
    }

    private void InitPlayer()
    {
        if (string.IsNullOrEmpty(scoreData.playerData.name))
        {
            scoreData.playerData.name = GeneratePlayerName();
            scoreData.playerData.score = 0;
            scoreData.playerData.distance = 0;
            Save();
        }
    }

    private string GeneratePlayerName()
    {
        int index = 0;

        string resultName = "Player";
        string currentName = "Player";

        do
        {
            index++;
            currentName = $"{resultName}_{index}";
        } 
        while (scoreData.scores.Any(s => s.name == currentName));

        resultName = currentName;
        return resultName;
    }

    public bool TrySetNewPersonalRecord(string name, int score, float distance)
    {
        if (scoreData.playerData == null || score > scoreData.playerData.score)
        {
            scoreData.playerData.score = score;
            scoreData.playerData.distance = distance;
            Save();
            return true;
        }

        return false;
    }

    public void AddScore(string name, int score, float distance)
    {
        ScoreEntry existing = scoreData.scores
            .FirstOrDefault(s => s.name == name);

        if (existing != null)
        {
            if (score > existing.score)
            {
                existing.score = score;
                existing.distance = distance;
            }
        }
        else
        {
            scoreData.scores.Add(new ScoreEntry(name, score, distance));
        }

        scoreData.scores = scoreData.scores
            .OrderByDescending(s => s.score)
            .Take(MAX_SCORES)
            .ToList();

        Save();
    }

    private void Save()
    {
        _saveLoadService.Save(scoreData, FILE_NAME);
    }

    private void Load()
    {
        if (!_saveLoadService.TryLoad(FILE_NAME, out scoreData))
        {
            scoreData = new ScoreData();
            Save();
        }
    }

    public void Clear()
    {
        scoreData = new ScoreData();
        Save();
    }
}

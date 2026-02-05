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
    public ScoreData _scoreData;

    private const int MAX_SCORES = 5;


    public List<ScoreEntry> GetScores
    {
        get { return _scoreData.scores; }
        set { _scoreData.scores = value; }
    }

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "scores.json");
        //Clear();
        Load();
        InitPlayer();
    }

    private void InitPlayer()
    {
        if (string.IsNullOrEmpty(_scoreData.playerData.name))
        {
            _scoreData.playerData.name = GeneratePlayerName();
            _scoreData.playerData.score = 0;
            _scoreData.playerData.distance = 0f;
            Save();
        }
    }

    private string GeneratePlayerName()
    {
        int index = 0;

        string resultName = "Player";

        do
        {
            index++;
            resultName = $"{resultName}_{index}";
        } 
        while (_scoreData.scores.Any(s => s.name == resultName));

        return resultName;
    }

    public bool TrySetNewPersonalRecord(string name, int score, float distance)
    {
        if (_scoreData.playerData == null || score > _scoreData.playerData.score)
        {
            _scoreData.playerData.score = score;
            _scoreData.playerData.distance = distance;
            Save();
            return true;
        }

        return false;
    }

    public void AddScore(string name, int score, float distance)
    {
        ScoreEntry existing = _scoreData.scores
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
            _scoreData.scores.Add(new ScoreEntry(name, score, distance));
        }

        _scoreData.scores = _scoreData.scores
            .OrderByDescending(s => s.score)
            .Take(MAX_SCORES)
            .ToList();

        Save();
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(_scoreData, true);
        File.WriteAllText(_filePath, json);
    }

    private void Load()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _scoreData = JsonUtility.FromJson<ScoreData>(json);
        }
        else
        {
            Clear();
        }
    }

    public void Clear()
    {
        _scoreData = new ScoreData();
        Save();
    }
}

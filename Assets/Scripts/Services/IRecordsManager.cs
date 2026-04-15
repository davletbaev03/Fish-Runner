using FishRunner.Services;
public interface IRecordsManager
{
    public ScoreData ScoreData { get; }

    public bool TrySetNewPersonalRecord(string name, int score, float distance);

    public void AddScore(string name, int score, float distance);
}

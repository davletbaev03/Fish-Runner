public interface ISaveLoadService
{
    public void Save<T>(T data, string fileName) where T : class;
    public bool TryLoad<T>(string fileName, out T data) where T : class;
    public T LoadOrCreate<T>(string fileName) where T : class, new();

    public void Delete(string fileName);
}

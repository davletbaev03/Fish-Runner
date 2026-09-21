using Cysharp.Threading.Tasks;

namespace FishRunner.Services
{
    public interface ILoadingService
    {
        UniTask LoadScene(string sceneName);
    }
}
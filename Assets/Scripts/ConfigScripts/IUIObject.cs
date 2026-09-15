namespace FishRunner.UI
{
    public interface IUIObject
    {
        public string Id { get; }
        public void Show();
        public void Hide();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using FishRunner.Services;
using FishRunner.Configs;
using Zenject;

namespace FishRunner.UI
{
    public class WindowRecords : MonoBehaviour, IUIObject
    {
        public string Id => nameof(WindowRecords);

        [SerializeField] private Button _exitButton = null;

        public IRecordsManager _recordsManager = null;

        [SerializeField] private Transform _content;
        [SerializeField] private RecordItemUI _itemPrefab;

        [Inject]
        private void Construct(IRecordsManager recordsManager)
        {
            _recordsManager = recordsManager;
        }

        void Start()
        {
            _exitButton.onClick.AddListener(CloseLeaderboard);

            SetLeaderboard(_recordsManager.ScoreData.scores);

            Hide();
        }

        private void SetLeaderboard(List<ScoreEntry> records)
        {
            foreach (Transform child in _content)
                Destroy(child.gameObject);

            for (int i = 0; i < records.Count; i++)
            {
                var item = Instantiate(_itemPrefab, _content);
                item.Setup(i + 1, records[i]);
            }
        }
        private void CloseLeaderboard()
        {
            Hide();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
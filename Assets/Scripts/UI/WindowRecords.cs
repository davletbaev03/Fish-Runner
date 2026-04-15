using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using FishRunner.Services;

namespace FishRunner.UI
{
    public class WindowRecords : MonoBehaviour
    {
        [SerializeField] private Button _exitButton = null;

        public IRecordsManager _recordsManager = null;

        [SerializeField] private Transform _content;
        [SerializeField] private RecordItemUI _itemPrefab;

        void Start()
        {
            _exitButton.onClick.AddListener(CloseLeaderboard);

            _recordsManager = ServiceLocator.Get<IRecordsManager>();
            SetLeaderboard(_recordsManager.ScoreData.scores);

            this.gameObject.SetActive(false);
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
            this.gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WindowRecords : MonoBehaviour
{
    [SerializeField] private Button _exitButton = null;

    [SerializeField] public RecordsManager _recordsManager = null;
    [SerializeField] private Transform _content;
    [SerializeField] private RecordItemUI _itemPrefab;
    void Start()
    {
        _exitButton.onClick.AddListener(CloseLeaderboard);

        SetLeaderboard(_recordsManager._scoreData.scores);
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

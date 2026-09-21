using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FishRunner.UI
{
    public class WindowLoadingScreen : MonoBehaviour,IUIObject
    {
        public string Id => nameof(WindowLoadingScreen);

        [SerializeField] private TextMeshProUGUI _loadingText = null;
        private Sequence _loadingTween;

        private void OnEnable()
        {
            StartLoadingTextTween();
        }

        private void StartLoadingTextTween()
        {
            _loadingTween = DOTween.Sequence()
                .AppendCallback(() => _loadingText.text = "Loading")
                .AppendInterval(0.4f)
                .AppendCallback(() => _loadingText.text = "Loading.")
                .AppendInterval(0.4f)
                .AppendCallback(() => _loadingText.text = "Loading..")
                .AppendInterval(0.4f)
                .AppendCallback(() => _loadingText.text = "Loading...")
                .AppendInterval(0.4f)
                .SetLoops(-1);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _loadingTween.Kill();
        }
    }
}
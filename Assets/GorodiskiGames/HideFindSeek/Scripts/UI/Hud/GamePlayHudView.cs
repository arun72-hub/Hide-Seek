using DG.Tweening;
using UnityEngine;
using TMPro;
using Utilities;
using UnityEngine.UI;

namespace Game.UI.Hud
{
    public sealed class GamePlayHudView : BaseHudWithModel<GameModel>
    {
        private const float _coinsScaleUp = 1.25f;
        private const float _coinsScaleDuration = 0.1f;

        [SerializeField] private TMP_Text _cashText;
        [SerializeField] private TMP_Text _countdownText;
        [SerializeField] private TMP_Text _levelDurationText;
        [SerializeField] private RectTransform _content;
        

        // Add the clock icon image
        [SerializeField] private Image _clockIcon;

        [SerializeField] private Color _seekColor;
        [SerializeField] private Color _hideColor;

        // Add public property for the clock icon
        public Image ClockIcon => _clockIcon;
        public TMP_Text CountdownText => _countdownText;
        public TMP_Text LevelDurationText => _levelDurationText;
        public RectTransform Content => _content;
        public GameObject settingpanel;

        public Color SeekColor => _seekColor;
        public Color HideColor => _hideColor;

        protected override void OnDisable()
        {
        }

        protected override void OnEnable()
        {
        }

        protected override void OnModelChanged(GameModel model)
        {
            _cashText.text = MathUtil.NiceCash(model.Cash);

            if (model.Cash <= 0)
                return;

            DOTween.Kill(this);
            _cashText.transform.localScale = Vector3.one;
            _cashText.transform.DOScale(_coinsScaleUp, _coinsScaleDuration).SetLoops(2, LoopType.Yoyo).SetId(this);
        }

        public void settings()
        {
            Time.timeScale=0f;
            settingpanel.SetActive(true);
        }

        public void close()
        {
            Time.timeScale=1f;
            settingpanel.SetActive(false);
        }
    }
}

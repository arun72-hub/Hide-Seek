using Game.Core.UI;
using Game.Managers;
using Game.States;
using Injection;

namespace Game.UI.Hud
{
    public sealed class GameMenuHudMediator : Mediator<GameMenuHudView>
    {
        private const string _levelPattern = "{0} {1}";
        private const string _arenaWord = "ARENA";

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameManager _gameManager;
        [Inject] private HudManager _hudManager;

        protected override void Show()
        {
            if (_view != null)
            {
                if (_view.LevelLabetText != null && _gameManager != null && _gameManager.Model != null)
                {
                    _view.LevelLabetText.text = string.Format(_levelPattern, _arenaWord, _gameManager.Model.Level);
                }

                if (_gameManager != null)
                {
                    _view.Model = _gameManager.Model;
                }

                if (_view.SettingsButton != null) _view.SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
                if (_view.PlayButton != null) _view.PlayButton.onClick.AddListener(OnPlayButtonClicked);
                if (_view.ShopButton != null) _view.ShopButton.onClick.AddListener(OnShopButtonClicked);
            }
        }

        protected override void Hide()
        {
            if (_view != null)
            {
                if (_view.SettingsButton != null) _view.SettingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
                if (_view.PlayButton != null) _view.PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
                if (_view.ShopButton != null) _view.ShopButton.onClick.RemoveListener(OnShopButtonClicked);
            }
        }

        private void OnShopButtonClicked()
        {
            _hudManager.ShowAdditional<ShopHudMediator>();
        }

        private void OnPlayButtonClicked()
        {
            if(_gameManager.Model.IsSeek)
                _gameStateManager.SwitchToState(typeof(GamePlaySeekState));
            else
                _gameStateManager.SwitchToState(typeof(GamePlayHideState));
        }

        private void OnSettingsButtonClicked()
        {
            _hudManager.ShowAdditional<SettingsHudMediator>();
        }
    }
}
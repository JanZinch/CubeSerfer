using Controllers;
using CoreModel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Management
{
    public class GameStateMachine : MonoBehaviour
    {
        public static GameStateMachine Instance { get; private set; }

        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Character _character;
        [SerializeField] private Button _restartButton;

        [Header("Events")]
        [SerializeField] private UnityEvent _onWinning;
        [SerializeField] private UnityEvent _onLoss;
        
        private const float ShowRestartButtonDelay = 1.0f;
        
        public UnityEvent OnWinning => _onWinning;
        public UnityEvent OnLoss => _onLoss;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            _playerController.OnPathPassed.AddListener(Win);
            _character.OnWon.AddListener(Win);
            _character.OnLost.AddListener(Lost);
            _restartButton.onClick.AddListener(RestartGame);
        }

        private void Win()
        {
            OnWinning?.Invoke();
            ShowRestartButton();
        }
        
        private void Lost()
        {
            OnLoss?.Invoke();
            ShowRestartButton();
        }

        private void ShowRestartButton()
        {
            DOVirtual.DelayedCall(ShowRestartButtonDelay, () =>
            {
                _restartButton.gameObject.SetActive(true);
            });
        }

        private static void RestartGame()
        {
            SceneManager.LoadScene("Game");
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(RestartGame);
            _character.OnWon.RemoveListener(Win);
            _character.OnLost.RemoveListener(Lost);
            _playerController.OnPathPassed.RemoveListener(Win);
        }
    }
}
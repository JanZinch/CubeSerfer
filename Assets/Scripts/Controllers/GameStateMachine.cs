using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class GameStateMachine : MonoBehaviour
    {
        public static GameStateMachine Instance { get; private set; }

        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Character _character;
        [SerializeField] private Button _restartButton;
        
        public event Action OnWinning;
        public event Action OnLoss;
        
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
            DOVirtual.DelayedCall(3.0f, () =>
            {
                _restartButton.gameObject.SetActive(true);
            });
        }

        private void RestartGame()
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
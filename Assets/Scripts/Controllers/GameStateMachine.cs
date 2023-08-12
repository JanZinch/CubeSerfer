using System;
using UnityEngine;

namespace Controllers
{
    public class GameStateMachine : MonoBehaviour
    {
        public static GameStateMachine Instance { get; private set; }

        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Character _character;

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
        }

        private void Win()
        {
            OnWinning?.Invoke();
        }
        
        private void Lost()
        {
            OnLoss?.Invoke();
        }

        private void OnDisable()
        {
            _character.OnWon.RemoveListener(Win);
            _character.OnLost.RemoveListener(Lost);
            _playerController.OnPathPassed.RemoveListener(Win);
        }
    }
}
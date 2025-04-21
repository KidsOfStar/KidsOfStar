using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour 
{
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button rotateButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private PuzzleSystem puzzleSystem;

        private void Awake()
        {
            upButton.onClick.AddListener(() => puzzleSystem.MoveSelection("Up"));
            downButton.onClick.AddListener(() => puzzleSystem.MoveSelection("Down"));
            leftButton.onClick.AddListener(() => puzzleSystem.MoveSelection("Left"));
            rightButton.onClick.AddListener(() => puzzleSystem.MoveSelection("Right"));
            rotateButton.onClick.AddListener(puzzleSystem.RotateSelectedPiece);
            //exitButton.onClick.AddListener(() => Managers.Instance.UIManager.Hide
        }
    
}

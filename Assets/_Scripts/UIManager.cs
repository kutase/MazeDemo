using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MazeDemo
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Slider widthSlider;

        [SerializeField]
        private Slider heightSlider;

        [Inject]
        private GameManager gameManager;

        public void OnGenerateMazeClick()
        {
            gameManager.GenerateMaze(
                Mathf.RoundToInt(widthSlider.value),
                Mathf.RoundToInt(heightSlider.value)
            );
        }

        public void SetWidthAndHeight(int width, int height)
        {
            widthSlider.value = width;
            heightSlider.value = height;
        }
    }
}
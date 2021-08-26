using System;
using UnityEngine;

namespace MazeDemo
{
    public class MazePathSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask selectorLayers;

        [SerializeField] private MazeRenderer mazeRenderer;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, +Single.PositiveInfinity, selectorLayers))
                {
                    Debug.Log($"hit: {hit.point} {mazeRenderer.Maze.WorldCoordinatesToMaze(hit.point)}");
                }
            }
        }
    }
}

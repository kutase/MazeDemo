using System;
using UnityEngine;
using Zenject;

namespace MazeDemo
{
    public class MazePathSelector : MonoBehaviour
    {
        [SerializeField]
        private LayerMask selectorLayers;

        [Inject]
        private IMazeProvider mazeProvider;

        [Inject]
        private Character character;

        [Inject]
        private PathRenderer pathRenderer;

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

                if (Physics.SphereCast(ray, 0.5f, out hit, +Single.PositiveInfinity, selectorLayers))
                {
                    var path = mazeProvider.Maze.FindPath(character.transform.position, hit.point);

                    pathRenderer.ShowPath(path);
                    character.MoveByPath(path);
                }
            }
        }
    }
}

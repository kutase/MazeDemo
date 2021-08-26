using System.Collections;
using System.Collections.Generic;
using MazeDemo;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private MazeRenderer mazeRenderer;

    private void Start()
    {
        var maze = mazeRenderer.Maze;

        var randomMazeCell = maze.GetRandomCell();
        var startPosition = maze.MazeCoordinatesToWorld(randomMazeCell.X, randomMazeCell.Y);
    }
}

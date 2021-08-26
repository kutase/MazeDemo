using System.Collections;
using System.Collections.Generic;
using MazeDemo;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour, IMazeProvider
{
    [SerializeField]
    private int width = 10;

    [SerializeField]
    private int height = 10;

    [Inject]
    private MazeRenderer mazeRenderer;

    [Inject]
    private PathRenderer pathRenderer;

    [Inject]
    private UIManager uiManager;

    [Inject]
    private Character character;

    [Inject]
    private CameraManager cameraManager;

    private Maze maze;
    public Maze Maze => maze;

    private void Awake()
    {
        uiManager.SetWidthAndHeight(width, height);

        GenerateMaze(width, height);
    }

    public void GenerateMaze(int width, int height)
    {
        this.width = width;
        this.height = height;

        maze = new Maze(width, height);
        mazeRenderer.Draw(maze);

        var randomMazeCell = maze.GetRandomCell();
        var startPosition = maze.MazeCoordinatesToWorld(randomMazeCell.X, randomMazeCell.Y);

        character.SetPosition(startPosition);

        pathRenderer.Clear();

        cameraManager.UpdateCameraSizeAndPosition(maze.GetMazeCenter(), maze.GetMazeWorldSize());
    }
}

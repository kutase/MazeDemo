using UnityEngine;

namespace MazeDemo
{
    public class MazeRenderer : MonoBehaviour
    {
        [SerializeField] private float cellSize = 1f;

        [SerializeField] private int width = 10;

        [SerializeField] private int height = 10;

        [SerializeField] private GameObject wallPrefab;

        private Maze maze;
        public Maze Maze => maze;

        private void Awake()
        {
            maze = new Maze(width, height);
            Draw(maze);
        }

        private void Draw(Maze maze)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = maze.GetCell(i, j);
                    var position = maze.MazeCoordinatesToWorld(i, j);

                    if (cell.HasFlag(WallState.UP))
                    {
                        var topWall = Instantiate(wallPrefab, transform);
                        topWall.transform.position = position + new Vector3(0, 0, cellSize / 2f);

                        var topWallScale = topWall.transform.localScale;
                        topWall.transform.localScale = new Vector3(cellSize, topWallScale.y, topWallScale.z);
                    }

                    if (cell.HasFlag(WallState.LEFT))
                    {
                        var leftWall = Instantiate(wallPrefab, transform);
                        leftWall.transform.position = position + new Vector3(-cellSize / 2f, 0, 0);
                        leftWall.transform.eulerAngles = new Vector3(0, 90, 0);

                        var leftWallScale = leftWall.transform.localScale;
                        leftWall.transform.localScale = new Vector3(cellSize, leftWallScale.y, leftWallScale.z);
                    }

                    if (i == width - 1)
                    {
                        if (cell.HasFlag(WallState.RIGHT))
                        {
                            var rightWall = Instantiate(wallPrefab, transform);
                            rightWall.transform.position = position + new Vector3(cellSize / 2f, 0, 0);
                            rightWall.transform.eulerAngles = new Vector3(0, 90, 0);

                            var rightWallScale = rightWall.transform.localScale;
                            rightWall.transform.localScale = new Vector3(cellSize, rightWallScale.y, rightWallScale.z);
                        }
                    }

                    if (j == 0)
                    {
                        if (cell.HasFlag(WallState.DOWN))
                        {
                            var downWall = Instantiate(wallPrefab, transform);
                            downWall.transform.position = position + new Vector3(0, 0, -cellSize / 2f);

                            var downWallScale = downWall.transform.localScale;
                            downWall.transform.localScale = new Vector3(cellSize, downWallScale.y, downWallScale.z);
                        }
                    }
                }
            }
        }
    }
}
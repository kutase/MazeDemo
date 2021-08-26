using System.Collections.Generic;
using UnityEngine;

namespace MazeDemo
{
    public class MazeRenderer : MonoBehaviour
    {
        [SerializeField]
        private float cellSize = 1f;

        [SerializeField]
        private GameObject wallPrefab;
        
        private List<GameObject> walls = new List<GameObject>();

        public void Draw(Maze maze)
        {
            foreach (var wall in walls)
            {
                Destroy(wall);
            }

            walls.Clear();

            for (int i = 0; i < maze.Width; i++)
            {
                for (int j = 0; j < maze.Height; j++)
                {
                    var cell = maze.GetCell(i, j);
                    var position = maze.MazeCoordinatesToWorld(i, j);

                    if (cell.HasFlag(WallState.UP))
                    {
                        var topWall = Instantiate(wallPrefab, transform);
                        topWall.transform.position = position + new Vector3(0, 0, cellSize / 2f);

                        var topWallScale = topWall.transform.localScale;
                        topWall.transform.localScale = new Vector3(cellSize, topWallScale.y, topWallScale.z);

                        walls.Add(topWall);
                    }

                    if (cell.HasFlag(WallState.LEFT))
                    {
                        var leftWall = Instantiate(wallPrefab, transform);
                        leftWall.transform.position = position + new Vector3(-cellSize / 2f, 0, 0);
                        leftWall.transform.eulerAngles = new Vector3(0, 90, 0);

                        var leftWallScale = leftWall.transform.localScale;
                        leftWall.transform.localScale = new Vector3(cellSize, leftWallScale.y, leftWallScale.z);
                        
                        walls.Add(leftWall);
                    }

                    if (i == maze.Width - 1)
                    {
                        if (cell.HasFlag(WallState.RIGHT))
                        {
                            var rightWall = Instantiate(wallPrefab, transform);
                            rightWall.transform.position = position + new Vector3(cellSize / 2f, 0, 0);
                            rightWall.transform.eulerAngles = new Vector3(0, 90, 0);

                            var rightWallScale = rightWall.transform.localScale;
                            rightWall.transform.localScale = new Vector3(cellSize, rightWallScale.y, rightWallScale.z);

                            walls.Add(rightWall);
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

                            walls.Add(downWall);
                        }
                    }
                }
            }
        }
    }
}

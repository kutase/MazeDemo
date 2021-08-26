using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeDemo
{
    public struct CellPosition
    {
        public CellPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override bool Equals(object obj)
        {
            return obj is CellPosition cellPosition && this == cellPosition;
        }

        public static bool operator ==(CellPosition first, CellPosition second) 
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static bool operator !=(CellPosition x, CellPosition y) 
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }

    public struct Neighbour
    {
        public CellPosition CellPosition;
        public WallState SharedWall;
    }

    // 0000 -> NO WALLS
    // 1111 -> LEFT,RIGHT,UP,DOWN
    [Flags]
    public enum WallState
    {
        LEFT = 1, // 0001
        RIGHT = 2, // 0010
        UP = 4, // 0100
        DOWN = 8, // 1000

        VISITED = 128, // 1000 0000
    }

    public class Maze
    {
        private WallState[,] cells;

        private int width;
        private int height;

        public int Width => width;

        public int Height => height;

        private float halfWidth;
        private float halfHeight;

        public Maze(int width, int height)
        {
            Generate(width, height);
        }

        public WallState GetCell(int x, int y)
        {
            return cells[x, y];
        }

        private WallState GetOppositeWall(WallState wall)
        {
            switch (wall)
            {
                case WallState.RIGHT:
                    return WallState.LEFT;

                case WallState.LEFT:
                    return WallState.RIGHT;

                case WallState.UP:
                    return WallState.DOWN;

                case WallState.DOWN:
                    return WallState.UP;
            }

            return WallState.UP;
        }

        private void ApplyRecursiveBacktracker()
        {
            var positionStack = new Stack<CellPosition>();

            var position = new CellPosition
            {
                X = Random.Range(0, width),
                Y = Random.Range(0, height),
            };

            positionStack.Push(position);

            cells[position.X, position.Y] |= WallState.VISITED;

            while (positionStack.Count > 0)
            {
                var current = positionStack.Pop();
                var neighbours = GetUnvisitedNeighbours(current);

                if (neighbours.Count > 0)
                {
                    positionStack.Push(current);

                    var index = Random.Range(0, neighbours.Count);
                    var neighbour = neighbours[index];

                    var neighbourPos = neighbour.CellPosition;

                    // remove shared wall
                    cells[current.X, current.Y] &= ~neighbour.SharedWall;

                    // remove own shared wall
                    cells[neighbourPos.X, neighbourPos.Y] &= ~GetOppositeWall(neighbour.SharedWall);

                    // set cell visited
                    cells[neighbourPos.X, neighbourPos.Y] |= WallState.VISITED;

                    positionStack.Push(neighbourPos);
                }
            }
        }

        private List<Neighbour> GetUnvisitedNeighbours(CellPosition cellPosition)
        {
            var list = new List<Neighbour>();

            if (cellPosition.X > 0)
            {
                if (!cells[cellPosition.X - 1, cellPosition.Y].HasFlag(WallState.VISITED))
                {
                    list.Add(new Neighbour
                    {
                        CellPosition = new CellPosition
                        {
                            X = cellPosition.X - 1,
                            Y = cellPosition.Y,
                        },
                        SharedWall = WallState.LEFT,
                    });
                }
            }

            if (cellPosition.Y > 0)
            {
                if (!cells[cellPosition.X, cellPosition.Y - 1].HasFlag(WallState.VISITED))
                {
                    list.Add(new Neighbour
                    {
                        CellPosition = new CellPosition
                        {
                            X = cellPosition.X,
                            Y = cellPosition.Y - 1,
                        },
                        SharedWall = WallState.DOWN,
                    });
                }
            }

            if (cellPosition.X < width - 1)
            {
                if (!cells[cellPosition.X + 1, cellPosition.Y].HasFlag(WallState.VISITED))
                {
                    list.Add(new Neighbour
                    {
                        CellPosition = new CellPosition
                        {
                            X = cellPosition.X + 1,
                            Y = cellPosition.Y,
                        },
                        SharedWall = WallState.RIGHT,
                    });
                }
            }

            if (cellPosition.Y < height - 1)
            {
                if (!cells[cellPosition.X, cellPosition.Y + 1].HasFlag(WallState.VISITED))
                {
                    list.Add(new Neighbour
                    {
                        CellPosition = new CellPosition
                        {
                            X = cellPosition.X,
                            Y = cellPosition.Y + 1,
                        },
                        SharedWall = WallState.UP,
                    });
                }
            }

            return list;
        }

        private void Generate(int width, int height)
        {
            this.width = width;
            this.height = height;

            halfWidth = width / 2f;
            halfHeight = height / 2f;

            cells = new WallState[width, height];

            var initialState = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cells[i, j] = initialState;
                }
            }

            ApplyRecursiveBacktracker();
        }

        public Vector3 MazeCoordinatesToWorld(int x, int y)
        {
            if (x > width || x < 0 || y > height || y < 0)
            {
                return -Vector3.one;
            }

            return new Vector3(-halfWidth + x, 0, -halfHeight + y);
        }

        public Vector3 MazeCoordinatesToWorld(CellPosition position)
        {
            return MazeCoordinatesToWorld(position.X, position.Y);
        }

        public CellPosition WorldCoordinatesToMaze(Vector3 worldCoordinates)
        {
            return new CellPosition(
                Mathf.RoundToInt(Mathf.RoundToInt(worldCoordinates.x) + halfWidth),
                Mathf.RoundToInt(Mathf.RoundToInt(worldCoordinates.z) + halfHeight)
            );
        }

        public Vector3 GetMazeCenter()
        {
            var leftTopPosition = MazeCoordinatesToWorld(0, height - 1);
            var righBottomPosition = MazeCoordinatesToWorld(width - 1, 0);

            var diff = (righBottomPosition - leftTopPosition);

            return leftTopPosition + diff.normalized * diff.magnitude / 2f;
        }

        public Vector2 GetMazeWorldSize()
        {
            return new Vector2(
                Vector3.Distance(MazeCoordinatesToWorld(0, 0), MazeCoordinatesToWorld(width - 1, 0)),
                Vector3.Distance(MazeCoordinatesToWorld(0, 0), MazeCoordinatesToWorld(0, height - 1))
            );
        }

        public void Regenerate(int width, int height)
        {
            Generate(width, height);
        }

        public CellPosition GetRandomCell()
        {
            return new CellPosition(Random.Range(0, width), Random.Range(0, height));
        }

        public List<Vector3> FindPath(Vector3 startPoint, Vector3 endPoint)
        {
            var startCell = WorldCoordinatesToMaze(startPoint);
            var endCell = WorldCoordinatesToMaze(endPoint);

            var parents = new Dictionary<CellPosition, CellPosition>();

            var visitedNodes = new List<CellPosition>();

            var cellsQueue = new Queue<CellPosition>();

            cellsQueue.Enqueue(startCell);
            visitedNodes.Add(startCell);

            var path = new List<CellPosition>();

            while (cellsQueue.Count > 0)
            {
                var currentCell = cellsQueue.Dequeue();

                if (currentCell == endCell)
                {
                    break;
                }

                var neighbours = GetNeighbours(currentCell)
                    .Where(it => !visitedNodes.Contains(it))
                    .ToList();

                foreach (var nextCell in neighbours)
                {
                    visitedNodes.Add(nextCell);
                    parents[nextCell] = currentCell;

                    cellsQueue.Enqueue(nextCell);
                }
            }

            var parentCell = endCell;

            while (parents.ContainsKey(parentCell))
            {
                path.Add(parentCell);
                parentCell = parents[parentCell];
            }

            path.Add(startCell);
            path.Reverse();

            return path.Select(MazeCoordinatesToWorld).ToList();
        }

        private List<CellPosition> GetNeighbours(CellPosition cell)
        {
            var result = new List<CellPosition>();

            if (cell.X - 1 >= 0 && !cells[cell.X, cell.Y].HasFlag(WallState.LEFT))
            {
                result.Add(new CellPosition(cell.X - 1, cell.Y));
            }

            if (cell.X + 1 < width && !cells[cell.X, cell.Y].HasFlag(WallState.RIGHT))
            {
                result.Add(new CellPosition(cell.X + 1, cell.Y));
            }

            if (cell.Y - 1 >= 0 && !cells[cell.X, cell.Y].HasFlag(WallState.DOWN))
            {
                result.Add(new CellPosition(cell.X, cell.Y - 1));
            }

            if (cell.Y + 1 < height && !cells[cell.X, cell.Y].HasFlag(WallState.UP))
            {
                result.Add(new CellPosition(cell.X, cell.Y + 1));
            }

            return result;
        }
    }
}

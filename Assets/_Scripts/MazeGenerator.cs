using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

public struct Position
{
    public int X;
    public int Y;
}

public struct Neighbour
{
    public Position Position;
    public WallState SharedWall;
}

public class MazeGenerator
{
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

    private WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
    {
        var positionStack = new Stack<Position>();

        var position = new Position
        {
            X = Random.Range(0, width),
            Y = Random.Range(0, height),
        };

        positionStack.Push(position);

        maze[position.X, position.Y] |= WallState.VISITED;

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var index = Random.Range(0, neighbours.Count);
                var neighbour = neighbours[index];

                var neighbourPos = neighbour.Position;
                maze[current.X, current.Y] &= ~neighbour.SharedWall;
                maze[neighbourPos.X, neighbourPos.Y] &= ~GetOppositeWall(neighbour.SharedWall);

                maze[neighbourPos.X, neighbourPos.Y] |= WallState.VISITED;

                positionStack.Push(neighbourPos);
            }
        }

        return maze;
    }

    private List<Neighbour> GetUnvisitedNeighbours(Position position, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbour>();

        if (position.X > 0)
        {
            if (!maze[position.X - 1, position.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X - 1,
                        Y = position.Y,
                    },
                    SharedWall = WallState.LEFT,
                });
            }
        }

        if (position.Y > 0)
        {
            if (!maze[position.X, position.Y - 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X,
                        Y = position.Y - 1,
                    },
                    SharedWall = WallState.DOWN,
                });
            }
        }

        if (position.X < width - 1)
        {
            if (!maze[position.X + 1, position.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X + 1,
                        Y = position.Y,
                    },
                    SharedWall = WallState.RIGHT,
                });
            }
        }

        if (position.Y < height - 1)
        {
            if (!maze[position.X, position.Y + 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = position.X,
                        Y = position.Y + 1,
                    },
                    SharedWall = WallState.UP,
                });
            }
        }

        return list;
    }
    
    public WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];

        var initialState = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = initialState;
            }
        }

        return ApplyRecursiveBacktracker(maze, width, height);
    }
}

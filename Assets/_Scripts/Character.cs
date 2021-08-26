using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MazeDemo;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Coroutine moveCoroutine;

    public void SetPosition(Vector3 nextPosition)
    {
        ClearMoveProcess();

        transform.position = nextPosition;
    }

    public void MoveByPath(List<Vector3> path)
    {
        ClearMoveProcess();

        moveCoroutine = StartCoroutine(MoveByPathProcess(path));
    }

    private void ClearMoveProcess()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);

            transform.DOKill();
        }
    }

    private IEnumerator MoveByPathProcess(List<Vector3> path)
    {
        foreach (var point in path)
        {
            var position = transform.position;

            var nextPoint = point;
            nextPoint.y = position.y;

            var distanceToPoint = Vector3.Distance(position, nextPoint);

            yield return transform
                .DOMove(point, distanceToPoint / moveSpeed)
                .WaitForCompletion();
        }
    }
}

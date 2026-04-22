using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType
    {
        Horizontal,
        Vertical
    }

    [Header("Movement Settings")]
    [SerializeField] private MovementType movementType = MovementType.Horizontal;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;

    private void Start()
    {
        startPosition = transform.position;

        if (movementType == MovementType.Horizontal)
        {
            targetPosition = startPosition + new Vector3(moveDistance, 0f, 0f);
        }
        else
        {
            targetPosition = startPosition + new Vector3(0f, moveDistance, 0f);
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        MovePlatform();
    }

    private void MovePlatform()
    {
        Vector3 destination = movingToTarget ? targetPosition : startPosition;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            movingToTarget = !movingToTarget;
        }
    }
}
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector2 boxSize; // Width and height of the camera's box
    public Vector2 margin; // Margin beyond the box before the camera moves
    public float boxTransitionSpeed = 5f;

    private Transform _playerTransform;
    private Vector3 _targetPosition;

    void Start()
    {
        transform.position = startingPosition;
        _targetPosition = startingPosition;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (_playerTransform == null) return;

        Vector3 playerPosition = _playerTransform.position;

        // Calculate the bounds of the current box
        float boxLeft = _targetPosition.x - boxSize.x / 2;
        float boxRight = _targetPosition.x + boxSize.x / 2;
        float boxBottom = _targetPosition.y - boxSize.y / 2;
        float boxTop = _targetPosition.y + boxSize.y / 2;

        // Check if the player has escaped the box + margin
        if (playerPosition.x < boxLeft - margin.x)
        {
            _targetPosition.x -= boxSize.x; // Move left
        }
        else if (playerPosition.x > boxRight + margin.x)
        {
            _targetPosition.x += boxSize.x; // Move right
        }

        if (playerPosition.y < boxBottom - margin.y)
        {
            _targetPosition.y -= boxSize.y; // Move down
        }
        else if (playerPosition.y > boxTop + margin.y)
        {
            _targetPosition.y += boxSize.y; // Move up
        }

        // Smoothly transition the camera to the target position
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.fixedDeltaTime * boxTransitionSpeed);
    }

    private void OnDrawGizmos()
    {
        // Draw the camera box and margin in the Scene view for debugging
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_targetPosition, new Vector3(boxSize.x, boxSize.y, 0));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_targetPosition, new Vector3(boxSize.x + margin.x * 2, boxSize.y + margin.y * 2, 0));
    }
}

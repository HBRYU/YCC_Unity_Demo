using System;
using UnityEngine;

public class WorldBorder : MonoBehaviour
{
    public enum BorderType
    {
        Top,
        Bottom,
        Left,
        Right,
    }

    public BorderType type;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case BorderType.Top:
                if (_playerTransform.position.y > transform.position.y)
                {
                    PlayerStatus.Die();
                }
                break;
            case BorderType.Bottom:
                if (_playerTransform.position.y < transform.position.y)
                {
                    PlayerStatus.Die();
                }
                break;
            case BorderType.Left:
                if (_playerTransform.position.x < transform.position.x)
                {
                    PlayerStatus.Die();
                }
                break;
            case BorderType.Right:
                if (_playerTransform.position.x > transform.position.x)
                {
                    PlayerStatus.Die();
                }
                break;
        }
    }
}

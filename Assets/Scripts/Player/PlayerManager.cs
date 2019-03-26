using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private FollowTranaform cameraFollowPlayer;

    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager && _playerPrefab)
        {
            Vector2Int playerSpawnPoint = _gameManager.GetRandomFloorCoordinates();
            _player = Instantiate(_playerPrefab) as GameObject;
            _player.transform.position = new Vector3(playerSpawnPoint.x, 1.5f, playerSpawnPoint.y);
            cameraFollowPlayer.SetFollowTransform(_player.transform);
        }
    }

    public Transform GetPlayerTransform()
    {
        return _player.transform;
    }

    public float GetScore()
    {
        // Temp
        return 10;
    }
}

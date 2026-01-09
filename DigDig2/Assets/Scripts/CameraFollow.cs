using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float cameraSpeed;
    Vector3 zOffset;

    [SerializeField] GameStateManager gsManager;

    void Awake()
    {
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        zOffset = transform.position - playerTransform.position;
        gsManager.OnGameStateChange += OnGameStateChanged;
    }

    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + zOffset, Time.deltaTime * cameraSpeed);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

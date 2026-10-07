using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float camSpeed = 0.3f;
    public float difficultyStartTime = 20f;
    public float speedIncreasePerSecond = 0.1f;
    [Range(0.5f, 0.85f)] public float followViewportX = 0.68f;
    public float CurrentSpeed { get; private set; }
    Camera gameCamera;
    public float initialGracePeriod = 5f;
    public GameObject Spikes, Coin, Ground;
    public Transform Player;
    public float groundLength = 17f;
    public float initialObstacleSpacing = 24f;
    public float minimumObstacleSpacing = 14.5f;
    public float obstacleDifficultyDuration = 75f;
    float elapsed, coinTimer = 3f, nextSpawnPoint = 19f, nextObstacleX;
    PlayerMovement playerMovement;
    readonly List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
        Time.timeScale = 1f;
        gameCamera = GetComponent<Camera>();
        playerMovement = Player != null ? Player.GetComponent<PlayerMovement>() : null;
        nextObstacleX = Player != null ? Player.position.x + initialObstacleSpacing : 20f;
        for (int i = 0; i < 3; i++) SpawnGround();
    }

    void Update()
    {
        if (Time.timeScale == 0f || Player == null) return;
        elapsed += Time.deltaTime;
        while (Mathf.Max(Player.position.x, transform.position.x) > nextSpawnPoint - groundLength) SpawnGround();
        coinTimer -= Time.deltaTime;
        if (elapsed >= 8f) SpawnObstacleSequence();
        if (coinTimer <= 0f)
        {
            Spawn(Coin, new Vector3(Mathf.Max(transform.position.x, Player.position.x) + Random.Range(6f, 12f), Random.Range(-2f, -1f), 0f));
            coinTimer = Random.Range(3f, 5f);
        }
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            if (spawned[i] == null) spawned.RemoveAt(i);
            else if (spawned[i].transform.position.x < transform.position.x - 35f)
            {
                Destroy(spawned[i]);
                spawned.RemoveAt(i);
            }
        }
    }

    void SpawnObstacleSequence()
    {
        float leadX = Mathf.Max(Player.position.x, transform.position.x);
        float halfWidth = gameCamera != null ? gameCamera.orthographicSize * gameCamera.aspect : 10f;
        // Generate beyond the visible edge, in world order, avoiding overlapping random spawns.
        float horizon = leadX + halfWidth + 24f;
        float difficulty = Mathf.Clamp01((elapsed - 8f) / Mathf.Max(1f, obstacleDifficultyDuration));
        float spacing = Mathf.Lerp(initialObstacleSpacing, minimumObstacleSpacing, difficulty);
        // Reserve a full jump cycle plus landing time, including if movement settings change.
        float jumpCycleDistance = 0f;
        if (playerMovement != null)
        {
            float gravity = Mathf.Abs(Physics2D.gravity.y) * playerMovement.gravityScale;
            float flightTime = 2f * playerMovement.jumpspeed / Mathf.Max(0.1f, gravity);
            jumpCycleDistance = playerMovement.GetMovementSpeed(10f) * (flightTime + 0.25f);
        }
        spacing = Mathf.Max(spacing, jumpCycleDistance);
        while (nextObstacleX < horizon)
        {
            // Keep the first sequence outside the screen even if the player has already advanced.
            nextObstacleX = Mathf.Max(nextObstacleX, leadX + halfWidth + 4f);
            Spawn(Spikes, new Vector3(nextObstacleX, -2f, 0f));
            if (difficulty > 0.35f && Random.value < difficulty * 0.5f)
                Spawn(Spikes, new Vector3(nextObstacleX + 1.2f, -2f, 0f));
            nextObstacleX += spacing * Random.Range(1f, 1.12f);
        }
    }

    public float GetMinimumScrollSpeed(float secondsAhead = 0f)
    {
        float time = elapsed + Mathf.Max(0f, secondsAhead);
        float baseSpeed = time < initialGracePeriod ? 0f : camSpeed;
        return baseSpeed + Mathf.Max(0f, time - difficultyStartTime) * speedIncreasePerSecond;
    }

    void LateUpdate()
    {
        CurrentSpeed = 0f;
        if (Time.timeScale == 0f || Player == null || Time.deltaTime <= 0f) return;

        // Follow at the right side of the screen, leaving room to see obstacles.
        float halfWidth = gameCamera != null ? gameCamera.orthographicSize * gameCamera.aspect : 10f;
        float followOffset = (followViewportX * 2f - 1f) * halfWidth;
        float followX = Player.position.x - followOffset;
        float minimumSpeed = GetMinimumScrollSpeed();
        float previousX = transform.position.x;
        float nextX = Mathf.Max(previousX + minimumSpeed * Time.deltaTime, followX);
        transform.position = new Vector3(nextX, transform.position.y, transform.position.z);
        CurrentSpeed = (nextX - previousX) / Time.deltaTime;
    }

    void Spawn(GameObject prefab, Vector3 position)
    {
        if (prefab != null) spawned.Add(Instantiate(prefab, position, Quaternion.identity));
    }

    void SpawnGround()
    {
        Spawn(Ground, new Vector3(nextSpawnPoint, -4f, 0f));
        nextSpawnPoint += groundLength;
    }
}

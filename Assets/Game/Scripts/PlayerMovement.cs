using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float movement_speed = 7f;
    public float acceleration = 40f;
    public float speedMargin = 2f;
    GameManager game;
    public float gravityScale = 0.85f;
    public float jumpspeed = 7.5f;
    public int Coins { get; private set; }
    AudioSource jumpSFX, coinSFX, gameoverSFX;
    readonly ContactPoint2D[] contacts = new ContactPoint2D[16];
    bool jumpRequested, dead;
    float horizontal;
    bool down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        game = FindFirstObjectByType<GameManager>();
        // Damping was shortening jumps and slowing horizontal movement.
        rb.linearDamping = 0f;
        rb.gravityScale = gravityScale;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        GameObject manager = GameObject.FindGameObjectWithTag("MM");
        if (manager == null) return;
        AudioSource[] sources = manager.GetComponents<AudioSource>();
        if (sources.Length > 1) jumpSFX = sources[1];
        if (sources.Length > 2) coinSFX = sources[2];
        if (sources.Length > 3) gameoverSFX = sources[3];
    }

    public bool IsGrounded()
    {
        int count = rb.GetContacts(contacts);
        for (int i = 0; i < count; i++)
            if (contacts[i].normal.y > 0.5f && contacts[i].collider.CompareTag("Ground")) return true;
        return false;
    }

    void Update()
    {
        if (dead || Time.timeScale == 0f) return;
        horizontal = (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f) - (Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f);
        down = Input.GetKey(KeyCode.DownArrow);
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded()) jumpRequested = true;
    }

    public float GetMovementSpeed(float secondsAhead = 0f)
    {
        float scrollSpeed = game != null ? game.GetMinimumScrollSpeed(secondsAhead) : 0f;
        // Use automatic scroll speed, never camera follow speed, to avoid a feedback loop.
        return Mathf.Max(movement_speed, scrollSpeed + Mathf.Max(0.5f, speedMargin));
    }

    void FixedUpdate()
    {
        if (dead) return;
        float effectiveSpeed = GetMovementSpeed();
        float horizontalSpeed = Mathf.MoveTowards(rb.linearVelocity.x,
            horizontal * effectiveSpeed, Mathf.Max(acceleration, effectiveSpeed * 6f) * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);
        if (jumpRequested)
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpspeed);
                if (jumpSFX != null) jumpSFX.Play();
            }
            jumpRequested = false;
        }
        if (down) rb.AddForce(Vector2.down * 15f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CoinCollectible coin = other.GetComponent<CoinCollectible>();
        if (dead || coin == null || !coin.TryCollect()) return;
        Coins++;
        if (coinSFX != null) coinSFX.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead || !collision.gameObject.CompareTag("Enemy")) return;
        dead = true;
        UIManager score = FindFirstObjectByType<UIManager>();
        if (score != null) score.SaveResult();
        if (gameoverSFX != null && gameoverSFX.clip != null && Camera.main != null) AudioSource.PlayClipAtPoint(gameoverSFX.clip, Camera.main.transform.position);
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
}

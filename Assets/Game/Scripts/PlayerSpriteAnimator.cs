using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerSpriteAnimator : MonoBehaviour
{
    public Sprite[] idleFrames;
    public Sprite[] runFrames;
    public Sprite[] jumpFrames;
    public float idleFramesPerSecond = 4f;
    public float runFramesPerSecond = 10f;
    SpriteRenderer spriteRenderer;
    Rigidbody2D body;
    PlayerMovement movement;
    float animationTime;
    int previousState = -1;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f) return;
        Vector2 velocity = body.linearVelocity;
        if (Mathf.Abs(velocity.x) > 0.1f) spriteRenderer.flipX = velocity.x < 0f;
        bool airborne = !movement.IsGrounded() || velocity.y > 0.1f;
        int state = airborne ? 2 : (Mathf.Abs(velocity.x) > 0.1f ? 1 : 0);
        if (state != previousState) animationTime = 0f;
        previousState = state;
        animationTime += Time.deltaTime;

        Sprite[] frames = state == 2 ? jumpFrames : (state == 1 ? runFrames : idleFrames);
        if (frames == null || frames.Length == 0) return;
        int frame;
        if (state == 2)
        {
            // The three jump poses represent ascent, apex and descent.
            int pose = velocity.y > 1f ? 0 : (velocity.y < -1f ? 2 : 1);
            frame = Mathf.Min(pose, frames.Length - 1);
        }
        else
        {
            float fps = state == 1 ? runFramesPerSecond : idleFramesPerSecond;
            frame = Mathf.FloorToInt(animationTime * Mathf.Max(0f, fps)) % frames.Length;
        }
        if (frames[frame] != null) spriteRenderer.sprite = frames[frame];
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ActivatedPlatform : MonoBehaviour
{
    public enum PlatformState { Waiting, Moving, Falling }
    public const float MovementDuration = 5f;
    public Vector2 direction = Vector2.right;
    public float speed = 3f;
    public float fallingGravity = 1f;
    public PlatformState State { get; private set; } = PlatformState.Waiting;
    public float SecondsRemaining => Mathf.Max(0f, MovementDuration - (float)elapsed);
    public Vector2 TravelVelocity => State == PlatformState.Moving ? travelVelocity : Vector2.zero;
    Rigidbody2D body;
    Vector2 travelVelocity;
    double elapsed;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0f;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.linearDamping = 0f;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        GetComponent<Collider2D>().isTrigger = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != PlatformState.Waiting) return;
        if (collision.gameObject.GetComponentInParent<PlayerMovement>() == null) return;
        // Only this transition activates the platform; later contacts cannot reset it.
        State = PlatformState.Moving;
        elapsed = 0d;
        travelVelocity = (direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right) * Mathf.Max(0f, speed);
    }

    void FixedUpdate()
    {
        if (State != PlatformState.Moving) return;
        if (elapsed >= MovementDuration - 0.000001d)
        {
            State = PlatformState.Falling;
            body.linearVelocity = Vector2.zero;
            body.bodyType = RigidbodyType2D.Dynamic;
            body.gravityScale = Mathf.Max(0.01f, fallingGravity);
            // Gravity affects Y; X remains locked so collision impulses cannot restart horizontal motion.
            body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            body.WakeUp();
            return;
        }
        double step = System.Math.Min(Time.fixedDeltaTime, MovementDuration - elapsed);
        body.MovePosition(body.position + travelVelocity * (float)step);
        elapsed += step;
    }
}

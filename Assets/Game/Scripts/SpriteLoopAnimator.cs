using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLoopAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float framesPerSecond = 8f;
    SpriteRenderer spriteRenderer;
    float animationTime;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Offset each coin so the scene does not rotate all coins in lockstep.
        animationTime = frames != null && frames.Length > 0
            ? Random.Range(0f, frames.Length / Mathf.Max(1f, framesPerSecond)) : 0f;
    }

    void Update()
    {
        if (Time.timeScale == 0f || frames == null || frames.Length == 0) return;
        animationTime += Time.deltaTime;
        int index = Mathf.FloorToInt(animationTime * Mathf.Max(0f, framesPerSecond)) % frames.Length;
        if (frames[index] != null) spriteRenderer.sprite = frames[index];
    }
}

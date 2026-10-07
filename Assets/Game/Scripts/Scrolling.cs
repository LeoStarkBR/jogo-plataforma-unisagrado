using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public float speed = 0.1f;
    public float parallaxPerUnit = 0.025f;
    public Renderer bkg;
    public float pointsPerBackground = 250f;
    public float transitionDuration = 3f;
    public Texture2D[] backgrounds;
    GameManager game;
    UIManager score;
    Material backgroundMaterial;
    int currentBackground, targetBackground;
    float transitionTime;
    bool transitioning;

    void Start()
    {
        game = FindFirstObjectByType<GameManager>();
        score = FindFirstObjectByType<UIManager>();
        if (bkg != null) backgroundMaterial = bkg.material;
        if (backgroundMaterial == null) return;
        if (backgrounds != null && backgrounds.Length > 0 && backgrounds[0] != null)
            backgroundMaterial.mainTexture = backgrounds[0];
        backgroundMaterial.SetTexture("_NextTex", backgroundMaterial.mainTexture);
        backgroundMaterial.SetFloat("_Blend", 0f);
    }

    void Update()
    {
        if (backgroundMaterial == null || Time.timeScale == 0f) return;
        UpdateBackground();
        float scrollingSpeed = game != null ? game.CurrentSpeed * parallaxPerUnit : speed;
        backgroundMaterial.mainTextureOffset += new Vector2(scrollingSpeed * Time.deltaTime, 0f);
    }

    void UpdateBackground()
    {
        if (backgrounds == null || backgrounds.Length == 0) return;
        if (transitioning)
        {
            transitionTime += Time.deltaTime;
            float blend = Mathf.Clamp01(transitionTime / Mathf.Max(0.01f, transitionDuration));
            backgroundMaterial.SetFloat("_Blend", Mathf.SmoothStep(0f, 1f, blend));
            if (blend < 1f) return;
            currentBackground = targetBackground;
            backgroundMaterial.mainTexture = backgrounds[currentBackground];
            backgroundMaterial.SetFloat("_Blend", 0f);
            transitioning = false;
        }
        float points = score != null ? score.Score : 0f;
        int index = Mathf.FloorToInt(points / Mathf.Max(1f, pointsPerBackground)) % backgrounds.Length;
        if (index == currentBackground || backgrounds[index] == null) return;
        targetBackground = index;
        transitionTime = 0f;
        backgroundMaterial.SetTexture("_NextTex", backgrounds[index]);
        transitioning = true;
    }

    void OnDestroy()
    {
        if (backgroundMaterial != null) Destroy(backgroundMaterial);
    }
}

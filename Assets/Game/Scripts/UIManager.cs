using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    float startX, distance;
    public GameObject player;
    PlayerMovement movement;
    public float Score => distance + (movement != null ? movement.Coins * 10f : 0f);

    void Start()
    {
        startX = player.transform.position.x;
        movement = player.GetComponent<PlayerMovement>();
        scoreText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        distance = Mathf.Max(distance, player.transform.position.x - startX);
        scoreText.text = $"Pontos: {Score:F0} | Moedas: {movement.Coins}";
    }

    public void SaveResult()
    {
        distance = Mathf.Max(distance, player.transform.position.x - startX);
        PlayerPrefs.SetFloat("LastScore", Score);
        PlayerPrefs.SetFloat("BestScore", Mathf.Max(Score, PlayerPrefs.GetFloat("BestScore", 0f)));
        PlayerPrefs.Save();
    }
}

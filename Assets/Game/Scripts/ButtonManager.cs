using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 2) return;
        foreach (TextMeshProUGUI label in FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None))
            if (label.text == "Fim de jogo")
            {
                label.enableAutoSizing = true;
                label.fontSizeMin = 14f;
                label.text = $"Fim de jogo\nPontos: {PlayerPrefs.GetFloat("LastScore", 0f):F0}\nRecorde: {PlayerPrefs.GetFloat("BestScore", 0f):F0}";
            }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
        TextMeshProUGUI label = GetComponentInChildren<TextMeshProUGUI>();
        if (label != null) label.text = Time.timeScale == 0f ? "Continuar" : "Pausar";
    }
}

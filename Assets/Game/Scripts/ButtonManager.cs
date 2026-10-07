using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MenuPrincipal" && gameObject.name == "Button")
        {
            GameObject activityButton = Instantiate(gameObject, transform.parent);
            activityButton.name = "BotaoAtividadePlataforma";
            RectTransform rect = activityButton.GetComponent<RectTransform>();
            rect.anchoredPosition += Vector2.down * 70f;
            rect.sizeDelta = new Vector2(270f, 40f);
            UnityEngine.UI.Button button = activityButton.GetComponent<UnityEngine.UI.Button>();
            button.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            button.onClick.AddListener(() => { Time.timeScale = 1f; SceneManager.LoadScene("PlataformaTemporizada"); });
            TextMeshProUGUI text = activityButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null) { text.text = "Desafio da plataforma"; text.enableAutoSizing = true; }
        }
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

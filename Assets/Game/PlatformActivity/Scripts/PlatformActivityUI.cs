using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformActivityUI : MonoBehaviour
{
    public ActivatedPlatform platform;
    public Transform player;
    public Collider2D destination;
    bool won, lost;
    GUIStyle titleStyle, textStyle;

    void Start() { Time.timeScale = 1f; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MenuPrincipal");
        if (won || lost || player == null) return;
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (platform.State != ActivatedPlatform.PlatformState.Waiting &&
            destination != null && playerCollider != null && playerCollider.IsTouching(destination)) won = true;
        if (player.position.y < -12f) lost = true;
    }

    void Restart() { Time.timeScale = 1f; SceneManager.LoadScene("PlataformaTemporizada"); }

    void OnGUI()
    {
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 25, fontStyle = FontStyle.Bold, wordWrap = true };
            textStyle = new GUIStyle(GUI.skin.label) { fontSize = 18, wordWrap = true };
        }
        float panelWidth = Mathf.Min(580f, Screen.width - 24f);
        GUI.Box(new Rect(12, 12, panelWidth, 210), GUIContent.none);
        GUI.Label(new Rect(26, 22, panelWidth - 28, 36), "DESAFIO DA PLATAFORMA", titleStyle);
        string status = platform.State == ActivatedPlatform.PlatformState.Waiting ? "Aguardando contato do jogador" :
            platform.State == ActivatedPlatform.PlatformState.Moving ? $"Movendo para a direita: {platform.SecondsRemaining:F2} s" :
            "Tempo encerrado: movimento horizontal parado, gravidade ativada";
        GUI.Label(new Rect(26, 65, panelWidth - 28, 56), status, textStyle);
        GUI.Label(new Rect(26, 123, panelWidth - 28, 76),
            "← → mover | ↑ pular | R reiniciar | Esc menu\nToque na plataforma azul, atravesse o vão e pule para a ilha verde antes da queda.", textStyle);
        if (!won && !lost) return;
        float x = Mathf.Max(12f, (Screen.width - 370f) / 2f);
        float y = Screen.height / 2f;
        GUI.Box(new Rect(x, y, 370, 105), GUIContent.none);
        GUI.Label(new Rect(x + 16, y + 12, 338, 40), won ? "Travessia concluída!" : "Você caiu. Tente novamente!", titleStyle);
        if (GUI.Button(new Rect(x + 16, y + 60, 150, 30), "Jogar novamente")) Restart();
        if (GUI.Button(new Rect(x + 185, y + 60, 165, 30), "Voltar ao menu")) SceneManager.LoadScene("MenuPrincipal");
    }
}

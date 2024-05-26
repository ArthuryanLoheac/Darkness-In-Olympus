using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgresiveText : MonoBehaviour
{
    public TextMeshPro guiText;
    private string message_one = 
        "Once upon a time, a sacred flame, a symbol of unity and peace, illuminated the skies. This flame, blessed by the gods, was carried by a champion chosen for his courage and purity of heart. This champion, known as Lumen, was the first bearer of the Olympic Flame.";
    private string message_two = 
        "Every four years, the Olympic Games gathered the best athletes from all the kingdoms, celebrating strength, agility, and spirit. The Olympic Flame, lit by the gods themselves, shone during these games, uniting nations in sacred truce and friendly competition.";
    private string message_three = 
        "But one day, a dark prophecy came true. The forces of chaos, jealous of the light and hope that the flame represented, emerged from the shadows. The world plunged into darkness, kingdoms fell one after another, and peace was replaced by terror.";
    private string message_four = 
        "The Olympic Games were canceled, the stadiums once filled with life and celebrations became silent ruins. Lumen, betrayed and captured, was imprisoned in an evil dungeon, populated by monstrous creatures, armed only with the last ashes of the Olympic Flame.";
    private string message;
    public int state;
    private bool ended;
    public Camera cam;

    // Start is called before the first frame update
    void Start () {
        if (state == 0)
            message = message_one;
        if (state == 1)
            message = message_two;
        if (state == 2)
            message = message_three;
        if (state == 3)
            message = message_four;
        guiText.text = "";
        ended = false;
        StartCoroutine(TypeLetters());
    }

    IEnumerator Load_Game () {
        for (float i = 5; i > 2.5f; i -= 0.05f) {
            cam.orthographicSize = i;
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("Game");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            StartCoroutine(Load_Game());
        if (ended && Input.GetKeyDown(KeyCode.Space)) {
            if (state == 0)
                SceneManager.LoadScene("AnimationScene_two");
            if (state == 1)
                SceneManager.LoadScene("AnimationScene_three");
            if (state == 2)
                SceneManager.LoadScene("AnimationScene_four");
            if (state == 3)
                StartCoroutine(Load_Game());
        }
    }
    
    IEnumerator TypeLetters () {
        foreach (char letter in message.ToCharArray()) {
            guiText.text += letter;
            yield return new WaitForSeconds(0.02f);
            if (Input.GetKeyDown(KeyCode.Space)) {
                guiText.text = message;
                break;
            }
        }
        ended = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Key_count_UI : MonoBehaviour
{
    private TextMeshProUGUI text;
    public GameObject player;
    void Start()
    {
        text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = player.GetComponent<Key_script>().key_count.ToString();
    }
}

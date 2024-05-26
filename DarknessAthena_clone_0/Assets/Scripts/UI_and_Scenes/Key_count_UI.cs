using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Key_count_UI : MonoBehaviour
{
    private TextMeshProUGUI txt;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        txt = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (player)
            txt.text = player.GetComponent<Key_script>().key_count.ToString();
        else        
            player = GameObject.Find("Player");
    }
}

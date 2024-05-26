using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    private Image fill_bar;
    private GameObject player = null;

    void Start()
    {
        player = GameObject.Find("Player");
        fill_bar = this.gameObject.transform.GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        if (player)
            fill_bar.fillAmount = player.GetComponent<LifePlayer>().get_life_as_percent() / 100f;
        else        
            player = GameObject.Find("Player");
    }
}

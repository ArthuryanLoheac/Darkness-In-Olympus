using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    private Image fill_bar;
    public GameObject player;

    void Start()
    {
        fill_bar = this.gameObject.transform.GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        fill_bar.fillAmount = player.GetComponent<LifePlayer>().get_life_as_percent() / 100f;
    }
}

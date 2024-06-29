using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingSlider;
    private PauseCheck PauseManager;
    public TextMeshProUGUI loadingTxt;
    public TextMeshProUGUI PourcentTxt;

    private int points = 0;
    private float timer = 1f;

    public void ShowLoading()
    {
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
        loadingPanel.SetActive(true);
        loadingSlider.value = 0f;
        PauseManager.IsPlayerCanMove = false;
        points = 0;
    }

    public void HideLoading()
    {
        loadingPanel.SetActive(false);
        PauseManager.IsPlayerCanMove = true;
    }

    public void setLoadingValue(float val)
    {
        loadingSlider.value = val;
        PourcentTxt.text = ((int)(val * 100)).ToString() + "%";
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            points = (points + 1) % 4;
            loadingTxt.text = "Loading";
            for (int i = 0; i < points; i++)
                loadingTxt.text = loadingTxt.text + ".";
            timer = 1f;
        }
    }

    void Start()
    {
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
    }
}

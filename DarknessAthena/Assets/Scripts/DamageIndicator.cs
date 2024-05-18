using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    private float Life_time;
    private float Reducing_scale;
    private PauseCheck PauseManager;

    void Start()
    {
        Life_time = 1f;
        Reducing_scale = 80f;
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.IsPlaying) {
            Life_time -= Time.deltaTime;
            if (Life_time <= 0f)
                Destroy(this.gameObject);
            else {
                if (this.gameObject.GetComponent<TextMeshPro>().fontSize > 0)
                    this.gameObject.GetComponent<TextMeshPro>().fontSize -= (Reducing_scale * Time.deltaTime);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeMob : MonoBehaviour
{
    private float Life;
    private float Invisibility_time;
    private float Ennemy_Type;
    private PauseCheck PauseManager;
    
    void Start()
    {
        Ennemy_Type = transform.gameObject.GetComponent<MoveMob>().Ennemy_Type;
        if (Ennemy_Type == 0) {
            Life = 10f;
        }
        if (Ennemy_Type == 1) {
            Life = 15f;
        }
        if (Ennemy_Type == 2) {
            Life = 30f;
        }
        if (Ennemy_Type == 3) {
            Life = 3f;
        }
        Invisibility_time = 0f;
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
    }

    void Update()
    {
        if (PauseManager.IsPlaying && Invisibility_time > 0f) {
            Invisibility_time -= Time.deltaTime;
        }
    }

    private void Add_Damage_Indicator(float intensity)
    {
        GameObject Texts = GameObject.Find("-- TEXTS --");
        GameObject Indicator = Texts.GetComponent<Transform>().Find("Damage_Indicator").gameObject;
        GameObject Instance = Instantiate(Indicator,
            new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z),
            Quaternion.identity, transform);
        Instance.GetComponent<TextMeshPro>().text = intensity.ToString("F2");
        Instance.SetActive(true);
    }

    private void Decrease_Life(float intensity)
    {
        if (Invisibility_time <= 0f) {
            Add_Damage_Indicator(intensity);
            Life -= intensity;
            Invisibility_time = 0.4f;
        }
        if (Life <= 0f)
            Destroy(this.gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Torch" && PauseManager.IsPlaying) {
            Decrease_Life(other.gameObject.GetComponent<basic_torch>().light_torch.pointLightInnerRadius /
            Vector2.Distance(other.transform.position, transform.position));
        }
    }
}

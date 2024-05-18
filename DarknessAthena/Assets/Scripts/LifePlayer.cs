using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifePlayer : MonoBehaviour
{
    private float Life;
    private float Invisibility_time;

    // Start is called before the first frame update
    void Start()
    {
        Life = 40f;
        Invisibility_time = 0f;
    }

    void Update()
    {
        Invisibility_time -= Time.deltaTime;
    }

    private void Add_Damage_Indicator(float damage)
    {
        GameObject Texts = GameObject.Find("-- TEXTS --");
        GameObject Indicator = Texts.GetComponent<Transform>().Find("Damage_Indicator").gameObject;
        GameObject Instance = Instantiate(Indicator,
            new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z),
            Quaternion.identity, transform);
        Instance.GetComponent<TextMeshPro>().text = damage.ToString("F2");
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
            this.gameObject.SetActive(false);
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Ennemy" && Invisibility_time <= 0f) {
            Decrease_Life(5);
        }
    }
}

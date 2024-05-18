using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LifePlayer : MonoBehaviour
{
    private float Life;
    private float max_life;
    private float Invisibility_time;
    private PauseCheck PauseManager;

    // Start is called before the first frame update
    void Start()
    {
        max_life = 100f;
        Life = max_life;
        Invisibility_time = 0f;
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
    }

    public float get_life_as_percent()
    {
        return  (Life * 100) / max_life;
    }

    void Update()
    {
        if (PauseManager.IsPlaying)
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
        if (Life <= 0f) {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(2);
        }
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Ennemy" && Invisibility_time <= 0f && PauseManager.IsPlaying) {
            Decrease_Life(5);
        }
    }
}

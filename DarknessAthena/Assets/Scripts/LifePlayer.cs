using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LifePlayer : MonoBehaviour
{
    public float Life;
    public float max_life;
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
        if (PauseManager.IsPlaying && Life > 0f)
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

    IEnumerator WaitAndKill()
    {
        for (int i = 0; i < 90; i++) {
            transform.rotation = Quaternion.Euler(i, 0, 0);
            yield return new WaitForSeconds(1/90);
        }
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        SceneManager.LoadScene(2);
    }

    public void Decrease_Life(float intensity)
    {
        if (this.gameObject.GetComponent<AudioSource>().enabled) {
            this.gameObject.GetComponent<AudioSource>().Stop();
            if (Life > 0f)
                this.gameObject.GetComponent<AudioSource>().Play();
        }
        if (Invisibility_time <= 0f) {
            Add_Damage_Indicator(intensity);
            Life -= intensity;
            Invisibility_time = 0.4f;
        }
        if (Life <= 0f) {
            StartCoroutine(WaitAndKill());
        }
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Ennemy" &&
            Invisibility_time <= 0f && PauseManager.IsPlaying && Life > 0f
            && collisionInfo.gameObject.GetComponent<LifeMob>().Life > 0f) {
            Decrease_Life(collisionInfo.gameObject.GetComponent<MoveMob>().Damage);
        }
    }
}

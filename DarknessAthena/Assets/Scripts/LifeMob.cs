using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeMob : MonoBehaviour
{
    public float Life;
    private float Invisibility_time;
    private float Ennemy_Type;
    private PauseCheck PauseManager;
    private float Time_animation_death;
    
    void Start()
    {
        Time_animation_death = 0f;
        Ennemy_Type = transform.gameObject.GetComponent<MoveMob>().Ennemy_Type;
        if (Life == 0) {
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
        }
        Invisibility_time = 0f;
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
    }

    void Update()
    {
        if (PauseManager.IsPlaying && Invisibility_time > 0f) {
            Invisibility_time -= Time.deltaTime;
        }
        if (Life <= 0f) {  
            Time_animation_death += Time.deltaTime;
            transform.rotation = Quaternion.Euler((Time_animation_death / 0.3f) * 90, 0, 0);
                if (Time_animation_death >= 0.3f) {
                Destroy(this.gameObject);
            }
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

    public void Decrease_Life(float intensity)
    {
        if (Invisibility_time <= 0f) {
            Add_Damage_Indicator(intensity);
            Life -= intensity;
            Invisibility_time = 0.4f;
        }
    }

    private bool is_torch_in_sight(Transform Player)
    {
        float distance_from_player = Vector2.Distance(Player.position,
            transform.position);
        if (distance_from_player > transform.gameObject.GetComponent<MoveMob>().Detection_Range)
            return false;
        float Angle_to_cible = Mathf.Atan2(Player.position.y - transform.position.y,
            Player.position.x - transform.position.x);
        RaycastHit2D[] ray_list = Physics2D.RaycastAll(transform.position,
            new Vector2(Mathf.Cos(Angle_to_cible), Mathf.Sin(Angle_to_cible)));
        foreach (RaycastHit2D ray in ray_list) {
            if (ray.collider.tag == "Wall" && ray.distance < distance_from_player)
                return false;
            if (ray.collider.tag == "Player")
                return true;
        }
        return true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Torch" && PauseManager.IsPlaying && Life > 0 && is_torch_in_sight(other.gameObject.GetComponent<Transform>())) {
            Decrease_Life(other.gameObject.GetComponent<basic_torch>().light_torch.pointLightInnerRadius /
            Vector2.Distance(other.transform.position, transform.position));
        }
    }
}

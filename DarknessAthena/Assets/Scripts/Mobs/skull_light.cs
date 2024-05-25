using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class skull_light : MonoBehaviour
{
    private Light2D lightAura;
    private ParticleSystem particles;
    private GameObject player;
    void Start()
    {
        lightAura = gameObject.transform.GetComponent<Transform>().GetChild(0).GetComponent<Light2D>();
        lightAura.intensity = 0;
        particles = gameObject.transform.GetComponent<Transform>().GetChild(1).GetComponent<ParticleSystem>();
        player = GameObject.Find("Player");
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
    void Update()
    {
        var emission = particles.emission;
        if (is_torch_in_sight(player.transform)) {
            lightAura.intensity = 10f;
            emission.rateOverTime = 100f;
        } else {
            lightAura.intensity = 0f;
            emission.rateOverTime = 0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class basic_torch : MonoBehaviour
{
    private float max_fuel = 100f;
    public float fuel = 100f;
    private float consumption_rate = 1f; //consumption of fuel per second, set to zero to make it infinite
    private float max_radius = 1.5f;
    public bool state = false;
    private CircleCollider2D hitbox;
    private GameObject torch;
    public ParticleSystem particles;
    public Light2D light_torch;
    private PauseCheck PauseManager;

    void Start()
    {
        torch = gameObject;
        hitbox = torch.GetComponent<CircleCollider2D>();
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
    }
    
    public void switch_torch_state()
    {
        if (PauseManager.IsPlaying)
            state = !state;
    }

    private void update_torch_radius(float time_spent)
    {
        if (state && fuel > 0.2)
            fuel -= time_spent * consumption_rate;
        hitbox.radius = (fuel * max_radius) / max_fuel;
        var emission = particles.emission;
        emission.rateOverTime = (fuel * 1000) / max_fuel;
    }

    void Update()
    {
        if (PauseManager.IsPlaying) {
            update_torch_radius(Time.deltaTime);
            light_torch.intensity = Mathf.Log(Mathf.PingPong(Time.time, 1) + 2f);
            light_torch.pointLightOuterRadius =  (fuel * max_radius) / max_fuel + 0.3f;
            light_torch.pointLightInnerRadius = (fuel * max_radius) / max_fuel;
            hitbox.enabled = state;
            light_torch.enabled = state;
            if (state) {
                particles.Play();
            } else {
                particles.Stop();
            }
        }
    }
}

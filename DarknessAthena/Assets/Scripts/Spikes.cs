using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private LifePlayer life;
    private float WaitTime;
    public float WaitTime_value;
    private bool Active;
    private bool Player_Hurted;
    private AudioSource SpikeOut;

    void Start()
    {
        life = GameObject.Find("Player").GetComponent<LifePlayer>();
        SpikeOut = gameObject.GetComponent<AudioSource>();
        if (WaitTime_value == 0f)
            WaitTime_value = 5f;
        WaitTime = WaitTime_value;
        Active = false;
    }

    void Update()
    {
        WaitTime -= Time.deltaTime;
        if (WaitTime <= 0f) {
            if (Active) {
                WaitTime = WaitTime_value;
            } else {
                SpikeOut.Play();
                Player_Hurted = false;
                WaitTime = 0.8f;
                this.gameObject.GetComponent<Animator>().Rebind();
                this.gameObject.GetComponent<Animator>().Play("Peaks");
            }
            Active = !Active;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && life.Life > 0f && Active && !Player_Hurted) {
            Player_Hurted = true;
            life.Decrease_Life(10);
        }
    }
}

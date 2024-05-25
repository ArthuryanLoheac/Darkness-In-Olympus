using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCheck : MonoBehaviour
{
    public bool IsPlaying;
    public GameObject PauseMenu;

    void Start()
    {
        IsPlaying = true;
        PauseMenu = GameObject.Find("PauseMenu");
        PauseMenu.SetActive(!IsPlaying);
    }

    void Update()
    {
        if (Input.GetKeyDown("escape")) {
            IsPlaying = !IsPlaying;
            PauseMenu.SetActive(!IsPlaying);
            GameObject[] Lst = GameObject.FindGameObjectsWithTag("Particules");
            foreach (GameObject obj in Lst) {
                var main = obj.GetComponent<ParticleSystem>().main;
                if (IsPlaying)
                    main.simulationSpeed = 1f;
                else
                    main.simulationSpeed = 0f;
            }
            GameObject[] Lst_anim = GameObject.FindGameObjectsWithTag("Ennemy");
            foreach (GameObject obj in Lst_anim) {
                var anim = obj.GetComponent<Animator>();
                if (anim)
                    anim.enabled = IsPlaying;
            }
        }
    }
}

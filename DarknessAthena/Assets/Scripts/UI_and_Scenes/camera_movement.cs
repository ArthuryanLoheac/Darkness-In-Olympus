using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{

    private float speed;
    public Transform player;
    private PauseCheck PauseManager;

    // Start is called before the first frame update
    void Start()
    {
        speed = 6f;
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.IsPlaying) {
            var step = speed * Time.deltaTime;
            Vector3 camera = Vector2.Lerp(transform.position, player.position, step);
            camera.z = -10;
            transform.position = camera;
        }
    }
}

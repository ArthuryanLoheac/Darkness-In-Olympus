using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMob : MonoBehaviour
{
    private GameObject Player;
    private Transform Player_pos;
    private float distance_from_player;
    private RaycastHit2D[] ray_list;
    private float Angle_to_Hero;
    private float speed_max;
    private float Detection_Range;

    private bool is_moving;
    private float time_before_re_moving;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Player_pos = Player.GetComponent<Transform>();
        speed_max = 0.15f;
        Detection_Range = 1.5f;
        is_moving = true;
        time_before_re_moving = 1f;
    }

    private bool is_player_in_sight()
    {
        distance_from_player = Vector2.Distance(Player_pos.position,
            transform.position);
        if (distance_from_player > Detection_Range)
            return false;
        Angle_to_Hero = Mathf.Atan2(Player_pos.position.y - transform.position.y,
            Player_pos.position.x - transform.position.x);
        ray_list = Physics2D.RaycastAll(transform.position,
            new Vector2(Mathf.Cos(Angle_to_Hero), Mathf.Sin(Angle_to_Hero)));
        foreach (RaycastHit2D ray in ray_list) {
            if (ray.collider.tag != "Player" && ray.collider.tag != "Ennemy")
                return false;
        }
        return true;
    }

    private void Run_into_Player()
    {
        Vector2 direction = new Vector2(Mathf.Cos(Angle_to_Hero), Mathf.Sin(Angle_to_Hero));
        transform.Translate(direction * (speed_max * Time.deltaTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_moving) {
            time_before_re_moving -= Time.deltaTime;
            if (time_before_re_moving <= 0f)
                is_moving = true;
        }
        if (is_player_in_sight() && is_moving) {
            Run_into_Player();
        }
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player") {
            is_moving = false;
            time_before_re_moving = 1f;
        }
    }
}

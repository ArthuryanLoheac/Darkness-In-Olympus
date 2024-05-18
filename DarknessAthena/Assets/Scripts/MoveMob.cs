using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMob : MonoBehaviour
{
    private GameObject Player;
    private Transform Player_pos;

    private float distance_from_player;
    private float Angle_to_Hero;
    private float speed_max;
    private float Detection_Range;
    private float Attack_Range;

    public float Damage;
    public int Ennemy_Type;

    private RaycastHit2D[] ray_list;

    private bool is_moving;
    private float time_before_re_moving_value;
    private float time_before_re_moving;
    private float distance_from_torch;
    private float radius_torch;

    private PauseCheck PauseManager;


    void Start()
    {
        PauseManager = GameObject.Find("GameManager").GetComponent<PauseCheck>();
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Player_pos = Player.GetComponent<Transform>();
        is_moving = true;
        time_before_re_moving = 0f;
        if (Ennemy_Type == 0) { //Skeleton de base
            speed_max = 0.3f;
            Detection_Range = 3f;
            Attack_Range = 0.5f;
            Damage = 5f;
            time_before_re_moving_value = 1f;
        }
        if (Ennemy_Type == 1) { //Skeleton faucille
            speed_max = 0.3f;
            Detection_Range = 3f;
            Attack_Range = 0.5f;
            Damage = 15f;
            time_before_re_moving_value = 1.5f;
        }
        if (Ennemy_Type == 2) { //Skull
            speed_max = 0.6f;
            Detection_Range = 3f;
            Attack_Range = 3f;
            Damage = 7f;
            time_before_re_moving_value = 0.5f;
        }
        if (Ennemy_Type == 3) { //Vampire
            speed_max = 1.5f;
            Detection_Range = 6f;
            Attack_Range = 0f;
            Damage = 20f;
            time_before_re_moving_value = 2.5f;
        }
    }

    private void Run_into_Player(float speed_boost, Transform obj_transform)
    {
        Angle_to_Hero = Mathf.Atan2(obj_transform.position.y - transform.position.y,
            obj_transform.position.x - transform.position.x);

        Vector2 direction = new Vector2(Mathf.Cos(Angle_to_Hero), Mathf.Sin(Angle_to_Hero));
        transform.Translate(direction * (speed_max * speed_boost * Time.deltaTime));

        if (Ennemy_Type != 2)
            transform.rotation = Quaternion.Euler(0, 0,
                Mathf.PingPong(Time.time * 100, 10) - 5);
    }

    private bool in_light(float offset)
    {
        distance_from_torch = 0f;
    
        GameObject[] LstTorch = GameObject.FindGameObjectsWithTag("Torch");
        foreach (GameObject one_torch in LstTorch) {
            distance_from_torch = Vector2.Distance(one_torch.GetComponent<Transform>().position,
                new Vector3(transform.position.x, transform.position.y - 0.04f, transform.position.z));
            radius_torch = one_torch.GetComponent<CircleCollider2D>().radius;
            if (one_torch.GetComponent<basic_torch>().state == true &&
                distance_from_torch + offset <= radius_torch) {
                return true;
            }
        }
        return false;
    }

    private bool in_light_run_out(float offset)
    {
        distance_from_torch = 0f;
    
        GameObject[] LstTorch = GameObject.FindGameObjectsWithTag("Torch");
        foreach (GameObject one_torch in LstTorch) {
            distance_from_torch = Vector2.Distance(one_torch.GetComponent<Transform>().position,
                new Vector3(transform.position.x, transform.position.y - 0.04f, transform.position.z));
            radius_torch = one_torch.GetComponent<CircleCollider2D>().radius;
            if (one_torch.GetComponent<basic_torch>().state == true &&
                distance_from_torch + offset <= radius_torch) {
                Run_into_Player(-1f, one_torch.GetComponent<Transform>());
                return true;
            }
        }
        return false;
    }

    private bool is_player_in_sight()
    {
        if (distance_from_player > Detection_Range)
            return false;
        Angle_to_Hero = Mathf.Atan2(Player_pos.position.y - transform.position.y,
            Player_pos.position.x - transform.position.x);
        ray_list = Physics2D.RaycastAll(transform.position,
            new Vector2(Mathf.Cos(Angle_to_Hero), Mathf.Sin(Angle_to_Hero)));
        foreach (RaycastHit2D ray in ray_list) {
            if (ray.collider.tag == "Wall")
                return false;
        }
        return true;
    }

    private void Update_Moving()
    {
        if (!is_moving) {
            time_before_re_moving -= Time.deltaTime;
            if (time_before_re_moving <= 0f)
                is_moving = true;
        }
        distance_from_player = Vector2.Distance(Player_pos.position,
            transform.position);
        if (time_before_re_moving <= 0f) {
            if ((is_player_in_sight() && is_moving && !in_light(-0.105f)) ||
                    distance_from_player <= Attack_Range) {
                Run_into_Player(1f, Player_pos);
            } else if (in_light_run_out(-0.105f) && is_moving) {            
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    void Update()
    {
        if (PauseManager.IsPlaying)
            Update_Moving();
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player" && PauseManager.IsPlaying) {
            is_moving = false;
            time_before_re_moving = time_before_re_moving_value;
        }
    }
}

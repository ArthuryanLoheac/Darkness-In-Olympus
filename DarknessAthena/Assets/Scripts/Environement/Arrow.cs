using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float Speed;
    private float Life;
    private Vector2 direction;
    private LifePlayer life_player;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 0.5f;
        Life = 3f;
        life_player = GameObject.Find("Player").GetComponent<LifePlayer>();
    }

    public void set_direction(float x, float y)
    {
        direction.x = x;
        direction.y = y;
        if (x == 0 && y < 0f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        if (x == 0 && y > 0f)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        if (x > 0f && y == 0f)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        if (x < 0f && y == 0f)
            transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    // Update is called once per frame
    void Update()
    {
        Life -= Time.deltaTime;
        if (Life <= 0f) {
            Destroy(this.gameObject);
        }
        transform.position = new Vector3(transform.position.x + (direction.x * Speed * Time.deltaTime),
            transform.position.y + (direction.y * Speed * Time.deltaTime),
            transform.position.z);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            life_player.Decrease_Life(10);
        if (other.gameObject.tag == "Ennemy")
            other.gameObject.GetComponent<LifeMob>().Decrease_Life(3);
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Player" || other.gameObject.tag == "Ennemy")
            Destroy(this.gameObject);
    }
}

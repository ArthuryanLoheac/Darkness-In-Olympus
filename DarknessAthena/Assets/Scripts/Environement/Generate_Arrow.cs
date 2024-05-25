using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Arrow : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject Generated_Arrow;
    public float Time_between_arrow;
    private float Time_arrow;
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        if (Time_between_arrow == 0)
            Time_between_arrow = 5f;
        if (direction.x == 0 && direction.y == 0)
            direction = new Vector2(0f, -1f);
        Time_arrow = Time_between_arrow;
    }

    // Update is called once per frame
    void Update()
    {
        Time_arrow -= Time.deltaTime;
        if (Time_arrow <= 0f) {
            Generated_Arrow = Instantiate(Arrow,
                new Vector3 (transform.position.x + (direction.x / 20), transform.position.y + (direction.y / 20), transform.position.z),
                Quaternion.identity, transform);
            Generated_Arrow.GetComponent<Arrow>().set_direction(direction.x, direction.y);
            Time_arrow = Time_between_arrow;
        }
    }
}

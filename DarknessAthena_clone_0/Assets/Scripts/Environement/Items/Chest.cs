using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool opened;
    public Sprite sprite_open;
    public GameObject[] LstObject;

    // Start is called before the first frame update
    void Start()
    {
        opened = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !opened && Input.GetKey(KeyCode.E)) {
            opened = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite_open;
            Instantiate(LstObject[Random.Range(0, LstObject.Length)], new Vector3 (transform.position.x, transform.position.y - 0.09f, transform.position.z), Quaternion.identity, transform);
        }
    }
}

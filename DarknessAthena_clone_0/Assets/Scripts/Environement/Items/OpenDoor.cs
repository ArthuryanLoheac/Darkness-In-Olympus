using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Key_script keys;
    public Transform doors;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player" &&
            Vector2.Distance(transform.position, other.gameObject.GetComponent<Transform>().position) <= 0.2f && keys.key_count > 0) {
            keys.key_count -= 1;
            doors.GetChild(0).gameObject.SetActive(false);
            doors.GetChild(1).gameObject.SetActive(false);
            doors.GetChild(2).gameObject.SetActive(true);
            doors.GetChild(3).gameObject.SetActive(true);
        }
    }
}

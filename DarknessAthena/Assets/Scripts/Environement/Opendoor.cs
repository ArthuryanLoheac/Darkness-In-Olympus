using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opendoor : MonoBehaviour
{
    public Key_script keys;
    public Transform doors;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player"
            && keys.key_count > 0) {
            keys.key_count -= 1;
            doors.GetChild(0).gameObject.SetActive(false);
            doors.GetChild(1).gameObject.SetActive(false);
            doors.GetChild(2).gameObject.SetActive(true);
            doors.GetChild(3).gameObject.SetActive(true);
            doors.GetChild(4).gameObject.SetActive(false);
        }
    }
}

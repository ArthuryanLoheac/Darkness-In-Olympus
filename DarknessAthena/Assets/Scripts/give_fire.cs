using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class give_fire : MonoBehaviour
{
    private basic_torch brasero;
    void Start()
    {
        brasero = this.gameObject.GetComponent<basic_torch>();
        brasero.state = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player") {
            var torch = other.gameObject.transform.GetChild(0).GetComponent<basic_torch>();
            if (brasero.state == true && Input.GetMouseButton(1)
                && torch.max_fuel - 5 > torch.fuel && brasero.fuel > 5) {
                torch.fuel += 1;
                brasero.fuel -= 1;
            }
        }
    }
}

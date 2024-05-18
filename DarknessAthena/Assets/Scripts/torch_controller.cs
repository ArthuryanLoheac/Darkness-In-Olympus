using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torch_controller : MonoBehaviour
{
    public GameObject torch;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Pressed left-click.");
            torch.GetComponent<basic_torch>().switch_torch_state();
        }
    }
}

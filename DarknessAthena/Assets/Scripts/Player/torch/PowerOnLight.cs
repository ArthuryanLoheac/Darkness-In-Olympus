using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOnLight : MonoBehaviour
{
    private basic_torch bas_torch;
    private QuestManager Quest;
    // Start is called before the first frame update
    void Start()
    {
        bas_torch = this.gameObject.GetComponent<basic_torch>();
        bas_torch.state = false;
        Quest = GameObject.Find("GameManager").GetComponent<QuestManager>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Torch" &&
            Vector2.Distance(transform.position, other.GetComponent<Transform>().position) <= 0.15f) {
            if (!bas_torch.state) {
                bas_torch.switch_torch_state();
                Quest.LightOnTorch();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private LifePlayer life;
    private basic_torch torch;

    void Start()
    {
        life = GameObject.Find("Player").GetComponent<LifePlayer>();
        torch = life.gameObject.GetComponent<Transform>().GetChild(0).gameObject.GetComponent<basic_torch>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && life.Life > 0f) {
            if (this.gameObject.tag == "LittleHealPotion" && life.Life < life.max_life) {
                life.Life += 20f;
                if (life.Life > life.max_life)
                    life.Life = life.max_life;
                Destroy(this.gameObject);
            }
            if (this.gameObject.tag == "BigHealthPotion" && life.Life < life.max_life) {
                life.Life += 40f;
                if (life.Life > life.max_life)
                    life.Life = life.max_life;
                Destroy(this.gameObject);
            }
            if (this.gameObject.tag == "FuelPotion") {
                torch.fuel += 20f;
                if (torch.fuel > torch.max_fuel)
                    torch.fuel = torch.max_fuel;
                Destroy(this.gameObject);
            }
            if (this.gameObject.tag == "Key") {
                GameObject.Find("Player").GetComponent<Key_script>().key_count += 1;
                Destroy(this.gameObject);
            }
        }
    }
}

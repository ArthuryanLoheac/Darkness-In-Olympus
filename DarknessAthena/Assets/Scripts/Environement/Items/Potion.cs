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

    private void Take_HealPotion(float Pv_Heal)
    {
        if (life.Life < life.max_life) {
            life.Life += Pv_Heal;
            if (life.Life > life.max_life)
                life.Life = life.max_life;
            Destroy(this.gameObject);
        }
    }

    private void Take_FuelPotion(float Fuel_Heal)
    {
        if (torch.fuel < torch.max_fuel) {
            torch.fuel += Fuel_Heal;
            if (torch.fuel > torch.max_fuel)
                torch.fuel = torch.max_fuel;
            Destroy(this.gameObject);
        }
    }

    private void Take_Key(int nb_keys)
    {
        GameObject.Find("Player").GetComponent<Key_script>().key_count += nb_keys;
        Destroy(this.gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && life.Life > 0f && Input.GetKeyDown(KeyCode.E)) {
            if (this.gameObject.tag == "LittleHealPotion")
                Take_HealPotion(20f);
            if (this.gameObject.tag == "BigHealthPotion")
                Take_HealPotion(40f);
            if (this.gameObject.tag == "FuelPotion")
                Take_FuelPotion(20f);
            if (this.gameObject.tag == "Key")
                Take_Key(1);
        }
    }
}

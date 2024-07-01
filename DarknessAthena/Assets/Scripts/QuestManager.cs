using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public GameObject Quest_txt;
    private GameObject[] Lst_torch;
    private int TTtorches;
    private int TorchesLighten;

    public void InitTextChandelier()
    {
        Lst_torch = null;
        Lst_torch = GameObject.FindGameObjectsWithTag("Torch");
        TTtorches = Lst_torch.Length - 1;
        Quest_txt.GetComponent<TextMeshProUGUI>().text = "Torch powered on : 0 / " + TTtorches.ToString();
    }

    public void LightOnTorch()
    {
        TorchesLighten++;
        Quest_txt.GetComponent<TextMeshProUGUI>().text = "Torch powered on : " + TorchesLighten.ToString() + " / " + TTtorches.ToString();
    }
}

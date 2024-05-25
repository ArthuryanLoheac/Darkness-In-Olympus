using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private Image Left;
    private Image Right;
    private Image Controls;
    void Start()
    {
        Left = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        Left.enabled = false;
        PlayerPrefs.SetInt("tuto_left", 1);
        Right = this.gameObject.transform.GetChild(1).GetComponent<Image>();
        Right.enabled = true;
        PlayerPrefs.SetInt("tuto_right", 0);
        Controls = this.gameObject.transform.GetChild(2).GetComponent<Image>();
        Controls.enabled = true;
        PlayerPrefs.SetInt("tuto_controls", 1);
    }
    private bool is_player_moved()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            return true;
        } else {
            return false;
        }
    }
    void Update()
    {
        if (is_player_moved() == true && PlayerPrefs.GetInt("tuto_controls") == 1) {
            PlayerPrefs.SetInt("tuto_controls", 2);
        }
        if (Input.GetMouseButton(0) == true  && PlayerPrefs.GetInt("tuto_left") == 1) {
            PlayerPrefs.SetInt("tuto_left", 2);
        }
        if (PlayerPrefs.GetInt("tuto_left") == 1) {
            Left.enabled = true;
        } else {
            Left.enabled = false;
        }
        if (PlayerPrefs.GetInt("tuto_right") == 1) {
            Right.enabled = true;
        } else {
            Right.enabled = false;
        }
        if (PlayerPrefs.GetInt("tuto_controls") == 1) {
            Controls.enabled = true;
        } else {
            Controls.enabled = false;
        }
    }
}

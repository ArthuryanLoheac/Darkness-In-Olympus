using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class topdownmovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2d;
    public GameObject player;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        PlayableDirector anim = player.GetComponent<PlayableDirector>();
        if (moveInput.y > 0 || moveInput.x > 0){
            anim.Play();
        }
        if (moveInput.y < 0 || moveInput.x < 0){
            anim.Play();
        }

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class topdownmovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        if (moveInput.x > 0){
            
            transform.Rotate(0, 0, 1);
            transform.Rotate(0, 0, -1);
        }
        if (moveInput.x < 0){
            transform.Rotate(0, 0, -2);
            transform.Rotate(0, 0, 1);
        }
        moveInput.y = Input.GetAxisRaw("Vertical");
        if (moveInput.y > 0){
            transform.Rotate(0, 0, 2);
            transform.Rotate(0, 0, -1);
        }
        if (moveInput.y < 0){
            transform.Rotate(0, 0, -2);
            transform.Rotate(0, 0, 1);
        }

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed * Time.deltaTime;
    }
}

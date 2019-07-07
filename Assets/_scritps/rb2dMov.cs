using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rb2dMov : MonoBehaviour
{
    private Vector3 moveDir = Vector3.zero;

    private SpriteRenderer sRenderer;
    private Animator animControl;

    [Range(1,15)]
    public float speed;

    private void Start()
    {
        speed = 5f;
        sRenderer = GetComponent<SpriteRenderer>();
        animControl = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        transform.position += moveDir;  
    }

    private void Update()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        moveDir.y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
        animate();
        flip();
    }

    void flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
           sRenderer.flipX = false;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            sRenderer.flipX = true;
        }
    }

    void animate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animControl.SetBool("isRunning", true);
        }
        else
        {
            animControl.SetBool("isRunning", false);
        }
        
    }
    
}

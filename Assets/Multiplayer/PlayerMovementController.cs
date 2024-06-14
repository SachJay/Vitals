using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{
    public float Speed = 0.01f;

    public GameObject playerModel;

    private void Start()
    {
        playerModel.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (playerModel.activeSelf == false) //Called for every player
            { 
                SetPosition();
                playerModel.SetActive(true);
            }

            if (isOwned) //Called for only your player
            {
                Movement();
            }
        } 
    }

    public void SetPosition()
    {
        //transform.position = Vector3.zero;
        transform.position = new Vector3(Random.Range(-6f, 6f), Random.Range(-4f, 4f), 0f);
    }

    public void Movement()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f);

        transform.position += moveDirection * Speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*Game Name: Return Home  
 Unity game
 Authors Name: Ayhan SAGLAM -- Khadka, Subarna Bijaya -- Vu, Hieu Phong
 Date: 2021/02/10
*/
/// <summary>
/// Scripts related to main character movement and attacking
/// attacking is not implemented in this iteration but will be implemented here later
/// </summary>
public class PlayerBehaviour : MonoBehaviour
{
    public CharacterController controller;

    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHeight = 3.0f;

    public Transform groundCheck;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;

    public Vector3 velocity;
    public bool isGrounded;
    public AudioSource jumpAudio;
    public AudioSource attackAudio;

    [Header("Minimap")]
    public GameObject miniMap;
    
    [Header("Controls")]
    public Joystick joystick;
    public float horizontalSensitivity;
    public float verticalSensitivity;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        miniMap.SetActive(false);
        
    }

    // Update is called once per frame - once every 16.6666ms

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }
        // Input for WebGL and Desktop
        // float x = Input.GetAxis("Horizontal");
        // float z = Input.GetAxis("Vertical");
        float x = joystick.Horizontal;
        float z = joystick.Vertical;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * maxSpeed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            jumpAudio.Play();
        }

        if (Input.GetButton("Vertical"))
        {
            // walkAudio.Play();
            // Debug.Log("Player is walking");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DoDamage();
        }

            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
       
        
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     // toggle minimap on and off
        //     miniMap.SetActive(!miniMap.activeInHierarchy);
        //
        // }

        
    }
    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        jumpAudio.Play();
    }
    void ToggleMinimap()
    {
        // toggle the MiniMap on/off
        miniMap.SetActive(!miniMap.activeInHierarchy);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
    public void OnJumpButtonPressed()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    public void OnMapButtonPressed()
    {
        ToggleMinimap();
    }

    public void DoDamage()
    {
            attackAudio.Play();
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < monsters.Length; ++i)
            {
                if (Vector3.Distance(controller.transform.position, monsters[i].transform.position) <= 3.0f)
                    monsters[i].GetComponent<EnemyBehaviour>().TakeDamage(25);
                    //Debug.Log("attack");
                    //Debug.Log(monsters[i].transform.position);       
        }         
    }


}

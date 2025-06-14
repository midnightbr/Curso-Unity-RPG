using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController PlayerController;
    public Animator PlayerAnimator;
    public Transform PlayerCamTransform;
    public float SmoothRotation;
    public float Speed;

    private float playerRotationSpeed; // Velocidade de rotação do jogador
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Obtendo comandos do jogador de movimentação
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical);

        if(moveDirection.magnitude >= 0.1f)
        {
            // Calculando a direção de movimento e rotação do jogador
            float view = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + PlayerCamTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, view, ref playerRotationSpeed, SmoothRotation);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 newDirection = Quaternion.Euler(0, view, 0) * Vector3.forward;

            // Movendo o jogador na direção calculada
            PlayerController.Move(newDirection * Speed * Time.deltaTime);
            PlayerAnimator.SetBool("Walk", true);
        }
        else
        {
            PlayerAnimator.SetBool("Walk", false);
        }
    }
}

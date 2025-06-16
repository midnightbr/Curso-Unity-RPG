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

    // Variáveis para armazenar a direção de movimento e rotação do jogador
    private float view;
    private float angle;

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

        // Calculando a direção de movimento e rotação do jogador
        view = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + PlayerCamTransform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, view, ref playerRotationSpeed, SmoothRotation);

        MovimentarPersonagem("Walk");

        if (Input.GetKey(KeyCode.LeftShift))
            MovimentarPersonagem("Run");
        else
            PlayerAnimator.SetBool("Run", false);


    }

    void MovimentarPersonagem(string nomeDoMovimento)
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calculando a direção de movimento e rotação do jogador
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 newDirection = Quaternion.Euler(0, view, 0) * Vector3.forward;

            // Movendo o jogador na direção calculada
            PlayerController.Move(newDirection * Speed * Time.deltaTime);
            PlayerAnimator.SetBool(nomeDoMovimento, true);

        }
        else
        {
            PlayerAnimator.SetBool(nomeDoMovimento, false);
        }
    }
}

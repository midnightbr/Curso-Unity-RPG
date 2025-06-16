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
        CalcularMovimentoERotacao();

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && moveDirection.magnitude >= 0.1f;
        bool isJumping = isRunning && Input.GetKeyDown(KeyCode.Space);

        // Criando um dicionário com os estados de animação
        Dictionary<string, bool> animStates = new()
        {
            { "Walk", moveDirection.magnitude >= 0.1f },
            { "Run", isRunning },
            { "Jump", isJumping }, // Novo estado de "Jumping"
            { "Jumping", !isJumping && Input.GetKeyDown(KeyCode.Space) } // Garantindo que "Jump" fique false ao correr
        };

        AtualizarAnimacoes(animStates);
    }

    void CalcularMovimentoERotacao()
    {
        view = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + PlayerCamTransform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, view, ref playerRotationSpeed, SmoothRotation);
    }

    void AtualizarAnimacoes(Dictionary<string, bool> animStates)
    {
        // Aplicando rotação e movimento quando "Walk" ou "Run" estiver ativado
        if (animStates["Walk"] || animStates["Run"])
        {
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 newDirection = Quaternion.Euler(0, view, 0) * Vector3.forward;
            float velocidade = animStates["Run"] ? Speed * 1.5f : Speed;
            PlayerController.Move(newDirection * velocidade * Time.deltaTime);
        }

        // Atualizando animações
        foreach (var anim in animStates)
        {
            PlayerAnimator.SetBool(anim.Key, anim.Value);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveDirection;

    void Update()
    {
        // Bewegungsrichtung über die Pfeiltasten der Tastatur erfassen
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Die Bewegungsrichtung verwenden, um den Spieler zu bewegen
        MovePlayer();
    }

    void MovePlayer()
    {
        // Den Spieler in die entsprechende Richtung bewegen
        float moveSpeed = 5f;
        Vector3 movement = new Vector3(moveDirection.x, 0f, moveDirection.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}
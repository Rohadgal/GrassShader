
using UnityEngine;

public enum Direction
{
    Forward, Right, Up
}

public class MoveFootball : MonoBehaviour
{
    [Header("Movement Settings")]
    public Direction moveDirection = Direction.Right;
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float amountOfInclination = 5f;
    
  
    

    void Update()
    {

        Vector3 baseDirection = GetDirectionVector(moveDirection) * speed;

       // Vector3 forwardDir = Vector3.right * speed;
        Vector3 verticalWave = new Vector3(0f, Mathf.Sin(Time.time) * amountOfInclination, 0f);
        Vector3 direction = baseDirection + verticalWave;

        transform.position += direction * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private Vector3 GetDirectionVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Vector3.forward;
            case Direction.Right: return Vector3.right;
            case Direction.Up: return Vector3.up;
            default: return Vector3.right;
        }
    }

}

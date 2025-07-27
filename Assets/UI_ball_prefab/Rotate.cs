using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public float rotationAngle = 45f;
    public GameObject ball;
    public float ballSpeed = 1.5f;
    private float t = 0f;
    
    void Update(){
        if (t < 1) {
            t += Time.deltaTime * rotationSpeed;
            t = Mathf.Clamp01(t);
            
            float angle = Mathf.Lerp(0f, rotationAngle, t);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        ball.transform.position += transform.forward * -ballSpeed * Time.deltaTime;
    }
}

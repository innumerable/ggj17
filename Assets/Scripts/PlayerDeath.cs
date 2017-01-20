using UnityEngine;

class PlayerDeath : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            Debug.Log("Zap"); // Do restart logic here
        }
    }
}

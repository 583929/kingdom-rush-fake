using UnityEngine;

public class GameManagerTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameManager.instance.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.instance.AddMoney(10);
        }

       
    }
}
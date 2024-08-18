using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 2;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int amount)
    {
        this.health -= amount;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }    
}

using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed = 120;
    private Rigidbody enemyRb;
    private GameObject playerGoal;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player Goal");

        if (speed <= 0)
        {
            speed = 120;
        }
    }

    void Update()
    {
        if (playerGoal == null || enemyRb == null)
        {
            return;
        }

        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Enemy Goal" || other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}

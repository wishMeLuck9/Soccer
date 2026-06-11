using System.Collections;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    private ParticleSystem boostParticle;
    private Coroutine powerupCooldownRoutine;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    public float boostForce = 20;

    private float normalStrength = 10;
    private float powerupStrength = 25;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        GameObject boostEffect = GameObject.Find("Smoke_Particle");
        if (boostEffect != null)
        {
            boostParticle = boostEffect.GetComponentInChildren<ParticleSystem>();
        }

        if (powerupIndicator != null)
        {
            powerupIndicator.SetActive(false);
        }
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (focalPoint != null)
        {
            playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime);
        }

        if (powerupIndicator != null)
        {
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && focalPoint != null)
        {
            playerRb.AddForce(focalPoint.transform.forward * boostForce, ForceMode.Impulse);

            if (boostParticle != null)
            {
                boostParticle.transform.position = transform.position;
                boostParticle.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;

            if (powerupIndicator != null)
            {
                powerupIndicator.SetActive(true);
            }

            if (powerupCooldownRoutine != null)
            {
                StopCoroutine(powerupCooldownRoutine);
            }

            powerupCooldownRoutine = StartCoroutine(PowerupCooldown());
        }
    }

    private IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;

        if (powerupIndicator != null)
        {
            powerupIndicator.SetActive(false);
        }

        powerupCooldownRoutine = null;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (enemyRigidbody == null)
        {
            return;
        }

        Vector3 awayFromPlayer = (other.transform.position - transform.position).normalized;
        float strength = hasPowerup ? powerupStrength : normalStrength;
        enemyRigidbody.AddForce(awayFromPlayer * strength, ForceMode.Impulse);
    }
}

using UnityEngine;

public class ArrowBowScript : MonoBehaviour
{


    // This script is attached to the arrow bow object in the game.
    private Rigidbody myBody;

    public float arrowSpeed = 20f; // Speed of the arrow when shot

    public float deactiveTime = 5f; // Time after which the arrow will be deactivated

    public float damage = 10f; // Damage dealt by the arrow

  

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DeactivateGameObject", deactiveTime);

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ShootArrow(Camera mainCamera)
    {
        myBody.linearVelocity = Camera.main.transform.forward * arrowSpeed;

        transform.LookAt(transform.position + myBody.linearVelocity);
    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }


    void OTriggerEnter(Collider other)
    {

    }
    



}

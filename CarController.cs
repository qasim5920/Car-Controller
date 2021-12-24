
using UnityEngine;

public class CarController : MonoBehaviour
{
  

    [SerializeField]  Rigidbody rb;
    [SerializeField]  float forwardAccel, reverseAccel,turnStrength;
    [SerializeField] float speedInput, turnInput;
    float gravityForce = 10f;
    [SerializeField] Transform lfw, rfw;
    [SerializeField] float maxWheelTurn;
    [SerializeField] AudioSource aud;
    bool grounded;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float groundRayLength = 0.5f;
    [SerializeField] Transform raypoint;
    [SerializeField] ParticleSystem l_ParticleSystem;
    [SerializeField] ParticleSystem r_ParticleSystem;
    [SerializeField] GameObject lfwAnim;
    [SerializeField] GameObject rfwAnim;
    [SerializeField] GameObject lbwAnim;
    [SerializeField] GameObject rbwAnim;

    private void Start()
    {
        rb.transform.parent = null;
    }
    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;
        if (Physics.Raycast(raypoint.position, -transform.up, out hit, groundRayLength, whatIsGround)) 
        {
            grounded = true;
        }
        if (grounded)
        {

            if (Mathf.Abs(speedInput) > 0)
            {
                rb.AddForce(transform.forward * speedInput);
            }
            else {
                rb.AddForce(Vector3.up * -gravityForce*100f);
            }

        }
        speedInput = 0;
        if (Input.GetAxis("Vertical") > 0) {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;
            l_ParticleSystem.Play();
            r_ParticleSystem.Play();
            lfwAnim.GetComponent<Animator>().enabled = true;
            rfwAnim.GetComponent<Animator>().enabled = true;
            lbwAnim.GetComponent<Animator>().enabled = true;
            rbwAnim.GetComponent<Animator>().enabled = true;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 1000f;
            lfwAnim.GetComponent<Animator>().enabled = true;
            rfwAnim.GetComponent<Animator>().enabled = true;
            lbwAnim.GetComponent<Animator>().enabled = true;
            rbwAnim.GetComponent<Animator>().enabled = true;
        }
        turnInput = Input.GetAxis("Horizontal");
        if (grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }
        transform.position = rb.transform.position;
        if (Input.GetAxis("Vertical") == 0 && l_ParticleSystem.isPlaying)
        {
            l_ParticleSystem.Stop();
        }
        if (Input.GetAxis("Vertical") == 0 && r_ParticleSystem.isPlaying)
        {
            r_ParticleSystem.Stop();
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            lfwAnim.GetComponent<Animator>().enabled = false;
            rfwAnim.GetComponent<Animator>().enabled = false;
            lbwAnim.GetComponent<Animator>().enabled = false;
            rbwAnim.GetComponent<Animator>().enabled = false;
        }

        lfw.localRotation = Quaternion.Euler(lfw.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), lfw.localRotation.eulerAngles.z);
        rfw.localRotation = Quaternion.Euler(rfw.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rfw.localRotation.eulerAngles.z);
        if (Input.GetAxis("Horizontal") != 0)
        {
            lfwAnim.GetComponent<Animator>().enabled = false;
            rfwAnim.GetComponent<Animator>().enabled = false;

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            aud.Play();


            if (Input.GetKeyUp(KeyCode.W))
            {
                aud.Stop();
            }
            
        }
    }



}
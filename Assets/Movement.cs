using UnityEngine;
using System.Collections;

public class ZeroGMovement : MonoBehaviour
{
    [Header("Movement")]
    public float shuffleSpeed = 3f;      
    public float pushForce = 5f;         

    [Header("Grapple")]
    public Transform cameraTransform;
    public float grappleRange = 10f;
    public float maxRopeLength = 30f;
    public float grapplePullSpeed = 10f;
    public string grappleTag = "Grapple";
    public LineRenderer grappleLine;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody rb;
    private bool nearWall = false;
    private Vector3 wallNormal;

    private bool grappling = false;
    private bool grappleActive = false;
    private Vector3 grapplePoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        if (grappleLine != null)
            grappleLine.positionCount = 0;
    }

    void Update()
    {
        DetectWall();
        HandleInput();
        UpdateGrappleLine();
    }

    void FixedUpdate()
    {
        if (nearWall)
            WallShuffle();

        if (grappling && grappleActive)
            GrappleMove();
        // Momentum carries the player naturally
    }

    void HandleInput()
    {
        // Start grapple
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(DoGrappleSequence());

        // Release grapple mid-pull
        if (Input.GetMouseButtonUp(0))
        {
            grappling = false;
            grappleActive = false;
            if (grappleLine != null)
                grappleLine.positionCount = 0;
        }

        // Push off wall
        if (nearWall && Input.GetKeyDown(KeyCode.Space))
            rb.velocity += cameraTransform.forward * pushForce;
    }

    void DetectWall()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 1f))
        {
            nearWall = true;
            wallNormal = hit.normal;
        }
        else
        {
            nearWall = false;
            wallNormal = Vector3.zero;
        }
    }

    void WallShuffle()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Fixed A/D directions
        Vector3 wallRight = Vector3.Cross(wallNormal, Vector3.up).normalized;
        Vector3 wallUp = Vector3.Cross(wallNormal, wallRight).normalized;
        Vector3 moveDir = wallRight * h + wallUp * v;

        rb.velocity = moveDir * shuffleSpeed;
    }

    private IEnumerator DoGrappleSequence()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, grappleRange))
        {
            if (!hit.collider.CompareTag(grappleTag)) yield break;

            grapplePoint = hit.point;
            grappling = true;
            grappleActive = false;

            // Stage 1: play Grapple animation
            if (animator != null)
                animator.Play("Grapple");

            // Stage 2: wait 0.25s, show rope
            yield return new WaitForSeconds(0.25f);
            if (grappleLine != null)
            {
                grappleLine.positionCount = 2;
                grappleLine.SetPosition(0, transform.position);
                grappleLine.SetPosition(1, grapplePoint);
            }

            // Stage 3: wait another 0.25s, start pulling
            yield return new WaitForSeconds(0.25f);
            grappleActive = true;

            // Wait until grapple ends
            while (grappling) yield return null;

            // Reset animation to IdleDown
            if (animator != null)
                animator.Play("IdleDown");
        }
    }

    void GrappleMove()
    {
        Vector3 dir = grapplePoint - transform.position;
        float distance = dir.magnitude;

        if (distance > 0.5f && distance < maxRopeLength)
        {
            rb.velocity = dir.normalized * grapplePullSpeed;
        }
        else
        {
            grappling = false;
            grappleActive = false;
            if (grappleLine != null)
                grappleLine.positionCount = 0;
        }
    }

    void UpdateGrappleLine()
    {
        if (grappling && grappleLine != null)
        {
            grappleLine.SetPosition(0, transform.position);
            grappleLine.SetPosition(1, grapplePoint);
        }
    }
}

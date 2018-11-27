using UnityEngine;
using System.Collections;

public class Player : Character {

    public float jumpForce, fallVelocity, lowJumpVelocity;
    public float shootCD;
    public float grenadeCD;

    public Transform aim;
    public Transform bezierPoint;
    public Transform destinationPoint;
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public LayerMask ground;

    private bool onPlace = false;
    private bool canShoot = true;
    private bool canThrowGrendade = true;
    private bool isGrounded;
    private float startTime;
    private float timeToDestination = 0.5f;
    private float horizontal, vertical;

    private UIManager ui;
    private VirtualJoystick VJ;
    private Vector3 forward, right;
    private Rigidbody rb;

    protected override void Awake() {
        ui = FindObjectOfType<UIManager>();
        rb = GetComponent<Rigidbody>();
        VJ = FindObjectOfType<VirtualJoystick>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        base.Awake();
    }

    void Update() {
        Jump();
        Move();
        SmoothFall();
        Shoot();
        ThrowGrenade();
        if (Input.GetKeyDown(KeyCode.L))
            ReceiveDamage(10);
    }

    public void Jump() {
        if (isGrounded && ui.jumpButton.IsPressed == true) {
            rb.velocity = Vector3.up * jumpForce;
        }
    }

    private void SmoothFall() {
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallVelocity - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && ui.jumpButton.IsPressed == false)
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpVelocity - 1) * Time.deltaTime;
    }

    public void Shoot() {
        if (ui.shootButton.IsPressed == true && canShoot) {
            canShoot = false;
            StartCoroutine(ShootCD());
            Instantiate(bulletPrefab, aim.position, aim.rotation);
        }
    }

    public void ThrowGrenade() {
        if (ui.grenadeButton.IsPressed == true && canThrowGrendade)
            StartCoroutine(GrenadeBezier());
    }

    private IEnumerator GrenadeBezier() {
        Vector3 origin = aim.position;
        Vector3 maxHeight = bezierPoint.position;
        Vector3 target = destinationPoint.position;
        startTime = Time.time;
        canThrowGrendade = false;
        float percent = 0;
        float currentTime = 0;
        GameObject newGrenade = Instantiate(grenadePrefab, aim.position, aim.rotation);
        while (percent < 1) {
            currentTime = Time.time - startTime;
            percent = currentTime / timeToDestination;
            Vector3 A = Vector3.Lerp(origin, maxHeight, percent);
            Vector3 B = Vector3.Lerp(maxHeight, target, percent);
            newGrenade.transform.position = Vector3.Lerp(A, B, percent);
            yield return null;
        }
        StartCoroutine(GrendadeCD());
    }

    public void Move() {

        horizontal = VJ.VJHorizontalAxis();
        vertical = VJ.VJVerticalAxis();

        Vector3 rightMovement = right * horizontal;
        Vector3 upMovement = forward * vertical;
        Vector3 towardsDir = Vector3.Normalize(rightMovement + upMovement);

        if (towardsDir != Vector3.zero) {
            transform.forward = towardsDir;
        }
        transform.position += towardsDir * speed * Time.deltaTime;
    }

    private IEnumerator GrendadeCD() {
        yield return new WaitForSeconds(grenadeCD);
        canThrowGrendade = true;
    }

    private IEnumerator ShootCD() {
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }

    private void OnCollisionStay(Collision col) {
        if (((1 << col.gameObject.layer) & ground) != 0) {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision col) {
        if (((1 << col.gameObject.layer) & ground) != 0) {
            isGrounded = false;
        }
    }

    protected override void ReceiveDamage(float damage) {
        base.ReceiveDamage(damage);
        float hpPercent = currentHP / maxHP;
        UIManager.instance.SetHPBar(hpPercent);
    }

    protected override void Death() {
        //anim.Play("Death");
        GameManager.instance.Restart();
    }
}

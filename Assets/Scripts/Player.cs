using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

    public float speed;
    public float shootCD = 1;
    public float grenadeCD = 1;

    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public Transform aim;
    public Transform bezierPoint;
    public Transform destinationPoint;

    private bool onPlace = false;
    private bool canShoot = true;
    private bool canThrowGrendade = true;
    private float startTime;
    private float timeToDestination = 0.5f;

    private VirtualJoystick VJ;
    private Vector3 forward, right;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        VJ = FindObjectOfType<VirtualJoystick>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void Update() {
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
            Shoot();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canThrowGrendade)
            StartCoroutine(ThrowGrenade()); ;

    }

    private void Shoot() {
        canShoot = false;
        StartCoroutine(ShootCD());
        Instantiate(bulletPrefab,aim.position,aim.rotation);
    }

    private IEnumerator ThrowGrenade() {
        Vector3 origin = aim.position;
        Vector3 maxHeight = bezierPoint.position;
        Vector3 target = destinationPoint.position;
        startTime = Time.time;
        canThrowGrendade = false;
        float percent = 0;
        float currentTime = 0;
        GameObject newGrenade = Instantiate(grenadePrefab,aim.position,aim.rotation);
        while (percent < 1) {
            print(percent);
            currentTime = Time.time - startTime;
            percent = currentTime / timeToDestination;
            print(percent);
            Vector3 A = Vector3.Lerp(origin, maxHeight, percent);
            Vector3 B = Vector3.Lerp(maxHeight, target, percent);
            newGrenade.transform.position = Vector3.Lerp(A, B, percent);
            yield return null;
        }
        StartCoroutine(GrendadeCD());
    }

    private void Move() {
        float horizontal = VJ.VJHorizontalAxis();
        float vertical = VJ.VJVerticalAxis();

        Vector3 rightMovement = right * horizontal;
        Vector3 upMovement = forward * vertical;
        Vector3 towardsDir = Vector3.Normalize(rightMovement + upMovement);

        if(towardsDir != Vector3.zero) {
            transform.forward = towardsDir;
        }
        rb.velocity = towardsDir * speed;
    }

    private IEnumerator GrendadeCD() {
        yield return new WaitForSeconds(grenadeCD);
        canThrowGrendade = true;
    }

    private IEnumerator ShootCD() {
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }
}

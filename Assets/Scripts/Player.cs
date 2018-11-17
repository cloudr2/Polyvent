using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;
    private VirtualJoystick VJ;

    void Awake() {
        VJ = FindObjectOfType<VirtualJoystick>();
    }

    void Update() {
        float horizontal = VJ.VJHorizontalAxis();
        float vertical = VJ.VJVerticalAxis();

        Vector3 direction = new Vector3(horizontal,0,vertical);
        transform.position += direction * speed * Time.deltaTime;
    }

}

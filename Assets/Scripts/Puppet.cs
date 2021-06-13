using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : MonoBehaviour {

    Animator animator;
    [Range(0f, 1f)]
    public float lowPush = 0.3f, highPush = 0.7f;

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    public void Move(Vector3 input, Vector3 direction) {
        float angle = Vector3.Angle(input, direction);

        if (angle <= lowPush * 180f) {
            animator.SetInteger("resistance", 0);
        } else if (angle >= highPush * 180f) {
            animator.SetInteger("resistance", 2);
        } else {
            animator.SetInteger("resistance", 1);
        }
    }

    public void Stop() {
        animator.SetBool("moving", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Puppet : MonoBehaviour {

    Animator animator;

    public float lowSpeed = 1f, highSpeed = 3f;
    [Space(5)]

    public StarterAssetsInputs input;
    public Rigidbody player;
    [Space(5)]

    public GameObject treat;
    public float dogDelay, dogDistance;

    public ParticleSystem sweat;

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    public void Update() {
        
    }

    private void FixedUpdate() {
        float speed = player.velocity.magnitude;

        if (input.move != Vector2.zero) {
            Vector3 inputDirection = new Vector3(-input.move.x, 0.0f, -input.move.y).normalized;
            float angle = Vector3.Angle(inputDirection, player.velocity.normalized);

            if (angle >= 90f) {
                Move(2);
            } else {
                if (player.velocity.magnitude <= lowSpeed) {
                    Move(2);
                } else if (player.velocity.magnitude >= highSpeed ) {
                    Move(0);
                } else {
                    Move(1);
                }
            }
        } else {
            Stop();
        } 
    }

    void Move(int level) {
        animator.SetBool("moving", true);
        animator.SetInteger("resistance", level);
        if (level == 2) {
            if (sweat != null) sweat.Play();
            Vector3 buffer = Vector3.zero;

            foreach (GameObject dog in RoundManager.instance.dogsList) {
                buffer += dog.transform.position - this.transform.position;
            }

            animator.transform.forward = -buffer.normalized;
        } else {
            animator.transform.forward = this.transform.parent.forward;
            if (sweat != null) sweat.Stop();
        }
    }

    void Stop() {
        animator.SetBool("moving", false);
        animator.SetBool("moved", player.velocity.magnitude >= lowSpeed);

        if (player.velocity.magnitude >= (highSpeed *2f + lowSpeed * 3f) / 5f) {
            animator.SetBool("moved", true);
            Vector3 buffer = Vector3.zero;

            if (sweat != null) sweat.Play();
            foreach (GameObject dog in RoundManager.instance.dogsList) {
                buffer += dog.transform.position - this.transform.position;
            }

            animator.transform.forward = -buffer.normalized;
        } else {
            if (sweat != null) sweat.Stop();
            animator.SetBool("moved", false);
            animator.transform.forward = this.transform.parent.forward;
        }

    }

    public void Dance() {
        //Instantiate(treat, transform);
        foreach(GameObject dog in RoundManager.instance.dogsList) {
            StartCoroutine(Dance(dog));
        }
    }


    IEnumerator Dance(GameObject dog) {
        float danceDelay = Random.Range(0f, 0.5f);
        yield return new WaitForSeconds(danceDelay);
        dog.GetComponent<Animation>().Play();
    }
}

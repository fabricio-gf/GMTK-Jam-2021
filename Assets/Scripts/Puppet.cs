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

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    public void Update() {
        
    }

    private void FixedUpdate() {
        float speed = player.velocity.magnitude;

        if (input.move != Vector2.zero) {
            animator.SetBool("moving", true);
            Vector3 inputDirection = new Vector3(-input.move.x, 0.0f, -input.move.y).normalized;
            float angle = Vector3.Angle(inputDirection, player.velocity.normalized);

            if (angle >= 90f) {
                animator.SetInteger("resistance", 2);
            } else {
                if (player.velocity.magnitude <= lowSpeed) {
                    animator.SetInteger("resistance", 2);
                } else if (player.velocity.magnitude >= highSpeed ) {
                    animator.SetInteger("resistance", 0);
                } else {
                    animator.SetInteger("resistance", 1);
                }
            }
        } else {
            animator.SetBool("moving", false);
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

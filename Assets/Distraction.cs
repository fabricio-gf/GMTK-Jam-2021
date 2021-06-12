using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Distraction : MonoBehaviour
{

    [Tooltip("Strength of attraction to dogs.")]
    [Range(0.1f, 5.0f)]
    public float weight = 1.0f;

    [Tooltip("Max number of dogs this distraction can hold; 0 for no limit.")]
    public int maxDogs = 0;

    public int currentDogs = 0;

    private void OnTriggerEnter(Collider other) {
        var newDog = other.GetComponent<Dog>();
        if (newDog == null) {
            // Collided object is not a dog
            return;
        }

        if (maxDogs > 0 && currentDogs >= maxDogs) {
            // There is no room for more dogs
            return;
        }

        // Weight of previous distraction, or 0 if there was none
        var prevDistractionWeight = newDog.Target?.weight ?? 0;
        if (prevDistractionWeight > weight) {
            // Previous distraction was more interesting
            return;
        }

        // This should replace previous distraction, if any
        newDog.Target = this;
    }

    private void OnTriggerExit(Collider other) {
        var dog = other.GetComponent<Dog>();
        if (dog == null) {
            // Object is not a dog
            return;
        }

        if (dog.Target != this) {
            // Dog is distracted by something else
            return;
        }

        // Dog is now out of range
        dog.Target = null;
    }

    private void LateUpdate() {
        if (maxDogs > 0 && currentDogs > maxDogs) {
            var pos = transform.position;
            Debug.LogError($"Distraction \"{gameObject.name}\" has more dogs than it supports: {currentDogs} > {maxDogs} ({pos.x}, {pos.y}, {pos.z}).");
        }
        if (currentDogs < 0) {
            var pos = transform.position;
            Debug.Log($"Distraction \"{gameObject.name}\" has negative dogs ({pos.x}, {pos.y}, {pos.z}).");
        }
    }
}

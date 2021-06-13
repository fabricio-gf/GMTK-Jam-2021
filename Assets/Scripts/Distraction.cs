using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Distraction : MonoBehaviour
{

    [Tooltip("Strength of attraction to dogs.")]
    [Range(0.1f, 5.0f)]
    public float weight = 1.0f;

    [Tooltip("Probability this will attract a dog when it enters its area.")]
    [Range(0.0f, 1.0f)]
    public float attractProbability = 1.0f;

    [Tooltip("Max number of dogs this distraction can hold; 0 for no limit.")]
    public int maxDogs = 0;

    public int currentDogs = 0;

    public UnityEvent followUpFunction;

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

        if (Random.value >= attractProbability) {
            // Randomly decided not to attract this dog
            return;
        }

        // This should replace previous distraction, if any
        print(gameObject.name + " - ATTRACT DOGGO");
        newDog.Target = this;
        followUpFunction?.Invoke();
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
            Debug.LogError($"Distraction \"{gameObject.name}\" has negative dogs ({pos.x}, {pos.y}, {pos.z}).");
        }
    }
}

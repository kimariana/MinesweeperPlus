using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ObjectScript : MonoBehaviour
{
    public ObjectSpawner objSpawner;
    public GameObject gameArea;

    public float speed;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move() // Movement of the object
    {
        transform.position += transform.up * (Time.deltaTime * speed);

        float distance = Vector3.Distance(transform.position, gameArea.transform.position);
        if(distance > objSpawner.deathCircleRadius) {
            RemoveObj();
        }
    }

    void RemoveObj()
    {
        Destroy(gameObject); // Destroys object 
        objSpawner.objCount -= 1;
    }

    void OnTriggerEnter(Collider other) {
        // When the trigger is hit by another object
        if(other.transform.tag == "Object") {
            // Set the object to a random color
            Material mat = GetComponent<Renderer>().material;
            Color newC = new Color(Random.value, Random.value, Random.value, 1.0f);
            mat.color = newC;
        }
    }
}

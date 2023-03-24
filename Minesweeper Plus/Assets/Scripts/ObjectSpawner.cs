using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject gameArea;
    public GameObject objPrefab;

    public int objCount = 0;
    public int objLimit = 10;
    public int objPerFrame = 1;

    public float spawnCircleRadius = 8.0f;
    public float deathCircleRadius = 10.0f;
    
    public float fastestSpeed = 4.0f;
    public float slowestSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        InitialPop();
    }

    // Update is called once per frame
    void Update()
    {
        MaintainPop();
    }

    void InitialPop()
    {
        for(int i = 0; i < objLimit; i++) {
            Vector3 position = GetRandPos(true);
            ObjectScript objScript = AddObj(position);
            objScript.transform.Rotate(Vector3.forward * Random.Range(0.0f,360.0f));
        }
    }

    void MaintainPop()
    {
        if(objCount < objLimit) {
            for(int i = 0; i < objPerFrame; i++) {
                Vector3 position = GetRandPos(false);
                ObjectScript objScript = AddObj(position);
                objScript.transform.Rotate(Vector3.forward * Random.Range(-45.0f,45.0f));
            }
        }
    }
    Vector3 GetRandPos(bool withinCam)
    {
        Vector3 position = Random.insideUnitCircle;

        if(!withinCam) {
            position = position.normalized;
        }

        position *= spawnCircleRadius;
        position += gameArea.transform.position;

        return position;
    }

    ObjectScript AddObj(Vector3 position)
    {
        objCount += 1;
        GameObject newObj = Instantiate(
            objPrefab, position,
            Quaternion.FromToRotation(Vector3.up, (gameArea.transform.position-position)),
            gameObject.transform);

        ObjectScript objScript = newObj.GetComponent<ObjectScript>();
        objScript.objSpawner = this;
        objScript.gameArea = gameArea;
        objScript.speed = Random.Range(slowestSpeed, fastestSpeed);

        return objScript;
    }
}

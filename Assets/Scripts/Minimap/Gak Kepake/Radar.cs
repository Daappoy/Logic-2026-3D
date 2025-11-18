using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Radar : MonoBehaviour
{
    [SerializeField] private Transform radarPing;
    private Transform SweepTransform;
    public float rotationSpeed;
    public float radarDistance;
    private List<Collider2D> colliderList;

    void Awake()
    {
        SweepTransform = transform.Find("Sweep");
        colliderList = new List<Collider2D>();
    }
    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void Update()
    {
        float previousRotation = (SweepTransform.eulerAngles.z % 360) - 180;
        SweepTransform.eulerAngles -= new Vector3(0,0, rotationSpeed * Time.deltaTime);
        float currentRotation = (SweepTransform.eulerAngles.z % 360) - 180;
        //make sure ini cuma detect enemy doang

        if(previousRotation < 0 && currentRotation >= 0)
        {
            colliderList.Clear();
        }

        RaycastHit2D[] raycastHit2DArray = Physics2D.RaycastAll(transform.position, GetVectorFromAngle(SweepTransform.eulerAngles.z),radarDistance, LayerMask.GetMask("Enemy") );
        foreach(RaycastHit2D raycastHit2D in raycastHit2DArray)
        {   
            if(raycastHit2D.collider != null)
            {
                if(!colliderList.Contains(raycastHit2D.collider))
                {
                    colliderList.Add(raycastHit2D.collider);
                    Debug.Log("Detected " + raycastHit2D.collider.name);
                    radarPing RadarPing = Instantiate(radarPing, raycastHit2D.point, Quaternion.identity).GetComponent<radarPing>();
                }
            }
        }
    }
}

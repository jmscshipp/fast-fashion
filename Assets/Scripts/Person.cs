using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField]
    private Material baseMat;
    private Material myMat;

    // Start is called before the first frame update
    void Awake()
    {
        myMat = new Material(baseMat);
        GetComponent<MeshRenderer>().material = myMat;
    }

    public void ChangeMatColor(Color newColor)
    {
        myMat.color = newColor;
    }
    
    public float Walk(float walkSpeed)
    {
        transform.Translate(Vector3.down * walkSpeed * Time.deltaTime);
        return transform.position.y;
    }

    public void Kill()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}

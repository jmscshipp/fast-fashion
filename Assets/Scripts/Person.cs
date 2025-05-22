using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField]
    private Material baseMat;
    private Material myMat;
    public bool markedForRemoval = false;

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
    
    public Vector3 Move(Vector3 moveDir)
    {
        transform.Translate(moveDir);
        return transform.position;
    }

    public void Kill()
    {
        markedForRemoval = true;
        GetComponent<MeshRenderer>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private bool hasStudent = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void SetStudentSeat()
    {
        hasStudent = true;
    }
    public void SetStudentOut()
    {
        hasStudent = false;
    }

    public bool GetHasStudent()
    {
        return hasStudent;
    }
}

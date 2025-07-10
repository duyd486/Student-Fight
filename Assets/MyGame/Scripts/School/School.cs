using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class School : MonoBehaviour
{
    public static School Instance { get; private set; }

    [SerializeField] private List<Classroom> classes;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Seat GetSeat()
    {
        foreach(Classroom cls in classes)
        {
            if (cls.CheckClassroomSeat())
            {
                Debug.Log("Has classroom seat");
                return cls.CheckEmtySeat();
            }
        }
        Debug.Log("Seat null");
        return null;
    }

    public void GetClassroom()
    {

    }
}

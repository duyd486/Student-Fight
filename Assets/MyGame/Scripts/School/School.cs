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
        //Time.timeScale = 3f;
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
        //foreach(Classroom cls in classes)
        //{
        //    if (cls.CheckClassroomSeat())
        //    {
        //        Debug.Log("Has classroom seat");
        //        return cls.CheckEmtySeat();
        //    }
        //}
        //Debug.Log("Seat null");
        //return null;
        List<Seat> validSeats = new List<Seat>();

        foreach (Classroom cls in classes)
        {
            if (cls.CheckClassroomSeat())
            {
                Seat seat = cls.CheckEmtySeat();
                if (seat != null)
                    validSeats.Add(seat);
            }
        }

        if (validSeats.Count > 0)
        {
            int randomIndex = Random.Range(0, validSeats.Count);
            Debug.Log("Random seat selected");
            return validSeats[randomIndex];
        }

        Debug.Log("Seat null");
        return null;
    }

    public void GetClassroom()
    {

    }
}

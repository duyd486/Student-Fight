using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classroom : MonoBehaviour
{
    [SerializeField] private List<Seat> seats;

    // Start is called before the first frame update
    void Start()
    {
        CheckEmtySeat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Seat CheckEmtySeat()
    {
        foreach (Seat seat in seats)
        {
            if (!seat.GetHasStudent())
            {
                return seat;
            }
        }
        return null;
    }
    public bool CheckClassroomSeat()
    {
        bool isHasSeat = false;
        foreach (Seat seat in seats)
        {
            if (!seat.GetHasStudent())
            {
                return true;
            }
        }
        return isHasSeat;
    }
}

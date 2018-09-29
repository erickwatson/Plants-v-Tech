using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace grid
{
    public enum POINTSTATE
    {
        EMPTY,
        SOLID
    }

    public class GridPoints : MonoBehaviour
    {
        public void Start()
        {
            Debug.Log("Create New Points Array");
            gridPoints = new POINTSTATE[8, 8];
            for (int x = 0; x < 8; x++)            // clamping grid's x-axis size
            {
                for (int z = 0; z < 8; z++)        // clamping grid's z-axis size
                {
                    gridPoints[x, z] = POINTSTATE.EMPTY;
                }
            }
        }
        public POINTSTATE[,] gridPoints;
    }
}




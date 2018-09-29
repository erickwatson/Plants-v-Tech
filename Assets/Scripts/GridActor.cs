using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace grid
{
    public enum Player
    {
        P_1,
        P_2,
    }

    public class GridActor : MonoBehaviour
    {
        public int size = 10;
        public GridPoints points;
        public GameObject tree_prefab;
        public GameObject tech_prefab;
        public Player player_type;
        private GameObject prefab;
        public Text player;

        public GameObject DebugBox;

        private void Start()
        {
            prefab = tree_prefab;
        }

        private void Update()
        {
            DebugBoxDraw();
        }

        public void DebugBoxDraw()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))      // Selects what was pressed based on cursor position
            {
                Vector3 hitPoint = hitInfo.point;
                hitPoint.x -= (size / 2);
                hitPoint.y -= (size / 2);

                var finalPosition = GetNearestPointOnGrid(hitPoint);         // find the closest grid point to the cursor click

                int x = Mathf.RoundToInt(finalPosition.x) + (size / 2);
                int z = Mathf.RoundToInt(finalPosition.z) + (size / 2);

                DebugBox.transform.position = new Vector3(x, 0, z);
            }
        }

        public Vector3 GetNearestPointOnGrid(Vector3 pos)
        {
            pos -= transform.position;

            // rounding each position axis into an int, and dividing by grid size
            int xCount = Mathf.RoundToInt(pos.x / size);        // x-axis
            int yCount = Mathf.RoundToInt(pos.y / size);        // y-axis
            int zCount = Mathf.RoundToInt(pos.z / size);        // z-axis

            Vector3 result = new Vector3(       // duh
                (float)xCount * size,           // duh
                (float)yCount * size,           // duh
                (float)zCount * size);          // duh

            result += transform.position;   // finalizes position

            return result;
        }

        public void PlaceObjectNear(Vector3 clickPoint)
        {
            var finalPosition = GetNearestPointOnGrid(clickPoint);         // find the closest grid point to the cursor click

            int x = Mathf.RoundToInt(finalPosition.x / size + (size / 2));
            int z = Mathf.RoundToInt(finalPosition.z / size + (size / 2));
            x -= 1;
            z -= 1;

            x = Mathf.Clamp(x, 0, 7);
            z = Mathf.Clamp(z, 0, 7);

            POINTSTATE p = points.gridPoints[x, z];
            Debug.Log("Coord: " + x + ", " + z + " - Pointstate: " + p);
            if (p == POINTSTATE.EMPTY)
            {
                if (x < 8 && z < 8)
                {
                    if ((finalPosition + new Vector3((size / 2), 0, (size / 2))).z <= 44)
                    {
                        Debug.Log("Spawning");
                        Instantiate(prefab, finalPosition + new Vector3((size / 2), 0, (size / 2)), Quaternion.identity);       // creates gameobject from prefab
                    }
                    else
                    {
                        Debug.Log("Spawning");
                        Instantiate(prefab, finalPosition + new Vector3((size / 2), 0, -5), Quaternion.identity);       // creates gameobject from prefab
                    }
                }
                switch (player_type)
                {
                    case Player.P_1:
                        {
                            player_type = Player.P_2;
                            prefab = tech_prefab;
                            p = POINTSTATE.SOLID;
                            points.gridPoints[x, z] = POINTSTATE.SOLID;
                            player.text = "2";
                            break;
                        }
                    case Player.P_2:
                        {
                            player_type = Player.P_1;
                            prefab = tree_prefab;
                            p = POINTSTATE.SOLID;
                            points.gridPoints[x, z] = POINTSTATE.SOLID;
                            player.text = "1";
                            break;
                        }
                }
            }
            else if (p == POINTSTATE.SOLID)
            {

            }
        }

        

        void OnDrawGizmos()
        {
            if (points != null)
            {
                Gizmos.color = Color.grey;
                int arrayX = 0;
                for (int x = -4; x < 4; x += 1)            // clamping grid's x-axis size
                {
                    int arrayZ = 0;
                    for (int z = -4; z < 4; z += 1)        // clamping grid's z-axis size
                    {
                        Vector3 point = new Vector3(x * size + (size /2),0,z * size + (size / 2));// GetNearestPointOnGrid(new Vector3(x, 0f, z));       // finds nearest point on grid
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(point, 0.5f);                                     // indicates centre of each grid point
                                                                                            // Stores grid points as:
                        arrayZ++;                                                           // { 0,0  1,0  2,0  3,0  4,0  5,0  6,0  7,0 }
                    }                                                                       // { 0,1  1,1  2,1  3,1  4,1  5,1  6,1  7,1 }
                    arrayX++;                                                               // { 0,2  1,2  2,2  3,2  4,2  5,2  6,2  7,2 }
                }                                                                           // { 0,3  1,3  2,3  3,3  4,3  5,3  6,3  7,3 }
            }
        }                                                                               // { 0,4  1,4  2,4  3,4  4,4  5,4  6,4  7,4 }
    }                                                                                   // { 0,5  1,5  2,5  3,5  4,5  5,5  6,5  7,5 }
                                                                                        // { 0,6  1,6  2,6  3,6  4,6  5,6  6,6  7,6 }
                                                                                        // { 0,7  1,7  2,7  3,7  4,7  5,7  6,7  7,7 }
}
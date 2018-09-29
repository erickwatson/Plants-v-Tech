using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace grid
{
    public class GameObjectPlacer : MonoBehaviour
    {
        public GridActor grid;
        public GameObject tree_prefab;
        public GameObject tech_prefab;
        //private GridPoints points;

        // Use this for initialization
        private void Awake()
        {
            //grid = FindObjectOfType<GridActor>();
        }

        public Vector3 GetObjectRayCast()
        {
            Vector3 mouse_pos = Input.mousePosition;
            // use the current camera to convert mouse position to a ray
            Ray fire_ray = Camera.main.ScreenPointToRay(mouse_pos);
            // create a plane that faces up at the same position as the player
            Plane player_plane = new Plane(Vector3.up, transform.position);
            // how far along the ray does the intersection with the plane occur?
            float ray_distance = 0;
            player_plane.Raycast(fire_ray, out ray_distance);
            // use the ray distance to calculate the point of collision
            Vector3 cast_point = fire_ray.GetPoint(ray_distance);
            Vector3 to_cast_point = (cast_point - transform.position);
            to_cast_point.Normalize();

            return to_cast_point;
        }

            //POINTSTATE p = points.gridPoints[x, z];
            //Debug.Log("Coord: " + x + ", " + z + " - Pointstate: " + p);
            //if (p == POINTSTATE.EMPTY)
            //{
            //    if (x < 8 && z < 8)
            //    {
            //        if ((finalPosition + new Vector3((size / 2), 0, (size / 2))).z <= 44)
            //        {
            //            Debug.Log("Spawning");
            //            Instantiate(prefab, finalPosition + new Vector3((size / 2), 0, (size / 2)), Quaternion.identity);       // creates gameobject from prefab
            //        }
            //        else
            //        {
            //            Debug.Log("Spawning");
            //            Instantiate(prefab, finalPosition + new Vector3((size / 2), 0, -5), Quaternion.identity);       // creates gameobject from prefab
            //        }
            //    }
            //    switch (player_type)
            //    {
            //        case Player.P_1:
            //            {
            //                player_type = Player.P_2;
            //                prefab = tech_prefab;
            //                p = POINTSTATE.SOLID;
            //                points.gridPoints[x, z] = POINTSTATE.SOLID;
            //                player.text = "2";
            //                break;
            //            }
            //        case Player.P_2:
            //            {
            //                player_type = Player.P_1;
            //                prefab = tree_prefab;
            //                p = POINTSTATE.SOLID;
            //                points.gridPoints[x, z] = POINTSTATE.SOLID;
            //                player.text = "1";
            //                break;
            //            }
            //    }
            //}
            //else if (p == POINTSTATE.SOLID)
            //{

            //}
        //}

        public GameObject GetObject()
        {
            Vector3 raycast = GetObjectRayCast();
            RaycastHit info;
            Ray fire_ray = new Ray(transform.position, raycast);

            if (Physics.Raycast(fire_ray, out info, 1000))
            {
                return info.collider.gameObject;
            }
            return null;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))        // left button clicked
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))      // Selects what was pressed based on cursor position
                {
                    Vector3 hitPoint = hitInfo.point;
                    hitPoint.x -= (grid.size / 2);
                    hitPoint.y -= (grid.size / 2);

                    var finalPosition = grid.GetNearestPointOnGrid(hitPoint);         // find the closest grid point to the cursor click

                    int z = Mathf.RoundToInt(finalPosition.z);
                    int x = Mathf.RoundToInt(finalPosition.x);
                    grid.PlaceObjectNear(new Vector3(x, 0, z));     // places gameObject at the selected grid point
                }
            }
            else if (Input.GetMouseButtonDown(1))        // left button clicked
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))      // Selects what was pressed based on cursor position
                {
                    GameObject obj = GetObject();
                    if (obj != null)
                    {
                        Vector3 finalPosition = grid.GetNearestPointOnGrid(obj.transform.position);         // find the closest grid point to the cursor click
                        int x = Mathf.RoundToInt(finalPosition.x / grid.size + (grid.size / 2));
                        int z = Mathf.RoundToInt(finalPosition.z / grid.size + (grid.size / 2));
                        x -= 1;
                        if (x >= 7)
                        {
                            x = 7;
                        }
                        z -= 1;

                        grid.points.gridPoints[x, z] = POINTSTATE.EMPTY;
                        Destroy(obj);
                    }
                }
            }
        }
    }
}
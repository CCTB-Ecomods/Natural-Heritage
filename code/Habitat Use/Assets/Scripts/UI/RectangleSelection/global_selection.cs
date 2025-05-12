using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class global_selection : MonoBehaviour
{
    selected_dictionary selected_table;
    RaycastHit hit;

    bool dragSelect;

    public float doubleClickTime = 0.3f;
    float doubleClickTimer;
    bool timerEnabled;
    bool areaSelected;
    //Collider variables
    //=======================================================//

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

    //the corners of our 2d selection box
    Vector2[] corners;

    //the vertices of our meshcollider
    Vector3[] verts;

    // Start is called before the first frame update
    void Start()
    {
        selected_table = GetComponent<selected_dictionary>();
        dragSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*bool overUI = EventSystem.current.IsPointerOverGameObject();
        if (overUI) 
            return;*/
        
        if (timerEnabled) doubleClickTimer += Time.deltaTime;
        if (doubleClickTimer > doubleClickTime)
        {
            timerEnabled = false;
            doubleClickTimer = 0;
        }


        //1. when left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(p1);
            if (Physics.Raycast(ray, out hit, 50000.0f))
            {
                if (hit.collider.gameObject.CompareTag("Hexagon")) //area-selection
                {
                    bool overUI = EventSystem.current.IsPointerOverGameObject();
                    if (overUI) return;
                    if (!timerEnabled)
                        timerEnabled = true;
                    else
                    {
                        List<TileData> tileList = hit.collider.gameObject.GetComponent<TileData>().area.GetTileList();
                        
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            selected_table.deselectAll();
                        }
                        List<GameObject> gos = new List<GameObject>();
                        foreach (TileData tile in tileList)
                        {
                            gos.Add(tile.gameObject);
                        }
                        selected_table.addSelected(gos);
                        timerEnabled = false;
                        doubleClickTimer = 0;
                        areaSelected = true;
                    }
                }
            }
        }

        //2. while left mouse button held
        if (Input.GetMouseButton(0))
        {
            if((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        //3. when mouse button comes up
        if (Input.GetMouseButtonUp(0))
        {
            bool overUI = EventSystem.current.IsPointerOverGameObject();
            if (areaSelected)
            {
                areaSelected = false;
                return;
            }
            if (dragSelect == false) //single select
            {
                if (!overUI)
                {
                    Ray ray = Camera.main.ScreenPointToRay(p1);

                    if (Physics.Raycast(ray, out hit, 50000.0f))
                    {

                        if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                        {
                            selected_table.addSelected(hit.transform.gameObject);
                        }
                        else //exclusive selected
                        {
                            selected_table.deselectAll();
                            selected_table.addSelected(hit.transform.gameObject);
                        }
                    }
                    else //if we didnt hit something
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            //do nothing
                        }
                        else
                        {
                            selected_table.deselectAll();
                        }
                    }
                }
            }
            else //marquee select
            {
                verts = new Vector3[4];
                int i = 0;
                p2 = Input.mousePosition;
                corners = getBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);
                    //Debug.DrawLine(ray.origin, ray.direction * 50000000, Color.red, 2);

                    if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8)))
                    {
                        verts[i] = new Vector3(hit.point.x, 0, hit.point.z);
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    selected_table.deselectAll();
                }

                Destroy(selectionBox, 0.02f);
                Invoke("updateTileSettings", 0.02f);
                

            }//end marquee select

            dragSelect = false;

        }
       
    }

    private void updateTileSettings()
    {
        selected_table.updateTileSettings();
    }

    private void OnGUI()
    {
        if(dragSelect == true)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1,Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }


        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }

    //generate a mesh from the 4 bottom points
    Mesh generateSelectionMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for(int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for(int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + Vector3.up * 100.0f;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        selected_table.addSelectedWithoutUIUpdate(other.gameObject);
    }

}

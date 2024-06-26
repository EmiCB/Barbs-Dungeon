using Graphs;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.Tilemaps;
using RandomSys = System.Random;


public class LevelGeneration : MonoBehaviour
{
    enum CellType
    {
        None,
        Room,
        Hallway
    }

    class Room
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    public GameObject room;
    public int amountOfRoom = 200;

    List<GameObject> rooms = new List<GameObject>();
    List<int> sizes = new List<int>();

    public Vector4 startColor = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    public Vector4 endColor = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);

    [SerializeField]
    Vector2Int size;

    private long tick = 0l;
    private int counter3 = 0;

    private int hasFinished = 0;


    RandomSys random;
    Grid2D<CellType> grid;
    List<Room> roomsList;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountOfRoom; i++) 
        {
            GameObject temp = Instantiate(room, new Vector3(0, 0, 0), Quaternion.identity);
            temp.GetComponentInChildren<SpriteRenderer>().color = Vector4.Lerp(startColor, endColor, i/(float)amountOfRoom);

            //if (i < (float) amountOfRoom/2)
            {
                temp.transform.localScale = new Vector3((int) Mathf.Ceil(GuassianRandomRange(8, 3, 25)), (int) Mathf.Ceil(GuassianRandomRange(8, 3, 25)), 1);
            }
            //else
            //{
                //temp.transform.localScale = new Vector3((int)Mathf.Ceil(GuassianRandomRange(2, 3, 7)), (int)Mathf.Ceil(GuassianRandomRange(2, 3, 5)), 1);
            //}
            temp.transform.position = RandomCircle(5.0f) - (temp.transform.localScale * 0.5f);
            rooms.Add(temp);
            sizes.Add((int) temp.transform.localScale.x * (int) temp.transform.localScale.y);
        }
    }

    private Vector3 RandomCircle (float radius)
    {
        return Random.insideUnitCircle * radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFinished == 0) {
            int counter2 = 0;
            for (int i = 0; i < amountOfRoom; ++i) {
                int counter = 0;

                Vector3 iRoomPos = rooms[i].transform.position + (rooms[i].transform.localScale * 0.5f);
                Vector3 sum = iRoomPos;
                for (int j = 0; j < amountOfRoom; ++j) {
                    if (i == j) continue;
                    if (DoesIntersect(rooms[i], rooms[j]))
                    {
                        counter++;
                        Vector3 jRoomPos = rooms[j].transform.position + (rooms[j].transform.localScale * 0.5f);

                        sum += jRoomPos;
                    }
                }
                if (counter == 0) continue;
                counter2++;
                sum = sum / (counter + 1);
                rooms[i].transform.position -= (sum - (rooms[i].transform.localScale * 0.5f) - rooms[i].transform.position).normalized;
                //rooms[i].transform.position = new Vector3(Mathf.Round(rooms[i].transform.position.x), Mathf.Round(rooms[i].transform.position.y), rooms[i].transform.position.z);
            }
            if (counter2 == 0) hasFinished = 1;
        } else if (hasFinished == 1) {
            sizes.Sort();
            for (int i = 0; i < amountOfRoom; ++i) {
                Vector3 roomScale = rooms[i].transform.localScale;
                if (roomScale.x * roomScale.y >= sizes[amountOfRoom - 11]) {
                        rooms[i].GetComponentInChildren<SpriteRenderer>().color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                } else {
                        rooms[i].GetComponentInChildren<SpriteRenderer>().color = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
                }
            }
            hasFinished = 2;
        } 
        else if (hasFinished == 2)
        {
            counter3++;
            if (counter3 == 60)
            {
                List<int> roomsToRemove = new List<int>(); 
                for (int i = 0; i < amountOfRoom; ++i)
                {
                    Vector3 roomScale = rooms[i].transform.localScale;
                    if (roomScale.x * roomScale.y < sizes[amountOfRoom - 11])
                    {
                        Destroy(rooms[i]);
                        roomsToRemove.Insert(0, i);
                    } else
                    {
                        rooms[i].transform.position = new Vector3(Mathf.Round(rooms[i].transform.position.x), Mathf.Round(rooms[i].transform.position.y), rooms[i].transform.position.z);
                    }
                }

                for (int i = 0; i < roomsToRemove.Count; ++i)
                {
                    rooms.RemoveAt(roomsToRemove[i]);
                }
                hasFinished = 3;
            }
        } else if(hasFinished == 3)
        {
            Generate();
            hasFinished = 4;
            GenerateRooms();
        }
    }

    bool DoesIntersect(GameObject a, GameObject b)
    {
        float aLeft = a.transform.position.x;
        float aRight = a.transform.position.x + a.transform.localScale.x + 4;
        float aTop = a.transform.position.y + a.transform.localScale.y + 4;
        float aBottom = a.transform.position.y;

        float bLeft = b.transform.position.x;
        float bRight = b.transform.position.x + b.transform.localScale.x + 4;
        float bTop = b.transform.position.y + b.transform.localScale.y + 4;
        float bBottom = b.transform.position.y;

        return (aLeft < bRight &&
                aRight > bLeft &&
                aTop > bBottom &&
                aBottom < bTop);
    }

    private static float GuassianRandom(float stdDev)
    {
        float f1 = Random.value;
        float f2 = Random.value;

        float randomStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(f1, 10)) * Mathf.Sin(2 * Mathf.PI * f2);
        return randomStdNormal * stdDev;
    }

    private static float GuassianRandomRange(float stdDev, float min, float max)
    {
        float temp = -1f;
        do
        {
            temp = GuassianRandom(stdDev) + min;
        } while (temp < min || temp > max);
        return temp;
    }

    void Generate()
    {
        random = new RandomSys(0);
        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        roomsList = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
    }

    void PlaceRooms()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            Vector2Int location = new Vector2Int(
                (int) rooms[i].transform.position.x + (size.x/2),
                (int) rooms[i].transform.position.y + (size.y/2)
            );

            Vector2Int roomSize = new Vector2Int(
                (int)rooms[i].transform.localScale.x,
                (int)rooms[i].transform.localScale.y
            );

            Room newRoom = new Room(location, roomSize);
            {
                roomsList.Add(newRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    grid[pos] = CellType.Room;
                }
            }
        }
    }

    void Triangulate()
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in roomsList)
        {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways()
    {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges)
        {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges)
        {
            if (random.NextDouble() < 0.125)
            {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways()
    {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

        foreach (var edge in selectedEdges)
        {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
                var pathCost = new DungeonPathfinder2D.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos) * 5;

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 3; j++)
                    {
                        if (i == 0 && i == j) continue;

                        var x = new Vector2Int(b.Position.x + i, b.Position.y + j);
                        if (x.y >= size.y) continue;
                        if (x.y < 0) continue;
                        if (x.x < 0) continue;
                        if (x.x >= size.x) continue;

                        if (grid[x] == CellType.Room)
                        {
                            pathCost.cost += 40;
                        }
                        else if (grid[b.Position] == CellType.Hallway)
                        {
                            pathCost.cost += 10;
                        }
                    }
                }

                if (grid[b.Position] == CellType.Room)
                {
                    pathCost.cost += 40;
                }
                else if (grid[b.Position] == CellType.None)
                {
                    pathCost.cost += 5;
                }
                else if (grid[b.Position] == CellType.Hallway)
                {
                    pathCost.cost = 1;
                }



                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var current = path[i];

                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0)
                    {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }
                
                foreach (var pos in path)
                {
                    if (grid[pos] == CellType.Hallway)
                    {
                        PlaceHallway(pos);
                    }
                }
            }
        }
    }

    List<GameObject> pathCubes = new List<GameObject>();
    void PlaceCube(Vector2Int location, Vector2Int placeSize)
    {
        GameObject go = Instantiate(room, new Vector3(location.x - (size.x/2), location.y - (size.y/2), 0), Quaternion.identity);
        go.GetComponent<Transform>().localScale = new Vector3(placeSize.x, 1, placeSize.y);
        pathCubes.Add(go);
    }

    void PlaceHallway(Vector2Int location)
    {
        PlaceCube(location, new Vector2Int(1, 1));
    }


    public Tile[] wallTiles;
    public Tile[] cornerTiles;
    public Tile[] wallBorderTiles;

    public Tile[] floorTiles;
    public Tile[] connerFloorTiles;
    public Tile[] floorEdgeTiles;

    public Tile[] hallwayEdgeTiles;



    public Tilemap wall;
    public Tilemap floor;
    //public Tile[] wallTiles;


    // Tile Map stuff
    public void GenerateRooms()
    {
        wall.ClearAllTiles();
        floor.ClearAllTiles();

        for (int i = 0; i < rooms.Count; i++)
        {
            {
                // wall
                Vector3Int pos = Vector3Int.FloorToInt(rooms[i].transform.position) * 2;
                Vector3Int roomSize = Vector3Int.FloorToInt(rooms[i].transform.localScale) * 2;

                for (int j = 0; j < roomSize.y - 1; j++)
                {
                    pos.y++;
                    wall.SetTile(pos, wallBorderTiles[3]);
                }
                wall.SetTile(pos, cornerTiles[0]);

                Vector3Int wallPos1 = pos;
                Vector3Int wallPos2 = pos;
                wallPos1.y--;
                wallPos2.y -= 2;

                for (int j = 0; j < roomSize.x - 1; j++)
                {
                    pos.x++;
                    wallPos1.x++;
                    wallPos2.x++;
                    wall.SetTile(pos, wallBorderTiles[0]);
                    wall.SetTile(wallPos1, wallTiles[2]);
                    wall.SetTile(wallPos2, wallTiles[3]);
                }
                wallPos1.x--;
                wallPos2.x--;
                wall.SetTile(wallPos1, wallTiles[4]);
                wall.SetTile(wallPos2, wallTiles[5]);

                wallPos1.x = (int)(rooms[i].transform.position.x * 2) + 1;
                wallPos2.x = wallPos1.x;
                wall.SetTile(wallPos1, wallTiles[0]);
                wall.SetTile(wallPos2, wallTiles[1]);

                wall.SetTile(pos, cornerTiles[1]);
                for (int j = 0; j < roomSize.y - 1; j++)
                {
                    pos.y--;
                    wall.SetTile(pos, wallBorderTiles[1]);
                }

                wall.SetTile(pos, cornerTiles[2]);
                for (int j = 0; j < roomSize.x - 1; j++)
                {
                    pos.x--;
                    wall.SetTile(pos, wallBorderTiles[2]);
                }
                wall.SetTile(pos, cornerTiles[3]);
            }

            {
                //Floor
                Vector3Int pos = Vector3Int.FloorToInt(rooms[i].transform.position) * 2;
                Vector3Int roomSize = Vector3Int.FloorToInt(rooms[i].transform.localScale) * 2;

                pos.x++;
                pos.y++;

                for (int j = 0; j < roomSize.y - 5; j++)
                {
                    pos.y++;
                    floor.SetTile(pos, floorEdgeTiles[3]);
                }
                floor.SetTile(pos, connerFloorTiles[0]);

                for (int j = 0; j < roomSize.x - 3; j++)
                {
                    pos.x++;
                    floor.SetTile(pos, floorEdgeTiles[0]);
                }

                floor.SetTile(pos, connerFloorTiles[1]);
                for (int j = 0; j < roomSize.y - 5; j++)
                {
                    pos.y--;
                    floor.SetTile(pos, floorEdgeTiles[1]);
                }

                floor.SetTile(pos, connerFloorTiles[2]);
                for (int j = 0; j < roomSize.x - 3; j++)
                {
                    pos.x--;
                    floor.SetTile(pos, floorEdgeTiles[2]);
                }
                floor.SetTile(pos, connerFloorTiles[3]);


                pos.x++;
                pos.y++;
                Vector3Int temp = new Vector3Int();
                for (int ii = 0; ii < roomSize.x - 4; ii++)
                {
                    for (int jj = 0; jj < roomSize.y - 6; jj++)
                    {
                        temp.x = pos.x + ii;
                        temp.y = pos.y + jj;

                        floor.SetTile(temp, floorTiles[0]);
                    }
                }

            }

            Destroy(rooms[i]);
        }

        for (int i = 0; i < pathCubes.Count; i++)
        {
            Vector3Int pos = Vector3Int.FloorToInt(pathCubes[i].transform.position) * 2;
            floor.SetTile(pos, floorTiles[0]);
            pos.x++;
            floor.SetTile(pos, floorTiles[0]);
            pos.y++;
            floor.SetTile(pos, floorTiles[0]);
            pos.x--;
            floor.SetTile(pos, floorTiles[0]);
        }

        for (int i = 0; i < pathCubes.Count; i++)
        {
            Vector3Int pos = Vector3Int.FloorToInt(pathCubes[i].transform.position) * 2;

            pos.x--;
            int index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.y--;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.x++;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.x++;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.x++;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.y++;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.y++;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.y++;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.x--;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.x--;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.x--;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.y--;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }

            pos.y--;
            index = hallwayIndex(floor, wall, pos);
            if (index != -1) { wall.SetTile(pos, hallwayEdgeTiles[index]); }
            Destroy(pathCubes[i]);
        }
    }

    public int hallwayIndex(Tilemap floor, Tilemap wall, Vector3Int pos)
    {
        var test = floor.GetTile(pos);

        if (test)
        {
            return -1;
        }

        if (wall.HasTile(pos))
        {
            UnityEngine.Debug.Log("Test");
            return -1;
        }


        Vector3Int temp = pos;
        temp.x++;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 0;

        temp = pos;
        temp.y--;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 2;

        temp = pos;
        temp.x--;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 4;

        temp = pos;
        temp.y++;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 6;


        temp = pos;
        temp.y--;
        temp.x++;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 1;

        temp = pos;
        temp.x--;
        temp.y--;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 3;

        temp = pos;
        temp.x--;
        temp.y++;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 5;

        temp = pos;
        temp.y++;
        temp.x++;
        test = floor.GetTile(temp);
        if (test == floorTiles[0]) return 7;

        return -1;
    }





}


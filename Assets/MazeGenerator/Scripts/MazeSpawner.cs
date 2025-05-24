// using UnityEngine;
// using System.Collections;

// //<summary>
// //Game object, that creates maze and instantiates it in scene
// //</summary>
// public class MazeSpawner : MonoBehaviour {
// 	public enum MazeGenerationAlgorithm{
// 		PureRecursive,
// 		RecursiveTree,
// 		RandomTree,
// 		OldestTree,
// 		RecursiveDivision,
// 	}

// 	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
// 	public bool FullRandom = false;
// 	public int RandomSeed = 12345;
// 	public GameObject Floor = null;
// 	public GameObject Wall = null;
// 	public GameObject Pillar = null;
// 	public int Rows = 5;
// 	public int Columns = 5;
// 	public float CellWidth = 5;
// 	public float CellHeight = 5;
// 	public bool AddGaps = true;
// 	public GameObject GoalPrefab = null;

// 	private BasicMazeGenerator mMazeGenerator = null;

// 	void Start () {
// 		if (!FullRandom) {
// 			Random.seed = RandomSeed;
// 		}
// 		switch (Algorithm) {
// 		case MazeGenerationAlgorithm.PureRecursive:
// 			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.RecursiveTree:
// 			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.RandomTree:
// 			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.OldestTree:
// 			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
// 			break;
// 		case MazeGenerationAlgorithm.RecursiveDivision:
// 			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
// 			break;
// 		}
// 		mMazeGenerator.GenerateMaze ();
// 		Color wallColor = new Color(Random.value, Random.value, Random.value); // Generate one random color for all walls

// 		for (int row = 0; row < Rows; row++) {
// 			for(int column = 0; column < Columns; column++){
// 				float x = column*(CellWidth+(AddGaps?.2f:0));
// 				float z = row*(CellHeight+(AddGaps?.2f:0));
// 				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
// 				GameObject tmp;
// 				tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
// 				tmp.transform.parent = transform;
// 				if(cell.WallRight){
// 					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.WallFront){
// 					tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.WallLeft){
// 					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.WallBack){
// 					tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
// 					tmp.GetComponent<Renderer>().material.color = wallColor;

// 					tmp.transform.parent = transform;
// 				}
// 				if(cell.IsGoal && GoalPrefab != null){
// 					tmp = Instantiate(GoalPrefab,new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
// 					tmp.transform.parent = transform;
// 				}
// 			}
// 		}
// 		if(Pillar != null){
// 			for (int row = 0; row < Rows+1; row++) {
// 				for (int column = 0; column < Columns+1; column++) {
// 					float x = column*(CellWidth+(AddGaps?.2f:0));
// 					float z = row*(CellHeight+(AddGaps?.2f:0));
// 					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
// 					tmp.transform.parent = transform;
// 				}
// 			}
// 		}
// 	}
// // }
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// //<summary>
// //Game object, that creates maze and instantiates it in scene
// //</summary>
// public class MazeSpawner : MonoBehaviour {
//     public enum MazeGenerationAlgorithm {
//         PureRecursive,
//         RecursiveTree,
//         RandomTree,
//         OldestTree,
//         RecursiveDivision,
//     }

//     public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
//     public bool FullRandom = false;
//     public int RandomSeed = 12345;
//     public GameObject Floor = null;
//     public GameObject Wall = null;
//     public GameObject Pillar = null;
//     public int Rows = 5;
//     public int Columns = 5;
//     public float CellWidth = 5;
//     public float CellHeight = 5;
//     public bool AddGaps = true;
//     public GameObject GoalPrefab = null;
//      public GameObject[] PlayerPrefabs;

//     private BasicMazeGenerator mMazeGenerator = null;
//     private List<Vector3> availablePositions = new List<Vector3>();

//     void Start() {
//         if (!FullRandom) {
//             Random.InitState(RandomSeed); // Updated from deprecated Random.seed
//         }

//         // Initialize the maze generator
//         switch (Algorithm) {
//             case MazeGenerationAlgorithm.PureRecursive:
//                 mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveTree:
//                 mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RandomTree:
//                 mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.OldestTree:
//                 mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveDivision:
//                 mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
//                 break;
//         }

//         mMazeGenerator.GenerateMaze();

//         // 🔹 Set a fixed Dark Blue color for all walls (#00008B)
//         Color wallColor = Color.HSVToRGB(Random.value, 0.9f, 1f); // ✅ Random vibrant color


//         // Loop through the maze and instantiate objects
//         for (int row = 0; row < Rows; row++) {
//             for (int column = 0; column < Columns; column++) {
//                 float x = column * (CellWidth + (AddGaps ? .2f : 0));
//                 float z = row * (CellHeight + (AddGaps ? .2f : 0));
//                 MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
//                 GameObject tmp;

//                 // Floor
//                 tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity);
//                 tmp.transform.parent = transform;
//                 availablePositions.Add(new Vector3(x, 1, z));

//                 // Walls with applied color
//                 if (cell.WallRight) {
//                     tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallFront) {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.identity);
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallLeft) {
//                     tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallBack) {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }

//                 // Goal
//                 if (cell.IsGoal && GoalPrefab != null) {
//                     tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//         }

//         // Pillars
//         if (Pillar != null) {
//             for (int row = 0; row < Rows + 1; row++) {
//                 for (int column = 0; column < Columns + 1; column++) {
//                     float x = column * (CellWidth + (AddGaps ? .2f : 0));
//                     float z = row * (CellHeight + (AddGaps ? .2f : 0));
//                     GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//             SpawnPlayers();
//         }
//     }
//     void SpawnPlayers() {
//         if (PlayerPrefabs.Length < 6) {
//             Debug.LogError("You need at least 6 different Player Prefabs assigned!");
//             return;
//         }

//         int playerCount = Mathf.Min(6, availablePositions.Count); // Ensure we have enough positions
//         List<int> usedIndices = new List<int>();

//         for (int i = 0; i < playerCount; i++) {
//             int randomIndex;
//             do {
//                 randomIndex = Random.Range(0, availablePositions.Count);
//             } while (usedIndices.Contains(randomIndex)); // Ensure unique spawn positions

//             usedIndices.Add(randomIndex);
//             Vector3 spawnPosition = availablePositions[randomIndex];

//             int randomPrefabIndex = Random.Range(0, PlayerPrefabs.Length);
//             GameObject player = Instantiate(PlayerPrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
//             Debug.Log($"Player {i + 1} spawned at {spawnPosition}");
//         }
//     }



//     // ✅ Function to Apply the Same Color & Glow to All Walls
//     void ApplyWallColor(GameObject wall, Color color) {
//         if (wall == null) return; // Prevent errors if instantiation fails

//         Renderer renderer = wall.GetComponent<Renderer>();
//         if (renderer != null) {
//             renderer.material.color = color; // Apply Dark Blue color
//             renderer.material.SetColor("_EmissionColor", color * 2f); // Add glow effect
//             renderer.material.EnableKeyword("_EMISSION");
//         }

//         wall.transform.parent = transform; // Maintain hierarchy
//     }
// // }
// using UnityEngine;
// using UnityEngine.AI;
// using Unity.AI.Navigation; // ✅ Required for NavMeshSurface
// using System.Collections;
// using System.Collections.Generic;

// public class MazeSpawner : MonoBehaviour
// {
//     public enum MazeGenerationAlgorithm
//     {
//         PureRecursive,
//         RecursiveTree,
//         RandomTree,
//         OldestTree,
//         RecursiveDivision,
//     }

//     public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
//     public bool FullRandom = false;
//     public int RandomSeed = 12345;
//     public GameObject Floor = null;
//     public GameObject Wall = null;
//     public GameObject Pillar = null;
//     public int Rows = 5;
//     public int Columns = 5;
//     public float CellWidth = 5f;
//     public float CellHeight = 5f;
//     public bool AddGaps = true;
//     public GameObject GoalPrefab = null;
//     public GameObject[] PlayerPrefabs;

//     private BasicMazeGenerator mMazeGenerator = null;
//     private List<Vector3> availablePositions = new List<Vector3>();
//     private NavMeshSurface navMeshSurface;

//     void Start()
//     {
//         if (!FullRandom) Random.InitState(RandomSeed);

//         GenerateMaze();
//         AutoBakeNavMesh();
//     }

//     void GenerateMaze()
//     {
//         // Select the maze generation algorithm
//         switch (Algorithm)
//         {
//             case MazeGenerationAlgorithm.PureRecursive:
//                 mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveTree:
//                 mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RandomTree:
//                 mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.OldestTree:
//                 mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveDivision:
//                 mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
//                 break;
//         }

//         mMazeGenerator.GenerateMaze();
//         Color wallColor = Color.HSVToRGB(Random.value, 0.9f, 1f); // Random vibrant color

//         for (int row = 0; row < Rows; row++)
//         {
//             for (int column = 0; column < Columns; column++)
//             {
//                 float x = column * (CellWidth + (AddGaps ? 0.2f : 0));
//                 float z = row * (CellHeight + (AddGaps ? 0.2f : 0));
//                 MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
//                 GameObject tmp;

//                 // Instantiate floor
//                 tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity);
//                 tmp.transform.parent = transform;
//                 availablePositions.Add(new Vector3(x, 1, z));

//                 // Walls
//                 if (cell.WallRight)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x + (CellWidth / 2), 0, z), Quaternion.Euler(0, 90, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallFront)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2), Quaternion.identity);
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallLeft)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z), Quaternion.Euler(0, 270, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }
//                 if (cell.WallBack)
//                 {
//                     tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2), Quaternion.Euler(0, 180, 0));
//                     ApplyWallColor(tmp, wallColor);
//                 }

//                 // Goal
//                 if (cell.IsGoal && GoalPrefab != null)
//                 {
//                     tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//         }

//         // Pillars
//         if (Pillar != null)
//         {
//             for (int row = 0; row < Rows + 1; row++)
//             {
//                 for (int column = 0; column < Columns + 1; column++)
//                 {
//                     float x = column * (CellWidth + (AddGaps ? .2f : 0));
//                     float z = row * (CellHeight + (AddGaps ? .2f : 0));
//                     GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity);
//                     tmp.transform.parent = transform;
//                 }
//             }
//         }

//         SpawnPlayers();
//     }

//     void SpawnPlayers()
//     {
//         if (PlayerPrefabs.Length < 6)
//         {
//             Debug.LogError("You need at least 6 different Player Prefabs assigned!");
//             return;
//         }

//         int playerCount = Mathf.Min(6, availablePositions.Count);
//         List<int> usedIndices = new List<int>();

//         for (int i = 0; i < playerCount; i++)
//         {
//             int randomIndex;
//             do
//             {
//                 randomIndex = Random.Range(0, availablePositions.Count);
//             } while (usedIndices.Contains(randomIndex));

//             usedIndices.Add(randomIndex);
//             Vector3 spawnPosition = availablePositions[randomIndex];

//             int randomPrefabIndex = Random.Range(0, PlayerPrefabs.Length);
//             GameObject player = Instantiate(PlayerPrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
//             Debug.Log($"Player {i + 1} spawned at {spawnPosition}");
//         }
//     }

//     // ✅ Corrected: Function to Apply Color & Glow to All Walls
//     void ApplyWallColor(GameObject wall, Color color)
//     {
//         if (wall == null) return;

//         Renderer renderer = wall.GetComponent<Renderer>();
//         if (renderer != null)
//         {
//             renderer.material.color = color;
//             renderer.material.SetColor("_EmissionColor", color * 2f);
//             renderer.material.EnableKeyword("_EMISSION");
//         }

//         wall.transform.parent = transform;
//     }

//     // ✅ Auto-Baking NavMesh at Runtime
//     void AutoBakeNavMesh()
//     {
//         navMeshSurface = gameObject.GetComponent<NavMeshSurface>();
//         if (navMeshSurface == null)
//         {
//             navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
//         }

//         navMeshSurface.collectObjects = CollectObjects.Children;
//         navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
//         navMeshSurface.BuildNavMesh(); // Auto-bake NavMesh
        
//         // Auto delete the NavMesh after 60 seconds (adjust as needed)
//         StartCoroutine(DeleteNavMeshAfterTime(360f));
//     }

//     // ✅ Remove the NavMesh after a set time
//     IEnumerator DeleteNavMeshAfterTime(float time)
//     {
//         yield return new WaitForSeconds(time);
//         if (navMeshSurface != null)
//         {
//             navMeshSurface.RemoveData();
//             Destroy(navMeshSurface);
//         }
//     }
// }
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshSurface))]
public class ShapeMazeGenerator : MonoBehaviour
{
    public enum MazeShape { Rectangle, Circle, Rhombus, Hexagon, Spiral }
    
    [Header("Core Settings")]
    [Tooltip("When enabled, EVERY parameter randomizes for maximum variety")]
    public bool FullRandomness = true;
    [Range(0, 100)] public int RandomSeed = 0;
    
    [Header("Shape Selection")]
    public MazeShape shape = MazeShape.Rectangle;
    public bool RandomizeShapeEachTime = true;
    
    [Header("Dimensions")]
    public int minSize = 5;
    public int maxSize = 15;
    [Min(0.1f)] public float baseCellSize = 2f;
    [Range(0f, 1f)] public float gapProbability = 0.3f;
    
    [Header("Prefabs")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject pillarPrefab;
    public GameObject goalPrefab;
    public GameObject[] playerPrefabs;

    private NavMeshSurface navSurface;
    private MazeShape currentShape;
    private int currentSize;
    private float currentCellSize;
    private List<Vector3> walkablePositions = new List<Vector3>();

    void Start() => GenerateNewMaze();

    public void GenerateNewMaze()
    {
        ClearPreviousMaze();
        InitializeRandomization();
        GenerateCurrentShape();
        SpawnPlayers();
        BakeNavigation();
    }

    void InitializeRandomization()
    {
        Random.InitState(FullRandomness ? System.DateTime.Now.Millisecond : RandomSeed);
        
        currentSize = Random.Range(minSize, maxSize + 1);
        currentCellSize = baseCellSize * Random.Range(0.9f, 1.1f);
        currentShape = RandomizeShapeEachTime ? 
            (MazeShape)Random.Range(0, System.Enum.GetValues(typeof(MazeShape)).Length) : shape;
    }

    void GenerateCurrentShape()
    {
        switch (currentShape)
        {
            case MazeShape.Rectangle: GenerateRectangle(); break;
            case MazeShape.Circle: GenerateCircle(); break;
            case MazeShape.Rhombus: GenerateRhombus(); break;
            case MazeShape.Hexagon: GenerateHexagon(); break;
            case MazeShape.Spiral: GenerateSpiral(); break;
        }
    }

    #region Shape Generation Methods
    
    void GenerateRectangle()
    {
        int width = currentSize;
        int height = Mathf.RoundToInt(currentSize * Random.Range(0.7f, 1.3f));
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(
                    x * currentCellSize - (width * currentCellSize / 2),
                    0,
                    y * currentCellSize - (height * currentCellSize / 2));
                
                CreateCell(pos, Quaternion.identity);
                
                // Walls
                if (x == 0 || Random.value < 0.3f) // West walls
                    CreateWall(pos + Vector3.left * currentCellSize/2, Quaternion.Euler(0, 90, 0));
                if (y == 0 || Random.value < 0.3f) // South walls
                    CreateWall(pos + Vector3.back * currentCellSize/2, Quaternion.identity);
            }
        }
    }

    void GenerateCircle()
    {
        int rings = currentSize;
        int sectors = Mathf.RoundToInt(currentSize * 1.5f);
        float radiusStep = currentCellSize;
        
        for (int r = 1; r <= rings; r++)
        {
            float radius = r * radiusStep;
            float sectorAngle = 360f / sectors;
            
            for (int s = 0; s < sectors; s++)
            {
                float angle = s * sectorAngle;
                Vector3 pos = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    0,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
                
                CreateCell(pos, Quaternion.Euler(0, -angle, 0));
                
                // Radial walls (spokes)
                if (Random.value < 0.25f)
                    CreateWall(pos, Quaternion.Euler(0, -angle, 0));
                
                // Circular walls (rings)
                if (r > 1 && Random.value < 0.3f)
                    CreateWall(pos, Quaternion.Euler(0, -angle + 90, 0));
            }
        }
    }

    void GenerateRhombus()
    {
        int size = currentSize;
        float halfSize = size * currentCellSize / 2f;
        
        for (int row = 0; row < size; row++)
        {
            int cols = size - Mathf.Abs(row - size/2);
            
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new Vector3(
                    (col - cols/2f) * currentCellSize * 1.2f,
                    0,
                    (row - size/2f) * currentCellSize);
                
                CreateCell(pos, Quaternion.identity);
                
                // Diagonal walls
                if (Random.value < 0.35f)
                    CreateWall(pos, Quaternion.Euler(0, 45, 0));
            }
        }
    }

    void GenerateHexagon()
    {
        int radius = currentSize;
        float hexWidth = Mathf.Sqrt(3) * currentCellSize;
        
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            
            for (int r = r1; r <= r2; r++)
            {
                Vector3 pos = new Vector3(
                    hexWidth * (q + r/2f),
                    0,
                    currentCellSize * 1.5f * r);
                
                CreateCell(pos, Quaternion.identity);
                
                // Hexagonal walls
                if (Random.value < 0.4f)
                    CreateWall(pos, Quaternion.Euler(0, 60 * Random.Range(0, 6), 0));
            }
        }
    }

    void GenerateSpiral()
    {
        int arms = Random.Range(3, 6);
        int pointsPerArm = currentSize;
        float rotationFactor = 5f;
        
        for (int arm = 0; arm < arms; arm++)
        {
            for (int point = 1; point <= pointsPerArm; point++)
            {
                float distance = point * currentCellSize;
                float angle = (360f/arms) * arm + (distance * rotationFactor);
                
                Vector3 pos = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
                    0,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * distance);
                
                CreateCell(pos, Quaternion.Euler(0, -angle, 0));
                
                // Spiral walls
                if (Random.value < 0.5f)
                    CreateWall(pos, Quaternion.Euler(0, -angle + 90, 0));
            }
        }
    }

    void CreateCell(Vector3 position, Quaternion rotation)
    {
        Instantiate(floorPrefab, position, rotation, transform);
        walkablePositions.Add(position + Vector3.up * 0.1f);
        
        // Random gaps
        if (Random.value < gapProbability)
        {
            Instantiate(pillarPrefab, position, Quaternion.identity, transform);
        }
    }

    void CreateWall(Vector3 position, Quaternion rotation)
    {
        GameObject wall = Instantiate(wallPrefab, position, rotation, transform);
        wall.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.5f, 1f);
    }

    #endregion

    void SpawnPlayers()
    {
        if (playerPrefabs == null || playerPrefabs.Length == 0) return;
        
        ShufflePositions();
        
        for (int i = 0; i < Mathf.Min(playerPrefabs.Length, walkablePositions.Count); i++)
        {
            Instantiate(playerPrefabs[i], walkablePositions[i], Quaternion.identity);
        }
    }

    void ShufflePositions()
    {
        for (int i = 0; i < walkablePositions.Count; i++)
        {
            int randomIndex = Random.Range(i, walkablePositions.Count);
            (walkablePositions[randomIndex], walkablePositions[i]) = 
                (walkablePositions[i], walkablePositions[randomIndex]);
        }
    }

    void BakeNavigation()
    {
        if (navSurface == null) navSurface = GetComponent<NavMeshSurface>();
        navSurface.BuildNavMesh();
    }

    void ClearPreviousMaze()
    {
        walkablePositions.Clear();
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
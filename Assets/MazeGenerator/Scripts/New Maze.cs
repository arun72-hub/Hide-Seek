// using UnityEngine;
// using UnityEngine.AI;
// using Unity.AI.Navigation;
// using System.Collections;
// using System.Collections.Generic;

// public enum MazeShape {
//     Rectangle,
//     Circle,
//     Triangle,
//     Hexagon,
//     Rhombus,
//     Trapezium,
//     RandomShape
// }

// public enum MazeGenerationAlgorithm {
//     PureRecursive,
//     RecursiveTree,
//     RandomTree,
//     OldestTree,
//     RecursiveDivision,
// }

// public class AdvancedMazeGenerator : MonoBehaviour {
//     [Header("Maze Configuration")]
//     public MazeShape shape = MazeShape.Rectangle;
//     public MazeGenerationAlgorithm algorithm = MazeGenerationAlgorithm.RecursiveDivision;
//     public bool fullRandom = true;
//     [Range(0f, 1f)] public float randomness = 1f;
//     public int randomSeed = 12345;
    
//     [Header("Dimensions")]
//     public int baseRows = 5;
//     public int baseColumns = 5;
//     public float cellWidth = 5f;
//     public float cellHeight = 5f;
//     public bool addGaps = true;
    
//     [Header("Visual Elements")]
//     public GameObject floorPrefab;
//     public GameObject wallPrefab;
//     public GameObject pillarPrefab;
//     public GameObject goalPrefab;
//     public GameObject[] playerPrefabs;
    
//     [Header("Shape Parameters")]
//     public float circleRadius = 10f;
//     public float triangleRatio = 1.5f;
//     public float hexagonSize = 1f;
//     public float rhombusAngle = 60f;
//     public float trapeziumTopRatio = 0.7f;
    
//     private BasicMazeGenerator mazeGenerator;
//     private List<Vector3> availablePositions = new List<Vector3>();
//     private NavMeshSurface navMeshSurface;
//     private MazeShape previousShape;
//     private int previousSeed;
    
//     void Start() {
//         GenerateMaze();
//     }

//     public void GenerateMaze() {
//         // Clear previous maze
//         foreach (Transform child in transform) {
//             Destroy(child.gameObject);
//         }
//         availablePositions.Clear();

//         // Ensure maximum randomness
//         if (fullRandom || randomness >= 1f) {
//             randomSeed = Random.Range(int.MinValue, int.MaxValue);
//         }
//         Random.InitState(randomSeed);

//         // Random shape selection if desired
//         if (randomness > 0.5f && shape == MazeShape.RandomShape) {
//             shape = (MazeShape)Random.Range(0, (int)MazeShape.RandomShape);
//         }

//         // Calculate dimensions based on shape
//         int rows, columns;
//         CalculateDimensions(out rows, out columns);

//         // Create the appropriate maze generator
//         CreateMazeGenerator(rows, columns);
//         mazeGenerator.GenerateMaze();

//         // Generate the maze geometry based on shape
//         switch (shape) {
//             case MazeShape.Circle:
//                 GenerateCircularMaze(rows, columns);
//                 break;
//             case MazeShape.Triangle:
//                 GenerateTriangularMaze(rows, columns);
//                 break;
//             case MazeShape.Hexagon:
//                 GenerateHexagonalMaze(rows, columns);
//                 break;
//             case MazeShape.Rhombus:
//                 GenerateRhombusMaze(rows, columns);
//                 break;
//             case MazeShape.Trapezium:
//                 GenerateTrapeziumMaze(rows, columns);
//                 break;
//             default:
//                 GenerateRectangularMaze(rows, columns);
//                 break;
//         }

//         SpawnPlayers();
//         AutoBakeNavMesh();
//     }

//     void CalculateDimensions(out int rows, out int columns) {
//         // Apply randomness to dimensions
//         float sizeVariation = 1f + (Random.value - 0.5f) * randomness;
        
//         rows = Mathf.Max(3, Mathf.RoundToInt(baseRows * sizeVariation));
//         columns = Mathf.Max(3, Mathf.RoundToInt(baseColumns * sizeVariation));
        
//         // Adjust for specific shapes
//         if (shape == MazeShape.Triangle) {
//             columns = Mathf.RoundToInt(rows * triangleRatio);
//         }
//         else if (shape == MazeShape.Hexagon) {
//             rows = columns; // Hexagons are typically regular
//         }
//     }

//     void CreateMazeGenerator(int rows, int columns) {
//         // Random algorithm selection if desired
//         if (randomness > 0.7f) {
//             algorithm = (MazeGenerationAlgorithm)Random.Range(0, System.Enum.GetValues(typeof(MazeGenerationAlgorithm)).Length);
//         }

//         switch (algorithm) {
//             case MazeGenerationAlgorithm.PureRecursive:
//                 mazeGenerator = new RecursiveMazeGenerator(rows, columns);
//                 break;
//             case MazeGenerationAlgorithm.RecursiveTree:
//                 mazeGenerator = new RecursiveTreeMazeGenerator(rows, columns);
//                 break;
//             case MazeGenerationAlgorithm.RandomTree:
//                 mazeGenerator = new RandomTreeMazeGenerator(rows, columns);
//                 break;
//             case MazeGenerationAlgorithm.OldestTree:
//                 mazeGenerator = new OldestTreeMazeGenerator(rows, columns);
//                 break;
//             default:
//                 mazeGenerator = new DivisionMazeGenerator(rows, columns);
//                 break;
//         }
//     }

//     void GenerateRectangularMaze(int rows, int columns) {
//         Color wallColor = GenerateRandomColor();
        
//         for (int row = 0; row < rows; row++) {
//             for (int column = 0; column < columns; column++) {
//                 float x = column * (cellWidth + (addGaps ? 0.2f : 0));
//                 float z = row * (cellHeight + (addGaps ? 0.2f : 0));
                
//                 MazeCell cell = mazeGenerator.GetMazeCell(row, column);
//                 CreateCell(x, z, cell, wallColor);
//             }
//         }
        
//         CreatePillars(rows, columns);
//     }

//     void GenerateCircularMaze(int rows, int columns) {
//         Color wallColor = GenerateRandomColor();
//         float angleStep = 360f / columns;
//         float radiusStep = circleRadius / rows;
        
//         for (int row = 0; row < rows; row++) {
//             for (int column = 0; column < columns; column++) {
//                 float radius = (row + 0.5f) * radiusStep;
//                 float angle = column * angleStep * Mathf.Deg2Rad;
                
//                 float x = radius * Mathf.Cos(angle);
//                 float z = radius * Mathf.Sin(angle);
                
//                 MazeCell cell = mazeGenerator.GetMazeCell(row, column);
//                 CreateRadialCell(x, z, angle, cell, wallColor);
//             }
//         }
//     }

//     void GenerateTriangularMaze(int rows, int columns) {
//         Color wallColor = GenerateRandomColor();
//         float height = cellHeight * rows;
//         float baseWidth = cellWidth * columns;
        
//         for (int row = 0; row < rows; row++) {
//             float rowWidth = baseWidth * (1f - (float)row / rows);
//             int colsInRow = Mathf.RoundToInt(columns * (1f - (float)row / rows));
            
//             for (int column = 0; column < colsInRow; column++) {
//                 float x = column * (rowWidth / colsInRow) - rowWidth / 2f;
//                 float z = row * cellHeight;
                
//                 MazeCell cell = mazeGenerator.GetMazeCell(row, column % columns);
//                 CreateCell(x, z, cell, wallColor);
//             }
//         }
//     }

//     void GenerateHexagonalMaze(int rows, int columns) {
//         Color wallColor = GenerateRandomColor();
//         float horizontalSpacing = cellWidth * 0.75f;
//         float verticalSpacing = cellHeight * 0.866f; // sqrt(3)/2
        
//         for (int row = 0; row < rows; row++) {
//             for (int column = 0; column < columns; column++) {
//                 float xOffset = (row % 2) * horizontalSpacing * 0.5f;
//                 float x = column * horizontalSpacing + xOffset;
//                 float z = row * verticalSpacing;
                
//                 MazeCell cell = mazeGenerator.GetMazeCell(row, column);
//                 CreateHexagonalCell(x, z, cell, wallColor);
//             }
//         }
//     }

//     void GenerateRhombusMaze(int rows, int columns) {
//         Color wallColor = GenerateRandomColor();
//         float angleRad = rhombusAngle * Mathf.Deg2Rad;
//         float horizontalScale = Mathf.Sin(angleRad);
        
//         for (int row = 0; row < rows; row++) {
//             for (int column = 0; column < columns; column++) {
//                 float x = column * cellWidth * horizontalScale + row * cellWidth * horizontalScale * 0.5f;
//                 float z = row * cellHeight;
                
//                 MazeCell cell = mazeGenerator.GetMazeCell(row, column);
//                 CreateCell(x, z, cell, wallColor, rhombusAngle);
//             }
//         }
//     }

//     void GenerateTrapeziumMaze(int rows, int columns) {
//         Color wallColor = GenerateRandomColor();
//         float topWidth = columns * cellWidth * trapeziumTopRatio;
//         float widthStep = (columns * cellWidth - topWidth) / rows;
        
//         for (int row = 0; row < rows; row++) {
//             float currentWidth = topWidth + row * widthStep;
//             int colsInRow = Mathf.RoundToInt(columns * (trapeziumTopRatio + (1f - trapeziumTopRatio) * row / rows));
            
//             for (int column = 0; column < colsInRow; column++) {
//                 float x = column * (currentWidth / colsInRow) - currentWidth / 2f;
//                 float z = row * cellHeight;
                
//                 MazeCell cell = mazeGenerator.GetMazeCell(row, column % columns);
//                 CreateCell(x, z, cell, wallColor);
//             }
//         }
//     }

//     void CreateCell(float x, float z, MazeCell cell, Color wallColor, float rotation = 0f) {
//         // Floor
//         GameObject floor = Instantiate(floorPrefab, new Vector3(x, 0, z), Quaternion.Euler(0, rotation, 0));
//         floor.transform.parent = transform;
//         availablePositions.Add(new Vector3(x, 1, z));
        
//         // Walls with random height variation
//         float wallHeight = 2f * (1f + (Random.value - 0.5f) * randomness);
        
//         if (cell.WallRight) {
//             CreateWall(x + cellWidth/2, z, 90f + rotation, wallColor, wallHeight);
//         }
//         if (cell.WallFront) {
//             CreateWall(x, z + cellHeight/2, rotation, wallColor, wallHeight);
//         }
//         if (cell.WallLeft) {
//             CreateWall(x - cellWidth/2, z, 270f + rotation, wallColor, wallHeight);
//         }
//         if (cell.WallBack) {
//             CreateWall(x, z - cellHeight/2, 180f + rotation, wallColor, wallHeight);
//         }
        
//         // Goal
//         if (cell.IsGoal && goalPrefab != null) {
//             Instantiate(goalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, rotation, 0)).transform.parent = transform;
//         }
//     }

//     void CreateRadialCell(float x, float z, float angle, MazeCell cell, Color wallColor) {
//         // Circular floor
//         GameObject floor = Instantiate(floorPrefab, new Vector3(x, 0, z), Quaternion.identity);
//         floor.transform.localScale = new Vector3(1.2f, 1f, 1.2f); // Larger for circular maze
//         floor.transform.parent = transform;
//         availablePositions.Add(new Vector3(x, 1, z));
        
//         // Radial walls
//         if (cell.WallRight) {
//             CreateWall(x + Mathf.Cos(angle) * 0.6f, z + Mathf.Sin(angle) * 0.6f, angle * Mathf.Rad2Deg, wallColor);
//         }
//         if (cell.WallLeft) {
//             CreateWall(x - Mathf.Cos(angle) * 0.6f, z - Mathf.Sin(angle) * 0.6f, (angle + Mathf.PI) * Mathf.Rad2Deg, wallColor);
//         }
//         if (cell.WallFront) {
//             CreateWall(x + Mathf.Cos(angle + Mathf.PI/2) * 0.6f, z + Mathf.Sin(angle + Mathf.PI/2) * 0.6f, 
//                       (angle + Mathf.PI/2) * Mathf.Rad2Deg, wallColor);
//         }
//         if (cell.WallBack) {
//             CreateWall(x + Mathf.Cos(angle - Mathf.PI/2) * 0.6f, z + Mathf.Sin(angle - Mathf.PI/2) * 0.6f, 
//                       (angle - Mathf.PI/2) * Mathf.Rad2Deg, wallColor);
//         }
        
//         // Goal
//         if (cell.IsGoal && goalPrefab != null) {
//             Instantiate(goalPrefab, new Vector3(x, 1, z), Quaternion.identity).transform.parent = transform;
//         }
//     }

//     void CreateHexagonalCell(float x, float z, MazeCell cell, Color wallColor) {
//         // Hexagonal floor would need a specific prefab
//         GameObject floor = Instantiate(floorPrefab, new Vector3(x, 0, z), Quaternion.identity);
//         floor.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
//         floor.transform.parent = transform;
//         availablePositions.Add(new Vector3(x, 1, z));
        
//         // Hex walls at 60 degree increments
//         for (int i = 0; i < 6; i++) {
//             float angle = 60f * i;
//             bool shouldCreateWall = false;
            
//             // Map hex walls to standard cell walls
//             if (i == 0 && cell.WallRight) shouldCreateWall = true;
//             if (i == 1 && cell.WallFront) shouldCreateWall = true;
//             if (i == 2 && Random.value < 0.5f * randomness) shouldCreateWall = true; // Extra randomness
//             if (i == 3 && cell.WallLeft) shouldCreateWall = true;
//             if (i == 4 && cell.WallBack) shouldCreateWall = true;
//             if (i == 5 && Random.value < 0.5f * randomness) shouldCreateWall = true; // Extra randomness
            
//             if (shouldCreateWall) {
//                 CreateWall(x + Mathf.Cos(angle * Mathf.Deg2Rad) * 0.7f, 
//                           z + Mathf.Sin(angle * Mathf.Deg2Rad) * 0.7f, 
//                           angle, wallColor);
//             }
//         }
        
//         // Goal
//         if (cell.IsGoal && goalPrefab != null) {
//             Instantiate(goalPrefab, new Vector3(x, 1, z), Quaternion.identity).transform.parent = transform;
//         }
//     }

//     void CreateWall(float x, float z, float angle, Color color, float height = 2f) {
//         GameObject wall = Instantiate(wallPrefab, new Vector3(x, height/2, z), Quaternion.Euler(0, angle, 0));
//         wall.transform.localScale = new Vector3(wall.transform.localScale.x, height, wall.transform.localScale.z);
//         wall.transform.parent = transform;
        
//         Renderer renderer = wall.GetComponent<Renderer>();
//         if (renderer != null) {
//             renderer.material.color = color;
//             renderer.material.SetColor("_EmissionColor", color * 2f);
//             renderer.material.EnableKeyword("_EMISSION");
//         }
//     }

//     void CreatePillars(int rows, int columns) {
//         if (pillarPrefab == null) return;
        
//         // Adjust pillar placement based on shape
//         switch (shape) {
//             case MazeShape.Circle:
//                 // Pillars around circumference
//                 int pillarCount = Mathf.Max(8, columns);
//                 for (int i = 0; i < pillarCount; i++) {
//                     float angle = 360f * i / pillarCount * Mathf.Deg2Rad;
//                     float x = circleRadius * Mathf.Cos(angle);
//                     float z = circleRadius * Mathf.Sin(angle);
//                     Instantiate(pillarPrefab, new Vector3(x, 0, z), Quaternion.identity).transform.parent = transform;
//                 }
//                 break;
                
//             case MazeShape.Hexagon:
//                 // Pillars at hex corners
//                 for (int row = -1; row <= rows; row++) {
//                     for (int column = -1; column <= columns; column++) {
//                         float xOffset = (row % 2) * cellWidth * 0.375f;
//                         float x = column * cellWidth * 0.75f + xOffset;
//                         float z = row * cellHeight * 0.866f;
//                         Instantiate(pillarPrefab, new Vector3(x, 0, z), Quaternion.identity).transform.parent = transform;
//                     }
//                 }
//                 break;
                
//             default:
//                 // Standard rectangular pillar placement
//                 for (int row = 0; row <= rows; row++) {
//                     for (int column = 0; column <= columns; column++) {
//                         float x = column * (cellWidth + (addGaps ? 0.2f : 0)) - cellWidth/2;
//                         float z = row * (cellHeight + (addGaps ? 0.2f : 0)) - cellHeight/2;
//                         Instantiate(pillarPrefab, new Vector3(x, 0, z), Quaternion.identity).transform.parent = transform;
//                     }
//                 }
//                 break;
//         }
//     }

//     void SpawnPlayers() {
//         if (playerPrefabs == null || playerPrefabs.Length == 0) {
//             Debug.LogError("No player prefabs assigned!");
//             return;
//         }
        
//         // Apply randomness to player count
//         int playerCount = Mathf.Min(
//             playerPrefabs.Length,
//             Mathf.Max(1, Mathf.RoundToInt(availablePositions.Count * (0.2f + 0.3f * Random.value)))
//         );
        
//         // Shuffle positions
//         for (int i = 0; i < availablePositions.Count; i++) {
//             int randomIndex = Random.Range(i, availablePositions.Count);
//             Vector3 temp = availablePositions[i];
//             availablePositions[i] = availablePositions[randomIndex];
//             availablePositions[randomIndex] = temp;
//         }
        
//         // Spawn players
//         for (int i = 0; i < playerCount; i++) {
//             int prefabIndex = Random.Range(0, playerPrefabs.Length);
//             Instantiate(playerPrefabs[prefabIndex], availablePositions[i], Quaternion.identity);
//         }
//     }

//     void AutoBakeNavMesh() {
//         navMeshSurface = GetComponent<NavMeshSurface>();
//         if (navMeshSurface == null) {
//             navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
//         }
        
//         navMeshSurface.collectObjects = CollectObjects.Children;
//         navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
        
//         // Adjust for different shapes
//         if (shape == MazeShape.Circle) {
//             navMeshSurface.size = new Vector3(circleRadius * 2.2f, 10f, circleRadius * 2.2f);
//         } else {
//             float width = baseColumns * cellWidth * 1.2f;
//             float length = baseRows * cellHeight * 1.2f;
//             navMeshSurface.size = new Vector3(width, 10f, length);
//         }
        
//         navMeshSurface.BuildNavMesh();
//     }

//     Color GenerateRandomColor() {
//         // Ensure maximum color variation with high randomness
//         if (randomness > 0.8f) {
//             return Color.HSVToRGB(Random.value, 0.7f + Random.value * 0.3f, 0.8f + Random.value * 0.2f);
//         }
//         return Color.HSVToRGB(Random.Range(0f, 0.2f), 0.7f, 0.9f); // More consistent colors with low randomness
//     }

//     // Editor button for testing
//     [ContextMenu("Generate New Maze")]
//     void GenerateNewMaze() {
//         GenerateMaze();
//     }
// }
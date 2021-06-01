using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance = null;

    [Header("Variables")]
    [Range(5, 15)] public int columns = 8;
    [Range(5, 15)] public int rows = 8;
    [Range(10, 100)] public int maxMines = 10;

    [Header("Game Objects")]
    public GameObject tile;
    public GameObject mine;
    public Sprite tileBack;
    public Sprite tileFlag;
    public Sprite tileMine;
    [InspectorName("Revealed Tiles Sprites")] public Sprite[] tileRevealed;

    [Header("Game Logic")]
    private float tileSize;
    private Transform gridHolder;
    private Vector3[,] gridPositions;
    public GameObject[,] grid;

    private void Awake() {
        // Singleton
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SetupGame();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            for (int col = 0; col < columns; col++) {
                for (int row = 0; row < rows; row++) {
                    grid[col, row].GetComponent<Tile>().RevealTile();
                }
            }
        }
    }

    public void SetupGame() {
        InitialiseGrid();
        BoardSetup();
        SetMines();
    }


    private void InitialiseGrid() {
        grid = new GameObject[columns, rows];
        gridPositions = new Vector3[columns, rows];
        // Getting the size of the tile for instantiation later.
        tileSize = tile.GetComponent<SpriteRenderer>().bounds.size.x;

        for (int col = 0; col < columns; col++) {
            for (int row = 0; row < rows; row++) {
                gridPositions[col, row] = new Vector3((col - columns / 2) * tileSize, (row - rows / 2) * tileSize, 0f);
            }
        }
    }

    private void BoardSetup() {
        // Used for parenting tiles
        if (gridHolder) {
            DestroyImmediate(gridHolder.gameObject);
        }
        gridHolder = new GameObject("Grid").transform;

        for (int col = 0; col < columns; col++) {
            for (int row = 0; row < rows; row++) {
                GameObject toInstantiate = Instantiate(tile, gridPositions[col, row], Quaternion.identity);
                
                // Set tile to xy position
                grid[col, row] = toInstantiate;

                // Used for order in hierarchy
                toInstantiate.transform.SetParent(gridHolder);
            }
        }
    }

    public void IncrementTiles(List<Vector2> coordinates) {
        print(coordinates.Count);
        foreach (Vector2 pos in coordinates) {
            if (grid[(int)pos.x, (int)pos.y].GetComponent<Tile>().isMine == false) {
                grid[(int)pos.x, (int)pos.y].GetComponent<Tile>().tileNumber++;
            }
        }
    }

    private void SetMines() {
        if (rows * columns <= maxMines) {
            print("too many mines");
            return;
        }

        for (int i = 0; i < maxMines; i++) {
            int maxLoops = 10;
            int recursiveLoopCounter = 0;
            while (true) {
                if (recursiveLoopCounter > maxLoops) {
                    print("FUCK");
                    break;
                }
                int row = Random.Range(0, columns);
                int col = Random.Range(0, rows);

                if (grid[col, row].GetComponent<Tile>().isMine == false) {
                    grid[col, row].GetComponent<Tile>().isMine = true;
                    grid[col, row].GetComponent<Tile>().IncrementAdjacentTiles(col, row, rows, columns);
                    break;
                }
                recursiveLoopCounter++;
            }
        }
    }
}

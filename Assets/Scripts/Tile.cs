using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer sr;

    [Header("Variables")]
    public bool isFlagged = false;
    public bool isMine = false;
    public bool isClicked { get; private set; } = false;
    public int tileNumber = 0;


    // Start is called before the first frame update
    void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver() {
        if (!isClicked) {
            if (Input.GetMouseButtonDown(0) && !isFlagged) {
                RevealTile();
            }
            else if (Input.GetMouseButtonDown(1)) {
                FlagTile();
            }
        }
    }

    private void FlagTile() {
        if (isFlagged) { sr.sprite = BoardManager.instance.tileBack; }
        else { sr.sprite = BoardManager.instance.tileFlag; }

        isFlagged = !isFlagged;
    }

    public void RevealTile() {
        isClicked = true;

        if (isMine) {
            sr.sprite = BoardManager.instance.tileMine;
        }
        else {
            sr.sprite = BoardManager.instance.tileRevealed[tileNumber];
        }
    }

    public void IncrementAdjacentTiles(int col, int row, int rows, int columns) {
        if (isMine) {
            for (int x = -1; x < 1 + 1; x++) {
                for (int y = -1; y < 1 + 1; y++) {
                    if (col + x < 0 || col + x > columns - 1 || row + y < 0 || row + y > rows -1) {
                        continue;
                    }
                    BoardManager.instance.grid[col + x, row + y].GetComponent<Tile>().tileNumber++;
                }
            }
        }
    }

}

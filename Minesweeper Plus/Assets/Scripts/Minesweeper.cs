using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minesweeper : MonoBehaviour
{
    static public int sMineCount = 32;

    static public int width = 16;
    static public int height = 16;
    public int mineCount = sMineCount;
    static public int objective = 0;

    private Board board;
    private Cell[,] state;
    static public bool gameover;
    static public bool start;
    static public bool end;
    static public int counter;
    static public int finalScore;
    
    void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    void Start()
    {
        NewLevel();
    }

    public void NewLevel()
    {
        state = new Cell[width, height];
        Level.level += 1;
        mineCount += 3;
        mineCount = Mathf.Clamp(mineCount, 0, (int) ((width * height) * .19));
        if(Difficulty.difficulty == "Easy") {
            objective = Random.Range((int) ((width * height) * 0.1), (int) ((width * height) * 0.15));
        } else{
            objective = Random.Range((int) ((width * height) * 0.07), (int) ((width * height) * 0.1));
        }
        counter = 0;
        gameover = false;
        start = false;
        end = false;

        GenerateCells();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f); // center camera
        board.Draw(state);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            gameover = true;
            SceneManager.LoadScene("Menu");
        }
        if(!start) {
            if(Input.GetMouseButtonDown(0)) {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
                Cell firstCell = GetCell(cellPos.x, cellPos.y);
                if(firstCell.type != Cell.Type.Invalid) {
                    firstCell.type = Cell.Type.Initial;
                    state[cellPos.x, cellPos.y] = firstCell;
                    for(int adjX = -1; adjX <= 1; adjX++) {
                        for(int adjY = -1; adjY <= 1; adjY++) {
                            if(adjX == 0 && adjY == 0) {
                                continue;
                            }

                            int x = cellPos.x + adjX;
                            int y = cellPos.y + adjY;

                            Cell cell = GetCell(x,y);
                            if(cell.type != Cell.Type.Invalid) {
                                state[x,y].type = Cell.Type.Initial;
                            }
                        }
                    }
                    GenerateMines();
                    for(int adjX = -1; adjX <= 1; adjX++) {
                        for(int adjY = -1; adjY <= 1; adjY++) {
                            int x = cellPos.x + adjX;
                            int y = cellPos.y + adjY;

                            Cell cell = GetCell(x,y);
                            if(cell.type != Cell.Type.Invalid) {
                                state[x,y].type = Cell.Type.Empty;
                            }
                        }
                    }
                    GenerateNumbers();
                    board.Draw(state);
                    Reveal();
                    start = true;
                }
            }
        } else if(!gameover) {
            if(Input.GetMouseButtonDown(1)) {
                Flag();
            } else if(Input.GetMouseButtonDown(0)) {
                Reveal();
            }
        } else if(end) {
            GameOver();
        }
    }

    void GenerateCells()
    {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x,y,0);
                cell.type = Cell.Type.Empty;
                state[x,y] = cell;
            }
        }
    }

    void GenerateMines()
    {
        for(int i = 0; i < mineCount; i++) {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while(state[x,y].type == Cell.Type.Mine || state[x,y].type == Cell.Type.Initial) {
                x++;
                
                if(x >= width) {
                    x = 0;
                    y++; // increment row

                    if(y >= height) {
                        y = 0;
                    }
                }
            }

            state[x,y].type = Cell.Type.Mine;
        }
    }

    void GenerateNumbers()
    {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                Cell cell = state[x,y];

                if(cell.type == Cell.Type.Mine) {
                    continue;
                }

                cell.number = CountMines(x,y);
                
                if(cell.number > 0) {
                    cell.type = Cell.Type.Number;
                }

                state[x,y] = cell;
            }
        }
    }

    int CountMines(int cellX, int cellY) {
        int count = 0;

        for(int adjX = -1; adjX <= 1; adjX++) {
            for(int adjY = -1; adjY <= 1; adjY++) {
                if(adjX == 0 && adjY == 0) {
                    continue;
                }

                int x = cellX + adjX;
                int y = cellY + adjY;

                if(GetCell(x,y).type == Cell.Type.Mine) {
                    count ++;
                }

            }
        }
        return count;
    }

    void Flag()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        if(cell.type == Cell.Type.Invalid || cell.revealed) {
            return;
        }

        cell.flagged = !cell.flagged;
        state[cellPos.x, cellPos.y] = cell;
        board.Draw(state);
    }

    void Reveal()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        if(cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged) {
            return;
        }

        switch(cell.type) {
            case Cell.Type.Mine:
                Explode(cell);
                break;
            case Cell.Type.Empty:
                Timer.timeCountdown += 0.25f;
                ScoreCounter.score += 10;
                counter += 1;
                RevealEmpty(cell);
                CheckWin();
                break;
            default:
                Timer.timeCountdown += 0.25f;
                ScoreCounter.score += 10;
                counter += 1;
                cell.revealed = true;
                state[cellPos.x, cellPos.y] = cell;
                CheckWin();
                break;
        }

        board.Draw(state);
    }

    void RevealEmpty(Cell cell)
    {
        if(cell.revealed) {
            return;
        }
        if(cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) {
            return;
        }

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if(cell.type == Cell.Type.Empty) {
            for(int adjX = -1; adjX <= 1; adjX++) {
                for(int adjY = -1; adjY <= 1; adjY++) {
                    if(adjX == 0 && adjY == 0) {
                        continue;
                    }

                    int x = cell.position.x + adjX;
                    int y = cell.position.y + adjY;

                    RevealEmpty(GetCell(x,y));
                }
            }
        }
    }

    void Explode(Cell cell)
    {
        cell.revealed = true;
        cell.exploded = true;
        state[cell.position.x, cell.position.y] = cell;

        Timer.timeCountdown -= 20;
        ScoreCounter.score -= 10;
    }

    void CheckWin()
    {
        if(counter != objective) {
            return;
        }

        gameover = true;

        for(int x = 0; x < width; x++) { // flag remaining mines
            for(int y = 0; y < height; y++) {
                Cell cell = state[x,y];

                if(cell.type == Cell.Type.Mine) {
                    cell.flagged = true;
                    state[x,y] = cell;
                }
            }
        }
        
        Timer.timeCountdown += 5;
        Timer.uiText.text = Timer.GetTime();
        ScoreCounter.uiText.text = "Score: " + ScoreCounter.score.ToString("#,0");
        Invoke("NewLevel", 1f);
    }

    void GameOver()
    {
        end = false;
        for(int x = 0; x < width; x++) { // reveal remaining mines
            for(int y = 0; y < height; y++) {
                Cell cell = state[x,y];

                if(cell.type == Cell.Type.Mine) {
                    cell.revealed = true;
                    state[x,y] = cell;
                }
            }
        }
        board.Draw(state);

        finalScore = ScoreCounter.score;
        HighScore.TRY_SET_HIGH_SCORE(ScoreCounter.score);
        ScoreCounter.score = 0;
        Timer.timeCountdown = 59;
        Level.level = 0;
        mineCount = sMineCount;

        Invoke("SceneChange", 1f);
    }

    void SceneChange()
    {
        SceneManager.LoadScene("Gameover");
    }

    Cell GetCell(int x, int y)
    {
        if(isValid(x,y)) {
            return state[x,y];
        } else {
            return new Cell(); // returns invalid cell
        }
    }

    bool isValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height; // checks if cell is within bounds
    }
}
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
    static public bool gameover; // For end of each level
    static public bool start; // For first click square
    static public bool end; // For end of game
    static public int counter; // Number of safe squares clicked by user
    static public int finalScore;
    
    void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    void Start()
    {
        NewLevel();
    }

    public void NewLevel() // Starts a new level, resetting all the variables
    {
        state = new Cell[width, height];
        Level.level += 1;
        mineCount += 3;
        mineCount = Mathf.Clamp(mineCount, 0, (int) ((width * height) * .19)); // Limits the number of mines
        if(Difficulty.difficulty == "Easy") {
            objective = Random.Range((int) ((width * height) * 0.1), (int) ((width * height) * 0.15)); // Sets a random objective count
        } else{
            objective = Random.Range((int) ((width * height) * 0.07), (int) ((width * height) * 0.1)); // Sets a random objective count
        }
        counter = 0;
        gameover = false;
        start = false;
        end = false;

        GenerateCells();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f); // Center camera
        board.Draw(state);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { // Pressing ESC shows a Menu with the option to Restart or Exit
            gameover = true;
            SceneManager.LoadScene("Menu");
        }
        if(!start) {
            if(Input.GetMouseButtonDown(0)) { // First left-click ensures no mine is clicked
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
                Cell firstCell = GetCell(cellPos.x, cellPos.y);
                if(firstCell.type != Cell.Type.Invalid) {
                    firstCell.type = Cell.Type.Initial;
                    state[cellPos.x, cellPos.y] = firstCell;
                    // Sets adjacent squares of the first clicked square as Initial type to avoid being a mine
                    for(int adjX = -1; adjX <= 1; adjX++) {
                        for(int adjY = -1; adjY <= 1; adjY++) {
                            if(adjX == 0 && adjY == 0) {
                                continue;
                            }

                            int x = cellPos.x + adjX;
                            int y = cellPos.y + adjY;

                            Cell cell = GetCell(x,y);
                            if(cell.type != Cell.Type.Invalid) { // Ensures cell is within bounds
                                state[x,y].type = Cell.Type.Initial;
                            }
                        }
                    }
                    GenerateMines();
                    // Sets adjacent squares of the first clicked square back to Empty type
                    for(int adjX = -1; adjX <= 1; adjX++) {
                        for(int adjY = -1; adjY <= 1; adjY++) {
                            int x = cellPos.x + adjX;
                            int y = cellPos.y + adjY;

                            Cell cell = GetCell(x,y);
                            if(cell.type != Cell.Type.Invalid) { // Ensures cell is within bounds
                                state[x,y].type = Cell.Type.Empty;
                            }
                        }
                    }
                    GenerateNumbers();
                    board.Draw(state);
                    Reveal(); // Reveal the first clicked square
                    start = true;
                }
            }
        } else if(!gameover) {
            if(Input.GetMouseButtonDown(1)) { // Flags square when right click
                Flag();
            } else if(Input.GetMouseButtonDown(0)) { // Reveals square when left click
                Reveal();
            }
        } else if(end) { // End of game
            GameOver();
        }
    }

    void GenerateCells() // Creates cells for the size of the board
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

    void GenerateMines() // Creates random mines across the board
    {
        for(int i = 0; i < mineCount; i++) {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while(state[x,y].type == Cell.Type.Mine || state[x,y].type == Cell.Type.Initial) {
                x++;
                
                if(x >= width) { // Ensures within bounds
                    x = 0;
                    y++; // Increment row

                    if(y >= height) { // Ensures within bounds
                        y = 0;
                    }
                }
            }

            state[x,y].type = Cell.Type.Mine;
        }
    }

    void GenerateNumbers() // Sets numbered squares indicating number of mines
    {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                Cell cell = state[x,y];

                if(cell.type == Cell.Type.Mine) { // Skips mines
                    continue;
                }

                cell.number = CountMines(x,y); // Counts number of mines
                
                if(cell.number > 0) {
                    cell.type = Cell.Type.Number;
                }

                state[x,y] = cell;
            }
        }
    }

    int CountMines(int cellX, int cellY) { // Counts the number of mines adjacent to a square
        int count = 0;

        for(int adjX = -1; adjX <= 1; adjX++) {
            for(int adjY = -1; adjY <= 1; adjY++) {
                if(adjX == 0 && adjY == 0) {
                    continue; // Skips initial square
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

    void Flag() // A flag to mark square as a mine
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        if(cell.type == Cell.Type.Invalid || cell.revealed) { // Avoids revealed cells and out of bounds
            return;
        }

        cell.flagged = !cell.flagged;
        state[cellPos.x, cellPos.y] = cell;
        board.Draw(state);
    }

    void Reveal() // Reveals the square user clicked
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(worldPos);
        Cell cell = GetCell(cellPos.x, cellPos.y);

        if(cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged) {
            return;
        }

        switch(cell.type) {
            case Cell.Type.Mine:
                Explode(cell); // If square clicked is a mine, explode (decrement score and time)
                break;
            case Cell.Type.Empty: // Safe square
                Timer.timeCountdown += 0.25f; // Adds 0.25 seconds to time
                ScoreCounter.score += 10; // Adds 10 points to score
                counter += 1; // Adds 1 to counter
                RevealEmpty(cell);
                CheckWin(); // Check if need to go to next level
                break;
            default: // Safe square (numbered)
                Timer.timeCountdown += 0.25f; // Adds 0.25 seconds to time
                ScoreCounter.score += 10; // Adds 10 points to score
                counter += 1; // Adds 1 to counter
                cell.revealed = true;
                state[cellPos.x, cellPos.y] = cell;
                CheckWin(); // Check if need to go to next level
                break;
        }

        board.Draw(state);
    }

    void RevealEmpty(Cell cell) // Reveals adjacent empty cells
    {
        if(cell.revealed) {
            return;
        }
        if(cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) {
            return;
        }

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if(cell.type == Cell.Type.Empty) { // Recursion to reveal all adjacent empty cells
            for(int adjX = -1; adjX <= 1; adjX++) {
                for(int adjY = -1; adjY <= 1; adjY++) {
                    if(adjX == 0 && adjY == 0) {
                        continue; // Skips initial cell
                    }

                    int x = cell.position.x + adjX;
                    int y = cell.position.y + adjY;

                    RevealEmpty(GetCell(x,y));
                }
            }
        }
    }

    void Explode(Cell cell) // Mine was revealed
    {
        cell.revealed = true;
        cell.exploded = true;
        state[cell.position.x, cell.position.y] = cell;

        Timer.timeCountdown -= 20; // Decrements 20 seconds
        ScoreCounter.score -= 10; // Decrements 10 points
    }

    void CheckWin() // Check if need to go to next level
    {
        if(counter != objective) { // Checks if number of safe squares clicked doesn't meet objective number
            return;
        }

        // Objective has been met
        gameover = true; // Pauses timer and stops mouse input

        for(int x = 0; x < width; x++) { // Flag remaining mines
            for(int y = 0; y < height; y++) {
                Cell cell = state[x,y];

                if(cell.type == Cell.Type.Mine) {
                    cell.flagged = true;
                    state[x,y] = cell;
                }
            }
        }
        
        Timer.timeCountdown += 5; // Adds 5 seconds to time
        Timer.uiText.text = Timer.GetTime();
        ScoreCounter.uiText.text = "Score: " + ScoreCounter.score.ToString("#,0"); // Updates time text
        Invoke("NewLevel", 1f); // Start next level
    }

    void GameOver() // Game has ended, time ran out
    {
        end = false; // Revert end back to false
        for(int x = 0; x < width; x++) { // Reveal remaining mines
            for(int y = 0; y < height; y++) {
                Cell cell = state[x,y];

                if(cell.type == Cell.Type.Mine) {
                    cell.revealed = true;
                    state[x,y] = cell;
                }
            }
        }
        board.Draw(state);

        finalScore = ScoreCounter.score; // To display final score at gameover scene
        HighScore.TRY_SET_HIGH_SCORE(ScoreCounter.score); // Checks if new high score
        // Resets variables
        ScoreCounter.score = 0;
        Timer.timeCountdown = 59;
        Level.level = 0;
        mineCount = sMineCount;

        Invoke("SceneChange", 1f); // Goes to gameover scene
    }

    void SceneChange() // Changes scene to gameover
    {
        SceneManager.LoadScene("Gameover");
    }

    Cell GetCell(int x, int y)
    {
        if(isValid(x,y)) {
            return state[x,y]; // Returns cell in board
        } else {
            return new Cell(); // Returns invalid cell
        }
    }

    bool isValid(int x, int y) // Checks if cell is within bounds
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
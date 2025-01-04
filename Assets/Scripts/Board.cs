using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class Board : MonoBehaviour
{
    private System.Random _rand = new System.Random();

    int[,] solvedGrid = new int[9,9];
    string s;

    int[,] riddleGrid = new int[9,9];
    int piecesToErase = 35;

    public Transform A1, A2, A3, B1, B2, B3, C1, C2, C3;
    public GameObject buttonPrefab;
    public GameObject dotPrefab;

    List<NumberField> fieldList = new List<NumberField>();

    //difficulties
    public enum Difficulties
    {
        DEBUG,
        EASY,
        MEDIUM,
        HARD,
        NIGHTMARE
    }

    public GameObject winPanel;
    public GameObject failPanel;
    public GameObject hintText;

    public Difficulties difficulty;
    int maxHints;

    public List<int> riddleListEasy;
    public List<int> riddleListMedium;
    public List<int> riddleListHard;
    public List<int> riddleListNightmare;

    public List<int> newRiddleListEasy;
    public List<int> newRiddleListMedium;
    public List<int> newRiddleListHard;
    public List<int> newRiddleListNightmare;

    public List<int> solvedListEasy;
    public List<int> solvedListMedium;
    public List<int> solvedListHard;
    public List<int> solvedListNightmare;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    void Start()
    {
        winPanel.SetActive(false);
        failPanel.SetActive(false);

        difficulty = (Board.Difficulties)Settings.difficulty;

        //This whole section creates everything
        //FillGridBase(ref solvedGrid);
        //SolveGrid(ref solvedGrid);
        //CreateRiddleGrid(ref solvedGrid, ref riddleGrid);
        //CreateButtons();


        LoadGame();


        //Debug.Log(riddleList[2]);
        

        //InitGrid(ref solvedGrid);

        //ShuffleGrid(ref solvedGrid, 10); //shuffles 10 times
        //CreateRiddleGrid();
        //CreateButtons();
    }

    void InitGrid(ref int[,] grid)
    {
        for(int i = 0; i<9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1;
            }
        }

        //int n1 = 8 * 3; //24
        //int n2 = 8 / 3; // 2;
        //int n = (n1 + n2 + 0) % 9 + 1;
        //print(n1 + "+" + n2 + "+" + 0);
        //print(n);
    }

    void DebugGrid(ref int[,] grid)
    {
        s = "";
        int sep = 0;
        for (int i = 0; i < 9; i++)
        {
            s += "|";
            for (int j = 0; j < 9; j++)
            {
                s += grid[i, j].ToString();

                sep = j % 3;
                if (sep == 2)
                {
                    s += "|";
                }
            }

            s += "\n";            
        }
        print(s);
    }

    void ShuffleGrid(ref int[,] grid, int shuffleAmount)
    {
        for (int i = 0; i < shuffleAmount; i++)
        {
            int value1 = UnityEngine.Random.Range(1, 10);
            int value2 = UnityEngine.Random.Range(1, 10);
            //Mix 2 cells
            MixTwoGridCells(ref grid, value1, value2);
        }
        //DebugGrid(ref grid);
    }

    void MixTwoGridCells(ref int[,] grid, int value1, int value2)
    {
        int x1 = 0;
        int x2 = 0;
        int y1 = 0;
        int y2 = 0;

        for (int i = 0; i < 9; i+=3)
        {
            for (int k = 0; k < 9; k+=3)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        if(grid[i+j, k+l] == value1)
                        {
                            x1 = i + j;
                            y1 = k + l;
                        }

                        if (grid[i + j, k + l] == value2)
                        {
                            x2 = i + j;
                            y2 = k + l;
                        }
                    }
                }
                grid[x1, y1] = value2;
                grid[x2, y2] = value1;
            }
        }
    }

    //void CreateRiddleGrid()
    //{
    //    //copy the solved grid
    //    for (int i = 0; i < 9; i++)
    //    {
    //        for (int j = 0; j < 9; j++)
    //        {
    //            riddleGrid[i, j] = solvedGrid[i,j];
    //        }
    //    }

    //    //set difficulty

    //    SetDifficulty(); //decides piecesToErase variable

    //    //erase from riddleGrid
    //    for (int i = 0; i < piecesToErase; i++)
    //    {
    //        int x1 = UnityEngine.Random.Range(0, 9);
    //        int y1 = UnityEngine.Random.Range(0, 9);

    //        //reroll until we find one without a 0
    //        while(riddleGrid[x1, y1] == 0)
    //        {
    //            x1 = UnityEngine.Random.Range(0, 9);
    //            y1 = UnityEngine.Random.Range(0, 9);
    //        }
    //        //once we found one without no 0
    //        riddleGrid[x1, y1] = 0;
    //    }
    //    //DebugGrid(ref riddleGrid);
    //}

    void CreateButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject newButton = Instantiate(buttonPrefab);

                //set all value
                NumberField numField = newButton.GetComponent<NumberField>();
                numField.SetValues(i, j, riddleGrid[i,j], i + "," + j, this);
                newButton.name = i + "," + j;
                SetButtonColor(newButton.GetComponent<Button>(), newButton.GetComponentInChildren<Text>().text);                


                if (riddleGrid[i,j] == 0)
                {
                    fieldList.Add(numField);
                }
                else
                {
                    GameObject dot = Instantiate(dotPrefab);
                    dot.transform.SetParent(newButton.transform, false);
                }

                //parent the button
                //A1
                if(i < 3 && j < 3)
                {
                    newButton.transform.SetParent(A1, false);
                }
                //A2
                if (i < 3 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(A2, false);
                }
                //A3
                if (i < 3 && j > 5)
                {
                    newButton.transform.SetParent(A3, false);
                }
                //B1
                if (i > 2 && i < 6 && j < 3)
                {
                    newButton.transform.SetParent(B1, false);
                }
                //B2
                if (i > 2 && i < 6 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(B2, false);
                }
                //B3
                if (i > 2 && i < 6 && j > 5)
                {
                    newButton.transform.SetParent(B3, false);
                }
                //C1
                if (i > 5 && j < 3)
                {
                    newButton.transform.SetParent(C1, false);
                }
                //C2
                if (i > 5 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(C2, false);
                }
                //C3
                if (i > 5 && j > 5)
                {
                    newButton.transform.SetParent(C3, false);
                }
            }
        }
    }

    public void SetInputInRiddleGrid(int x, int y, int value)
    {
        riddleGrid[x, y] = value;
        SetButtonColor(GameObject.Find(x + "," + y).GetComponent<Button>(), riddleGrid[x, y].ToString());
    }

    void SetDifficulty()
    {
        switch (difficulty)
        {
            case Difficulties.DEBUG:
                piecesToErase = 5;
                maxHints = 4;
                break;
            case Difficulties.EASY:
                piecesToErase = 30;
                PlayerPrefs.SetInt("easyHint", 5);
                maxHints = 5;
                hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                break;
            case Difficulties.MEDIUM:
                piecesToErase = 40;
                PlayerPrefs.SetInt("mediumHint", 3);
                maxHints = 3;
                hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                break;
            case Difficulties.HARD:
                piecesToErase = 50;
                PlayerPrefs.SetInt("hardHint", 2);
                maxHints = 2;
                hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                break;
            case Difficulties.NIGHTMARE:
                piecesToErase = 60;
                hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                break;
        }
    }

    public void CheckComplete()
    {
        if (CheckIfWon())
        {
            winPanel.SetActive(true);
            switch (difficulty)
            {
                case Difficulties.EASY:
                    PlayerPrefs.DeleteKey("easySave");
                    break;
                case Difficulties.MEDIUM:
                    PlayerPrefs.DeleteKey("mediumSave");
                    break;
                case Difficulties.HARD:
                    PlayerPrefs.DeleteKey("hardSave");
                    break;
                case Difficulties.NIGHTMARE:
                    PlayerPrefs.DeleteKey("nightmareSave");
                    break;
            }
            SaveClear();
        }
        else
            failPanel.SetActive(true);
    }
    bool CheckIfWon()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (riddleGrid[i,j] != solvedGrid[i,j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ShowHint()
    {
        if (fieldList.Count > 0 && maxHints > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, fieldList.Count);

            maxHints--;
            hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
            
            
            riddleGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()] = solvedGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()];

            //saving into newRiddleGrid
            SaveHints(fieldList[randIndex].GetX(), fieldList[randIndex].GetY());

            fieldList[randIndex].SetHint(riddleGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()]);

            //Set to color instead of number after setting hint to number
            SetButtonColor(GameObject.Find(fieldList[randIndex].GetX() + "," + fieldList[randIndex].GetY()).GetComponent<Button>(), riddleGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()].ToString());
            GameObject dot = Instantiate(dotPrefab);
            dot.transform.SetParent(GameObject.Find(fieldList[randIndex].GetX() + "," + fieldList[randIndex].GetY()).transform, false);

            fieldList.RemoveAt(randIndex);
        }
        else
        {
            StartCoroutine(OutOfHints(1.5f));
            //print("Out of hints");
        }
    }

    //Back-tracking


    //All checks
    bool ColumnContainsNumber(int y, int value, ref int[,] grid)
    {
        for (int x = 0; x < 9; x++)
        {
            if(grid[x, y] == value)
            {
                return true;
            }
        }
        return false;
    }
    bool RowContainsNumber(int x, int value, ref int[,] grid)
    {
        for (int y = 0; y < 9; y++)
        {
            if (grid[x, y] == value)
            {
                return true;
            }
        }
        return false;
    }
    bool BlockContainsNumber(int x, int y, int value, ref int[,] grid)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[x - (x % 3)+i, y - (y % 3)+j] == value)
                {
                    return true;
                }
            }
        }
        return false;
    }
    bool CheckAll(int x, int y, int value, ref int[,] grid)
    {
        if (ColumnContainsNumber(y, value, ref grid)) return false;
        if (RowContainsNumber(x, value, ref grid)) return false;
        if (BlockContainsNumber(x, y, value, ref grid)) return false;
        return true;
    }
    bool IsValidGrid(ref int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //Generate Grid
    void FillGridBase(ref int [,] grid)
    {
        List<int> rowValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> colValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int value = rowValues[UnityEngine.Random.Range(0, rowValues.Count)];
        grid[0, 0] = value;
        rowValues.Remove(value);
        colValues.Remove(value);
        //Rows first
        for (int i = 1; i < 9; i++)
        {
            value = rowValues[UnityEngine.Random.Range(0, rowValues.Count)];
            grid[i, 0] = value;
            rowValues.Remove(value);
        }
        //Columns here
        for (int i = 1; i < 9; i++)
        {
            value = colValues[UnityEngine.Random.Range(0, colValues.Count)];
            if (i < 3)
            {
                while (BlockContainsNumber(0, 0, value, ref grid))
                {
                    value = colValues[UnityEngine.Random.Range(0, colValues.Count)];
                }
            }
            grid[0, i] = value;
            colValues.Remove(value);
        }
        //DebugGrid(ref grid);
    }
    bool SolveGrid(ref int[,] grid)
    {
        //DebugGrid(ref grid);

        if (IsValidGrid(ref grid))
        {
            return true;
        }

        //Find first free cell
        int x = 0;
        int y = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }
        List<int> possibilities = new List<int>();
        possibilities = GetAllPossibilities(x, y, ref grid);

        for (int p = 0; p < possibilities.Count; p++)
        {
            //set a possible value
            grid[x, y] = possibilities[p];
            //back track
            if(SolveGrid(ref grid))
            {
                return true;
            }

            grid[x, y] = 0;
        }


        return false;
    }
    List<int> GetAllPossibilities(int x, int y, ref int[,] grid)
    {
        List<int> possibilities = new List<int>();
        for (int val = 1; val <= 9; val++)
        {
            if (CheckAll(x, y, val, ref grid))
            {
                possibilities.Add(val);
            }
        }
        return possibilities;
    }
    //new game play
    void CreateRiddleGrid(ref int[,] sGrid, ref int[,] rGrid)
    {
        //copy the solved grid
        System.Array.Copy(sGrid, rGrid, sGrid.Length);

        //set difficulty

        SetDifficulty(); //decides piecesToErase variable

        //erase mirrors from riddleGrid
        for (int i = 0; i < 20; i++)
        {
            int x1 = UnityEngine.Random.Range(0, 9);
            int y1 = UnityEngine.Random.Range(0, 9);

            //reroll until we find one without a 0
            while (rGrid[x1, y1] == 0)
            {
                x1 = UnityEngine.Random.Range(0, 9);
                y1 = UnityEngine.Random.Range(0, 9);
            }
            //once we found one without no 0
            int temp = rGrid[x1, y1];
            int temp2 = rGrid[Mathf.Abs(x1 - 8), Mathf.Abs(y1 - 8)];
            rGrid[x1, y1] = 0;
            rGrid[Mathf.Abs(x1 - 8), Mathf.Abs(y1 - 8)] = 0;

            //rGrid[Mathf.Abs(x1 - 8), Mathf.Abs(y1 - 8)] = 0;
            //i++;

            var guessArray = Enumerable.Range(1, 9).OrderBy(o => _rand.Next()).ToArray();
            int check = 0;
            CheckUniqueness(rGrid, guessArray, ref check);
            if (check != 1)
            {
                rGrid[x1, y1] = temp;
                rGrid[Mathf.Abs(x1 - 8), Mathf.Abs(y1 - 8)] = temp2;
            }
        }

        int total0 = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (rGrid[i, j] == 0)
                    total0++;
            }
        }

        piecesToErase = piecesToErase - total0;

        //erase from riddleGrid
        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = UnityEngine.Random.Range(0, 9);
            int y1 = UnityEngine.Random.Range(0, 9);

            //reroll until we find one without a 0
            while (rGrid[x1, y1] == 0)
            {
                x1 = UnityEngine.Random.Range(0, 9);
                y1 = UnityEngine.Random.Range(0, 9);
            }
            //once we found one without no 0
            int temp = rGrid[x1, y1];
            rGrid[x1, y1] = 0; //denna ska vara kvar

            //rGrid[Mathf.Abs(x1 - 8), Mathf.Abs(y1 - 8)] = 0;
            //i++;

            var guessArray = Enumerable.Range(1, 9).OrderBy(o => _rand.Next()).ToArray();
            int check = 0;
            CheckUniqueness(rGrid, guessArray, ref check);
            if (check != 1)
            {
                rGrid[x1, y1] = temp;
            }
            else
            {
                rGrid[x1, y1] = 0;
            }
        }

        //int total0s = 0;
        //for (int i = 0; i < 9; i++)
        //{
        //    for (int j = 0; j < 9; j++)
        //    {
        //        if (rGrid[i, j] == 0)
        //            total0s++;
        //    }
        //}
        //Debug.Log(total0s);
    }

    public void DebugThisJoke()
    {
        DebugGrid(ref riddleGrid);
    }

    public void SaveGame()
    {
        switch (difficulty)
        {
            case Difficulties.EASY:
                //need to save solvedGrid, riddleGrid and currentRiddle which is always riddleGrid
                //solvedGrid and riddleGrid can only be saved as they are created and before that they need to be deleted.
                //SaveGame function can be used directly for those 2 but they also need to be unique with difficulty.
                //Load must check only if difficulty save, or else it may create a new grid.
                PlayerPrefs.DeleteKey("easySave");
                riddleListEasy.Clear();
                SaveList(ref riddleGrid, "easySave", riddleListEasy);
                break;
            case Difficulties.MEDIUM:
                PlayerPrefs.DeleteKey("mediumSave");
                riddleListMedium.Clear();
                SaveList(ref riddleGrid, "mediumSave", riddleListMedium);
                break;
            case Difficulties.HARD:
                PlayerPrefs.DeleteKey("hardSave");
                riddleListHard.Clear();
                SaveList(ref riddleGrid, "hardSave", riddleListHard);
                break;
            case Difficulties.NIGHTMARE:
                PlayerPrefs.DeleteKey("nightmareSave");
                riddleListNightmare.Clear();
                SaveList(ref riddleGrid, "nightmareSave", riddleListNightmare);
                break;
        }

        StartCoroutine(SavedText(1.5f));
    }

    void SaveList(ref int[,] grid, string save, List<int> list)
    {
        PlayerPrefs.DeleteKey(save);
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                list.Add(grid[x,y]);
                PlayerPrefsExtra.SetList(save, list);
            }
        }
    }

    void LoadGame()
    {
        switch (difficulty)
        {
            case Difficulties.EASY:
                if (PlayerPrefs.HasKey("easySave"))
                {
                    newRiddleListEasy = PlayerPrefsExtra.GetList<int>("newRiddleListEasy", new List<int>());
                    LoadGrid(ref riddleGrid, newRiddleListEasy);
                    solvedListEasy = PlayerPrefsExtra.GetList<int>("solvedListEasy", new List<int>());
                    LoadGrid(ref solvedGrid, solvedListEasy);
                    CreateButtons();
                    riddleListEasy = PlayerPrefsExtra.GetList<int>("easySave", new List<int>());
                    UpdateButtons(riddleListEasy);
                    UpdateButtons(newRiddleListEasy);
                    maxHints = PlayerPrefs.GetInt("easyHint");
                    hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                }
                else
                {
                    CreateNewBoard();
                }
                break;
            case Difficulties.MEDIUM:
                if (PlayerPrefs.HasKey("mediumSave"))
                {
                    newRiddleListMedium = PlayerPrefsExtra.GetList<int>("newRiddleListMedium", new List<int>());
                    LoadGrid(ref riddleGrid, newRiddleListMedium);
                    solvedListMedium = PlayerPrefsExtra.GetList<int>("solvedListMedium", new List<int>());
                    LoadGrid(ref solvedGrid, solvedListMedium);
                    CreateButtons();
                    riddleListMedium = PlayerPrefsExtra.GetList<int>("mediumSave", new List<int>());
                    UpdateButtons(riddleListMedium);
                    UpdateButtons(newRiddleListMedium);
                    maxHints = PlayerPrefs.GetInt("mediumHint");
                    hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                }
                else
                {
                    CreateNewBoard();
                }
                break;
            case Difficulties.HARD:
                if (PlayerPrefs.HasKey("hardSave"))
                {
                    newRiddleListHard = PlayerPrefsExtra.GetList<int>("newRiddleListHard", new List<int>());
                    LoadGrid(ref riddleGrid, newRiddleListHard);
                    solvedListHard = PlayerPrefsExtra.GetList<int>("solvedListHard", new List<int>());
                    LoadGrid(ref solvedGrid, solvedListHard);
                    CreateButtons();
                    riddleListHard = PlayerPrefsExtra.GetList<int>("hardSave", new List<int>());
                    UpdateButtons(riddleListHard);
                    UpdateButtons(newRiddleListHard);
                    maxHints = PlayerPrefs.GetInt("hardHint");
                    hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                }
                else
                {
                    CreateNewBoard();
                }
                break;
            case Difficulties.NIGHTMARE:
                if (PlayerPrefs.HasKey("nightmareSave"))
                {
                    newRiddleListNightmare = PlayerPrefsExtra.GetList<int>("newRiddleListNightmare", new List<int>());
                    LoadGrid(ref riddleGrid, newRiddleListNightmare);
                    solvedListNightmare = PlayerPrefsExtra.GetList<int>("solvedListNightmare", new List<int>());
                    LoadGrid(ref solvedGrid, solvedListNightmare);
                    CreateButtons();
                    riddleListNightmare = PlayerPrefsExtra.GetList<int>("nightmareSave", new List<int>());
                    UpdateButtons(riddleListNightmare);
                    UpdateButtons(newRiddleListNightmare);
                    hintText.GetComponent<Text>().text = "Hint(" + maxHints.ToString() + ")";
                }
                else
                {
                    CreateNewBoard();
                }
                break;
        }
    }

    void LoadGrid(ref int[,] intoGrid, List<int> fromList)
    {
        int listArray = 0;

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                intoGrid[x,y] = fromList[listArray];
                listArray++;
            }
        }
        
        //DebugGrid(ref riddleGrid);
    }

    void CreateNewBoard()
    {
        FillGridBase(ref solvedGrid);
        SolveGrid(ref solvedGrid);
        CreateRiddleGrid(ref solvedGrid, ref riddleGrid);
        CreateButtons();

        switch (difficulty)
        {
            case Difficulties.EASY:
                PlayerPrefs.DeleteKey("easySave");
                SaveList(ref riddleGrid, "newRiddleListEasy", newRiddleListEasy);
                SaveList(ref solvedGrid, "solvedListEasy", solvedListEasy);
                break;
            case Difficulties.MEDIUM:
                PlayerPrefs.DeleteKey("mediumSave");
                SaveList(ref riddleGrid, "newRiddleListMedium", newRiddleListMedium);
                SaveList(ref solvedGrid, "solvedListMedium", solvedListMedium);
                break;
            case Difficulties.HARD:
                PlayerPrefs.DeleteKey("hardSave");
                SaveList(ref riddleGrid, "newRiddleListHard", newRiddleListHard);
                SaveList(ref solvedGrid, "solvedListHard", solvedListHard);
                break;
            case Difficulties.NIGHTMARE:
                PlayerPrefs.DeleteKey("nightmareSave");
                SaveList(ref riddleGrid, "newRiddleListNightmare", newRiddleListNightmare);
                SaveList(ref solvedGrid, "solvedListNightmare", solvedListNightmare);
                break;
        }
        
    }

    void UpdateButtons(List<int> saveList)
    {
        int listArray = 0;

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (riddleGrid[x, y] != saveList[listArray] && saveList[listArray] != 0)
                {
                    GameObject.Find(x + "," + y).GetComponentInChildren<Text>().text = saveList[listArray].ToString();
                    riddleGrid[x, y] = saveList[listArray];
                    SetButtonColor(GameObject.Find(x + "," + y).GetComponent<Button>(), riddleGrid[x, y].ToString());                    
                }
                listArray++;
            }
        }
    }

    public void NewPuzzle()
    {
        switch (difficulty)
        {
            case Difficulties.EASY:
                PlayerPrefs.DeleteKey("easySave");          
                break;
            case Difficulties.MEDIUM:
                PlayerPrefs.DeleteKey("mediumSave");
                break;
            case Difficulties.HARD:
                PlayerPrefs.DeleteKey("hardSave");
                break;
            case Difficulties.NIGHTMARE:
                PlayerPrefs.DeleteKey("nightmareSave");
                break;
        }

        if (PlayerPrefs.GetInt("loadCount") > 2)
        {
            GameObject.Find("AdManager").GetComponent<AdManager>().ShowInterstitialAd();
        }
        else
        {
            PlayerPrefs.SetInt("loadCount", PlayerPrefs.GetInt("loadCount") + 1);
            SceneManager.LoadScene("Game");
        }
    }

    public void SaveHints(int x, int y)
    {
        int listArray = 0;
        switch (difficulty)
        {
            case Difficulties.EASY:
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if ((x,y) == (i,j))
                        {
                            newRiddleListEasy[listArray] = riddleGrid[x, y];
                        }
                        listArray++;
                    }
                }
                PlayerPrefs.DeleteKey("newRiddleListEasy");
                PlayerPrefsExtra.SetList("newRiddleListEasy", newRiddleListEasy);
                PlayerPrefs.SetInt("easyHint", maxHints);
                break;
            case Difficulties.MEDIUM:
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if ((x, y) == (i, j))
                        {
                            newRiddleListMedium[listArray] = riddleGrid[x, y];
                        }
                        listArray++;
                    }
                }
                PlayerPrefs.DeleteKey("newRiddleListMedium");
                PlayerPrefsExtra.SetList("newRiddleListMedium", newRiddleListMedium);
                PlayerPrefs.SetInt("mediumHint", maxHints);
                break;
            case Difficulties.HARD:
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if ((x, y) == (i, j))
                        {
                            newRiddleListHard[listArray] = riddleGrid[x, y];
                        }
                        listArray++;
                    }
                }
                PlayerPrefs.DeleteKey("newRiddleListHard");
                PlayerPrefsExtra.SetList("newRiddleListHard", newRiddleListHard);
                PlayerPrefs.SetInt("hardHint", maxHints);
                break;
            case Difficulties.NIGHTMARE:
                break;
        }
    }

    public void SaveClear()
    {
        switch (difficulty)
        {
            case Difficulties.EASY:
                PlayerPrefs.SetInt("easyClear", PlayerPrefs.GetInt("easyClear") + 1);
                break;
            case Difficulties.MEDIUM:
                PlayerPrefs.SetInt("mediumClear", PlayerPrefs.GetInt("mediumClear") + 1);
                break;
            case Difficulties.HARD:
                PlayerPrefs.SetInt("hardClear", PlayerPrefs.GetInt("hardClear") + 1);
                break;
            case Difficulties.NIGHTMARE:
                PlayerPrefs.SetInt("nightmareClear", PlayerPrefs.GetInt("nightmareClear") + 1);
                break;
        }
    }

    private void SetButtonColor(Button button, string i)
    {
        switch (i)
        {
            case "0":
                button.GetComponent<Image>().color = new Color32(255, 255, 255, 255); //eraseButton
                break;
            case "1":
                button.GetComponent<Image>().color = new Color32(0, 255, 255, 255); //cyan //new Color32(0, 0, 0, 255); //black
                break;
            case "2":
                button.GetComponent<Image>().color = new Color32(0, 0, 255, 255); //blue
                break;
            case "3":
                button.GetComponent<Image>().color = new Color32(255, 0, 0, 255); //red
                break;
            case "4":
                button.GetComponent<Image>().color = new Color32(0, 255, 0, 255); //green
                break;
            case "5":
                button.GetComponent<Image>().color = new Color32(255, 255, 0, 255); //yellow
                break;
            case "6":
                button.GetComponent<Image>().color = new Color32(153, 76, 0, 255); //brown //new Color32(160, 160, 160, 255); //gray
                break;
            case "7":
                button.GetComponent<Image>().color = new Color32(255, 0, 255, 255); //pink
                break;
            case "8":
                button.GetComponent<Image>().color = new Color32(255, 128, 0, 255); //orange
                break;
            case "9":
                button.GetComponent<Image>().color = new Color32(153, 51, 255, 255); //purple
                break;
        }
    }

    IEnumerator SavedText (float waitTime)
    {
        GameObject.Find("SaveButton").GetComponentInChildren<Text>().text = "Saved";
        GameObject.Find("SaveButton").GetComponentInChildren<Text>().color = Color.green;
        yield return new WaitForSeconds(waitTime);
        GameObject.Find("SaveButton").GetComponentInChildren<Text>().text = "Save";
        GameObject.Find("SaveButton").GetComponentInChildren<Text>().color = Color.yellow;
    }

    IEnumerator OutOfHints (float waitTime)
    {
        hintText.GetComponent<Text>().color = Color.red;
        yield return new WaitForSeconds(waitTime);
        hintText.GetComponent<Text>().color = Color.yellow;
    }

    ////SOLVE

    private void CheckUniqueness(int[,] grid, int[] guessArray, ref int number)
    {
        int row = 0, col = 0;

        if (!FindEmptyLocation(grid, ref row, ref col))
        {
            number++;
            return;
        }

        for (int i = 0; i < 9 && number < 2; i++)
        {
            if (IsSafe(grid, row, col, guessArray[i]))
            {
                grid[row, col] = guessArray[i];
                CheckUniqueness(grid, guessArray, ref number);
            }

            grid[row, col] = 0;
        }
    }

    private bool FindEmptyLocation(int[,] grid, ref int row, ref int col)
    {
        for (row = 0; row < 9; row++)
            for (col = 0; col < 9; col++)
                if (grid[row, col] == 0)
                    return true;
        return false;
    }

    private bool IsSafe(int[,] grid, int row, int col, int value)
    {
        return !UsedInRow(grid, row, value) && !UsedInCol(grid, col, value) && !UsedInBox(grid, row - row % 3, col - col % 3, value);
    }

    private bool UsedInRow(int[,] grid, int row, int value)
    {
        for (int i = 0; i < 9; i++)
            if (grid[row, i] == value)
                return true;
        return false;
    }

    private bool UsedInCol(int[,] grid, int col, int value)
    {
        for (int i = 0; i < 9; i++)
            if (grid[i, col] == value)
                return true;
        return false;
    }

    private bool UsedInBox(int[,] grid, int boxStartRow, int boxStartCol, int value)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (grid[i + boxStartRow, j + boxStartCol] == value)
                    return true;
        return false;
    }
}

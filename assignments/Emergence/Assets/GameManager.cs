using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject cellPrefab;
    CellScript[,] grid;
    float spacing = 1.1f;

    bool[,] blockPattern = new bool[,]  //help from wiki and chat gpt
    {
        { true, true },
        { true, true }
    };

    bool[,] gliderPattern = new bool[,] //help from wiki and chat gpt
    {
        { false, true, false },
        { false, false, true },
        { true, true, true }
    };

    bool[,] lShapePattern = new bool[,] //help from wiki and chat gpt
    {
        { true, false },
        { true, false },
        { true, true }
    };


    // Start is called before the first frame update
    void Start()
    {
        grid = new CellScript[100, 100];
        for(int x = 0; x < 100; x++){
            for(int y = 0; y < 100; y++){
                Vector3 pos = transform.position;
                pos.x += x * spacing;
                pos.z += y * spacing;
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity);
                grid[x,y] = cell.GetComponent<CellScript>();
                grid[x,y].alive = (Random.value > 0.5f);
                grid[x,y].xIndex = x;
                grid[x,y].yIndex = y;
            }
        }   
    }

    public int CountNeighbors(int xIndex, int yIndex)
    {
        int count = 0;

        for(int x = xIndex - 1; x <= xIndex + 1; x++)
        {
            for(int y = yIndex - 1; y <= yIndex + 1; y++)
            {
                //prevention from indexing the two dimensional array out of bounds: 
                if(x >= 0 && x < 100 && y >= 0 && y < 100)
                {
                    if(!(x == xIndex && y == yIndex))
                    {
                        if(grid[x,y].alive)
                        {
                            count ++; 
                        }
                    }
                }
            }
        }
        return count; 
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Evolve our grid
            Simulate();
            SpreadLava();
        }
        DetectPatterns();   
    }

    void Simulate()
    {
        bool [,] nextAlive = new bool[100, 100]; 
        for(int x = 0; x < 100; x++)
        {
            for(int y = 0; y < 100; y ++)
            {
                int neighborCount = CountNeighbors(x, y);
                if(grid[x,y].alive && neighborCount < 2){
                    //underpopulation
                    nextAlive[x,y] = false;
                }else if(grid[x,y].alive && (neighborCount == 2 || neighborCount == 3)){
                    //healthy community
                    nextAlive[x,y] = true;
                }else if(grid[x,y].alive && neighborCount > 3){
                    //overpopulation
                    nextAlive[x,y] = false;
                }else if (!grid[x,y].alive && neighborCount == 3){
                    //reproduction
                    nextAlive[x,y] = true;
                }else{
                    nextAlive[x,y] = grid[x,y].alive;
                }

                if(!nextAlive[x,y] && grid[x,y].isLava)
                {
                    grid[x,y].isLava = false;
                    grid[x,y].SetColor();
                }

                grid[x, y].alive = nextAlive[x, y];
                grid[x, y].SetColor();

            // Trigger lava spikes for cells alive for a long time
                if (grid[x, y].lavaCount > 5 && Random.value < 0.2f)
                {
                    grid[x, y].TurnToLava(); // Can add spikes or other effects here
                }
            }
        }
    }

    void DetectPatterns()
    {
        for (int x = 0; x < 100 - 1; x++)  // Adjust range to prevent out-of-bounds
        {
            for (int y = 0; y < 100 - 1; y++)
            {
                if (IsPatternMatch(x, y, blockPattern))
                {
                // Trigger lava for all cells in the pattern
                TriggerLavaForPattern(x, y, blockPattern);
                }
                else if (IsPatternMatch(x, y, gliderPattern))
                {
                    TriggerLavaForPattern(x, y, gliderPattern);
                }
                else if (IsPatternMatch(x, y, lShapePattern))
                {
                    TriggerLavaForPattern(x, y, lShapePattern);
                }
            }
        }
    }

    bool IsPatternMatch(int startX, int startY, bool[,] pattern)
    {
        int patternWidth = pattern.GetLength(0);
        int patternHeight = pattern.GetLength(1);
    
        for (int x = 0; x < patternWidth; x++)
        {
            for (int y = 0; y < patternHeight; y++)
            {
                int gridX = startX + x;
                int gridY = startY + y;

            // Ensure we're not out of bounds
                if (gridX >= 100 || gridY >= 100) 
                    {
                        return false;
                    }
                    
                if (grid[gridX, gridY].alive != pattern[x, y])
                    {
                        return false; // pattern does not match
                    }    
                }
            }
        return true;
    }
    


    //     //Copy over updated values of alive
    //     for(int x = 0; x < 100; x++)
    //     {
    //         for(int y = 0; y < 100; y++)
    //         {
    //             grid[x,y].alive = nextAlive[x,y];
    //             grid[x,y].SetColor();

    //             if(grid[x,y].isLava)
    //             {
                
    //             // Increment how many times the cell has ever been alive (we use this in SetColor to have
    //             // the cell's color based on how many times it has been alive).
                
    //                 grid[x,y].lavaCount++;

    //             // Make it so that we make the cell a little taller every time it is alive.
    //             // grid[x,y].gameObject.transform.localScale = new Vector3(grid[x,y].gameObject.transform.localScale.x, 
    //             //                                                         grid[x,y].gameObject.transform.localScale.y + .5f, 
    //             //                                                         grid[x,y].gameObject.transform.localScale.z);
    //             // }
    //             }
    //         }
    //     }
    // }
    
    void TriggerLavaForPattern(int startX, int startY, bool[,] pattern)
    {
        int patternWidth = pattern.GetLength(0);
        int patternHeight = pattern.GetLength(1);

        for (int x = 0; x < patternWidth; x++)
        {
            for (int y = 0; y < patternHeight; y++)
            {
                int gridX = startX + x;
                int gridY = startY + y;

                if (grid[gridX, gridY].alive)
                {
                    grid[gridX, gridY].TurnToLava();
                }
            }
        }
    }
    void SpreadLava()
    {
        for(int x = 0; x < 100; x++)
        {
            for(int y = 0; y < 100; y++)
            {
                if(grid[x,y].alive && Random.value < 0.1f)
                {
                    grid[x,y].TurnToLava();
                }
            }
        }
    }

}

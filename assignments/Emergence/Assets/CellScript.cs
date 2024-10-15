using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    public Renderer cubeRenderer; 
    
    public bool alive = false; 
    public bool isLava = false;
    public float timeDead = 0f;

    public int lavaCount = 0; 

    public int xIndex = -1;
    public int yIndex = -1; 

    public Color aliveColor; 
    public Color deadColor; 
    public Color lavaColor;

    public float reviveTime = 5f;

    public float maxLavaHeight = 1f;

    GameManager gameManager;

    private Vector3 startPosition;
    public float burstHeight = 0.5f;
    public float burstDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        SetColor();

        GameObject gmObj = GameObject.Find("GameManagerObject");
        gameManager = gmObj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!alive && timeDead >= reviveTime)
        {
            ReviveCell(); 
        }

        // if(isLava)
        // {
        //     RaiseLava();
        // }
    }

    void OnMouseDown()
    {
        alive = !alive;
        timeDead = 0f;
        SetColor(); 

        //Count my neighbors
        int neighborCount = gameManager.CountNeighbors(xIndex, yIndex);
        Debug.Log("(" + xIndex + "," + yIndex + "): " + neighborCount);

        if(alive)
        {
            TurnToLava();
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click for Force Push
        {
            alive = false;
            isLava = true; 
            SetColor();
            StartCoroutine(LavaBurst()); // Optional: trigger lava burst on Force Push
        }
    }


    public void SetColor(){
        if(alive){
            cubeRenderer.material.color = aliveColor;

        }else if(isLava){ 
            cubeRenderer.material.color = lavaColor;
        }else{
            cubeRenderer.material.color = deadColor;
        }
    }

    public void TurnToLava()
    {
        if(alive)
        {
            isLava = true;
            alive = false;
            SetColor();
            StartCoroutine(LavaBurst());
        }
    }

    private IEnumerator LavaBurst()
    {
        float originalHeight = transform.localScale.y;
    
        while (isLava)
        {
            float randomHeight = Random.Range(0.3f, 0.8f);
        
            transform.position = new Vector3(transform.position.x, startPosition.y + randomHeight, transform.position.z);
        
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }


    public void SpreadLava(CellScript[,] grid)
    {
        for(int x = xIndex - 1; x <= xIndex + 1; x++)
        {
            for (int y = yIndex - 1; y <= yIndex + 1; y++)
            {
                if(x == xIndex && y == yIndex) continue;

                if(x >= 0 && x < 50 && y >= 0 && y < 50)
                {
                    if(grid[x,y].isLava && Random.value < 0.3f)
                    {
                        grid[x,y].TurnToLava();
                    }
                } 
            }
        }
    }

    public void ReviveCell()
    {
        alive = true; 
        timeDead = 0f;
        
        SetColor(); 
    }
}


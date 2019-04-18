using System.Collections;  
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMaker : MonoBehaviour
{
    public const int WIDTH = 7;
    public const int HEIGHT = 7;

    float xOffset = WIDTH / 2f - 0.5f;
    float yOffset = HEIGHT / 2f - 0.5f;

    public GameObject[,] tiles;
    public List<GameObject> redTiles;
    public List<GameObject> blueTiles;
    public List<GameObject> yellowTiles;
    public List<GameObject> greenTiles;
    public GameObject[] stones;
    public GameObject tilePrefab;
    public GameObject player;
    GameObject gridHolder;
    public GameManager gameManager;

    public GameObject playerGrid;

    public bool setStones;
    public bool waitForColoredLists;
    public bool waitForLerp;

    public static float slideLerp = -1;
    public float lerpSpeed = 0.25f;

    public AudioClip scoreSound;
    public AudioClip destroySound;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        redTiles = new List<GameObject>();
        blueTiles = new List<GameObject>();
        yellowTiles = new List<GameObject>();
        greenTiles = new List<GameObject>();
        stones = new GameObject[2];

        tiles = new GameObject[WIDTH, HEIGHT];

        gridHolder = new GameObject();
        gridHolder.transform.position = new Vector3(-1f, -0.5f, 0);

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                GameObject newTile = Instantiate(tilePrefab);
                newTile.transform.parent = gridHolder.transform;
                newTile.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

                tiles[x, y] = newTile;

                TileScript tileScript = newTile.GetComponent<TileScript>();
                tileScript.SetSprite(Random.Range(0, tileScript.colors.Length));
                tileScript.x = x;
                tileScript.y = y;
            }
        }
        SetAdjacentTiles();
        SetColoredLists();
        SetStones();
        tiles[(WIDTH - 1) / 2, (HEIGHT - 1) / 2].GetComponent<TileScript>().selected = true;
    }

    // Update is called once per frame
    void Update()
    {
        Repopulate();
        if (slideLerp >= 0)
        {
            slideLerp += Time.deltaTime / lerpSpeed;
            if (slideLerp >= 1)
            {
                slideLerp = -1;
                for (int x = 0; x < WIDTH; x++)
                {
                    for (int y = 0; y < HEIGHT; y++)
                    {
                        tiles[x, y].GetComponent<TileScript>().startPosition = tiles[x, y].GetComponent<TileScript>().destPosition;
                        tiles[x, y].GetComponent<TileScript>().canBeSelected = true;
                    }
                }
                waitForLerp = true;
            }
        }
    }

    public void SetColoredLists()
    {
        redTiles.Clear();
        blueTiles.Clear();
        yellowTiles.Clear();
        greenTiles.Clear();

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (tiles[x,y].GetComponent<TileScript>().type == 0)
                {
                    redTiles.Add(tiles[x, y]);
                }
                else if(tiles[x,y].GetComponent<TileScript>().type == 1)
                {
                    blueTiles.Add(tiles[x, y]);
                }
                else if (tiles[x, y].GetComponent<TileScript>().type == 2)
                {
                    yellowTiles.Add(tiles[x, y]);
                }
                else if (tiles[x, y].GetComponent<TileScript>().type == 3)
                {
                    greenTiles.Add(tiles[x, y]);
                }
            }
        }
        if (setStones)
        {
            waitForColoredLists = true;
        }
    }

    public void SetStones()
    {
        int rand = Random.Range(0, 4);
        if (rand == 0)
        {
            int randRed1 = Random.Range(0, redTiles.Count);
            int randRed2 = Random.Range(0, redTiles.Count);
            while(randRed2 == randRed1)
            {
                randRed2 = Random.Range(0, redTiles.Count);
            }
            redTiles[randRed1].GetComponent<TileScript>().isStone = true;
            redTiles[randRed2].GetComponent<TileScript>().isStone = true;
            stones[0] = redTiles[randRed1];
            stones[1] = redTiles[randRed2];
        }
        else if (rand == 1)
        {
            int randBlue1 = Random.Range(0, blueTiles.Count);
            int randBlue2 = Random.Range(0, blueTiles.Count);
            while (randBlue2 == randBlue1)
            {
                randBlue2 = Random.Range(0, blueTiles.Count);
            }
            blueTiles[randBlue1].GetComponent<TileScript>().isStone = true;
            blueTiles[randBlue2].GetComponent<TileScript>().isStone = true;
            stones[0] = blueTiles[randBlue1];
            stones[1] = blueTiles[randBlue2];
        }
        else if (rand == 2)
        {
            int randYellow1 = Random.Range(0, yellowTiles.Count);
            int randYellow2 = Random.Range(0, yellowTiles.Count);
            while (randYellow2 == randYellow1)
            {
                randYellow2 = Random.Range(0, yellowTiles.Count);
            }
            yellowTiles[randYellow1].GetComponent<TileScript>().isStone = true;
            yellowTiles[randYellow2].GetComponent<TileScript>().isStone = true;
            stones[0] = yellowTiles[randYellow1];
            stones[1] = yellowTiles[randYellow2];
        }
        else
        {
            int randGreen1 = Random.Range(0, greenTiles.Count);
            int randGreen2 = Random.Range(0, greenTiles.Count);
            while (randGreen1 == randGreen2)
            {
                randGreen2 = Random.Range(0, greenTiles.Count);
            }
            greenTiles[randGreen1].GetComponent<TileScript>().isStone = true;
            greenTiles[randGreen2].GetComponent<TileScript>().isStone = true;
            stones[0] = greenTiles[randGreen1];
            stones[1] = greenTiles[randGreen2];
        }
    }

    public void SetAdjacentTiles()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                TileScript newTile = tiles[x, y].GetComponent<TileScript>();
                if(newTile.x > 0)
                {
                    newTile.right = tiles[x - 1, y];
                }
                if (newTile.y > 0)
                {
                    newTile.top = tiles[x, y - 1];
                }
                if (newTile.x < WIDTH - 1)
                {
                    newTile.left = tiles[x + 1, y];
                }
                if (newTile.y < HEIGHT - 1)
                {
                    newTile.bottom = tiles[x, y + 1];
                }
            }
        }
    }

    public void DeselectEverything()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                tiles[x, y].GetComponent<TileScript>().selected = false;
            }
        }
    }

    public void RemoveSelected()
    {
        waitForColoredLists = false;
        waitForLerp = false;
        int removability = 0;
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                tiles[x, y].GetComponent<TileScript>().canBeSelected = false;
                if (tiles[x, y].GetComponent<TileScript>().selected)
                {
                    if (!tiles[x, y].GetComponent<TileScript>().isStone)
                    {
                        Destroy(tiles[x, y]);
                    }
                    else
                    {
                        if (tiles[x, y].GetComponent<TileScript>().TouchingSelected())
                        {
                            removability++;
                        }
                    }
                }
            }
        }
        if (removability == 2)
        {
            Destroy(tiles[stones[0].GetComponent<TileScript>().x, stones[0].GetComponent<TileScript>().y]);
            Destroy(tiles[stones[1].GetComponent<TileScript>().x, stones[1].GetComponent<TileScript>().y]);
            setStones = true;
            gameManager.SendMessage("Restart");
            GameManager.score++;
            audioSource.clip = scoreSound;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = destroySound;
            audioSource.Play();
        }
    }

    public bool CanDelete()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (tiles[x,y].GetComponent<TileScript>().destPosition != tiles[x, y].GetComponent<TileScript>().startPosition)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool Repopulate()
    {
        bool repop = false;

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                //tiles[x, y].GetComponent<TileScript>().canBeSelected = false;
                if (tiles[x, y] == null)
                {
                    repop = true;

                    if (y == 0)
                    {
                        tiles[x, y] = Instantiate(tilePrefab);
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        tileScript.SetSprite(Random.Range(0, tileScript.colors.Length));
                        tiles[x, y].transform.parent = gridHolder.transform;
                        tiles[x, y].transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);
                    }
                    else
                    {
                        slideLerp = 0;
                        tiles[x, y] = tiles[x, y - 1];
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        if (tileScript != null)
                        {
                            tileScript.SetupSlide(new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset));
                        }
                        PlayerScript playerScript = tiles[x, y].GetComponent<PlayerScript>();
                        if (playerScript != null)
                        {
                            playerScript.x = x;
                            playerScript.y = y;
                        }

                        tiles[x, y - 1] = null;
                    }
                }
            }
        }
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                //tiles[x, y].GetComponent<TileScript>().canBeSelected = true;
                tiles[x, y].GetComponent<TileScript>().x = x;
                tiles[x, y].GetComponent<TileScript>().y = y;
            }
        }
        SetAdjacentTiles();
        SetColoredLists();
        if (CanDelete() && setStones && waitForColoredLists && waitForLerp)
        {
            SetStones();
            setStones = false;
        }
        int playerx = playerGrid.GetComponent<PlayerGrid>().player.GetComponent<PlayerScript>().x;
        int playery = playerGrid.GetComponent<PlayerGrid>().player.GetComponent<PlayerScript>().y;
        tiles[playerx,playery].GetComponent<TileScript>().selected = true;
        return repop;
    }
}

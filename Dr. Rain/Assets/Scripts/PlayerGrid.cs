using System.Collections;  
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    public const int WIDTH = 7;
    public const int HEIGHT = 7;

    float xOffset = WIDTH / 2f - 0.5f;
    float yOffset = HEIGHT / 2f - 0.5f;

    public GameObject[,] playerGrid;
    public GameObject[,] cloneGrid;
    public GameObject playerPrefab;
    public GameObject clonePrefab;
    public GameObject player;
    public GameObject clone;
    GameObject playerGridHolder;
    GameObject cloneGridHolder;

    public GameObject gridManager;

    public bool canDelete;

    public GameObject switchedTile;
    public GameObject switchedTile2;
    public int switchHelper;
    public bool switching;

    public AudioClip switchSound;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        cloneGrid = new GameObject[WIDTH, HEIGHT];

        cloneGridHolder = new GameObject();
        cloneGridHolder.transform.position = new Vector3(-1f, -0.5f, 0);

        playerGrid = new GameObject[WIDTH, HEIGHT];

        playerGridHolder = new GameObject();
        playerGridHolder.transform.position = new Vector3(-1f, -0.5f, 0);

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (x == (WIDTH - 1) / 2 && y == (HEIGHT - 1) / 2)
                {
                    player = Instantiate(playerPrefab);
                    player.transform.parent = playerGridHolder.transform;
                    player.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

                    playerGrid[x, y] = player;
                    player.GetComponent<PlayerScript>().x = x;
                    player.GetComponent<PlayerScript>().y = y;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && player.GetComponent<PlayerScript>().x > 0 && !GameManager.lost)
        {

            gridManager.GetComponent<GridMaker>().DeselectEverything();

            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            playerGrid[x - 1, y] = player;
            playerGrid[x, y] = null;
            player.transform.localPosition = new Vector2(WIDTH - (x - 1) - xOffset, HEIGHT - y - yOffset);

            player.GetComponent<PlayerScript>().x--;
            gridManager.GetComponent<GridMaker>().tiles[player.GetComponent<PlayerScript>().x, y].GetComponent<TileScript>().selected = true;
        }
        if (Input.GetKeyDown(KeyCode.A) && player.GetComponent<PlayerScript>().x < WIDTH - 1 && !GameManager.lost)
        {

            gridManager.GetComponent<GridMaker>().DeselectEverything();

            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            playerGrid[x + 1, y] = player;
            playerGrid[x, y] = null;
            player.transform.localPosition = new Vector2(WIDTH - (x + 1) - xOffset, HEIGHT - y - yOffset);

            player.GetComponent<PlayerScript>().x++;
            gridManager.GetComponent<GridMaker>().tiles[player.GetComponent<PlayerScript>().x, y].GetComponent<TileScript>().selected = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && player.GetComponent<PlayerScript>().y < HEIGHT - 1 && !GameManager.lost)
        {

            gridManager.GetComponent<GridMaker>().DeselectEverything();

            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            playerGrid[x, y + 1] = player;
            playerGrid[x, y] = null;
            player.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - (y + 1) - yOffset);

            player.GetComponent<PlayerScript>().y++;
            gridManager.GetComponent<GridMaker>().tiles[x, player.GetComponent<PlayerScript>().y].GetComponent<TileScript>().selected = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && player.GetComponent<PlayerScript>().y > 0 && !GameManager.lost)
        {

            gridManager.GetComponent<GridMaker>().DeselectEverything();

            int x = player.GetComponent<PlayerScript>().x;
            int y = player.GetComponent<PlayerScript>().y;

            playerGrid[x, y - 1] = player;
            playerGrid[x, y] = null;
            player.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - (y - 1) - yOffset);

            player.GetComponent<PlayerScript>().y--;
            gridManager.GetComponent<GridMaker>().tiles[x, player.GetComponent<PlayerScript>().y].GetComponent<TileScript>().selected = true;
        }

        if (Input.GetKeyDown(KeyCode.J) && gridManager.GetComponent<GridMaker>().CanDelete() && !GameManager.lost)
        {
            canDelete = false;
            gridManager.GetComponent<GridMaker>().RemoveSelected();
        }

        if (Input.GetKeyDown(KeyCode.K) && !switching && 
            !gridManager.GetComponent<GridMaker>().tiles[player.GetComponent<PlayerScript>().x,player.GetComponent<PlayerScript>().y].GetComponent<TileScript>().isStone && !GameManager.lost)
        {
            clone = Instantiate(clonePrefab);
            clone.transform.parent = cloneGridHolder.transform;
            clone.transform.localPosition = new Vector2(WIDTH - player.GetComponent<PlayerScript>().x - xOffset, HEIGHT - player.GetComponent<PlayerScript>().y - yOffset);

            cloneGrid[player.GetComponent<PlayerScript>().x, player.GetComponent<PlayerScript>().y] = clone;
            clone.GetComponent<CloneScript>().x = player.GetComponent<PlayerScript>().x;
            clone.GetComponent<CloneScript>().y = player.GetComponent<PlayerScript>().y;

            switchedTile = gridManager.GetComponent<GridMaker>().tiles[player.GetComponent<PlayerScript>().x, player.GetComponent<PlayerScript>().y];
            switching = true;
        }
        else if(Input.GetKeyDown(KeyCode.K) && switching &&
            !gridManager.GetComponent<GridMaker>().tiles[player.GetComponent<PlayerScript>().x, player.GetComponent<PlayerScript>().y].GetComponent<TileScript>().isStone && !GameManager.lost)
        {
            audioSource.clip = switchSound;
            audioSource.Play();
            switchedTile2 = gridManager.GetComponent<GridMaker>().tiles[player.GetComponent<PlayerScript>().x, player.GetComponent<PlayerScript>().y];
            switchHelper = switchedTile.GetComponent<TileScript>().type;
            switchedTile.GetComponent<TileScript>().SetSprite(switchedTile2.GetComponent<TileScript>().type);
            switchedTile2.GetComponent<TileScript>().SetSprite(switchHelper);
            switching = false;
            Destroy(clone);
            gridManager.GetComponent<GridMaker>().DeselectEverything();
            gridManager.GetComponent<GridMaker>().tiles[player.GetComponent<PlayerScript>().x, player.GetComponent<PlayerScript>().y].GetComponent<TileScript>().selected = true;
        }
    }
}

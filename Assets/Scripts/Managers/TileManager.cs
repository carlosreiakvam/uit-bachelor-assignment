using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Singleton;
    const int MAX_TRIES_TO_FIND_EMPTY_TILE = 100;


    [SerializeField] Tilemap forestTilemap;
    [SerializeField] Tilemap treeTilemap;
    [SerializeField] Tilemap caveTilemap;


    private static Dictionary<SpawnEnums, int> mapBoundsOutdoor = new Dictionary<SpawnEnums, int>()
{
    { SpawnEnums.X_MIN, 1 },
    { SpawnEnums.X_MAX, 50 },
    { SpawnEnums.Y_MIN, 1 },
    { SpawnEnums.Y_MAX, 50 },
    { SpawnEnums.X_MIDDLE, 45/2 },
    { SpawnEnums.Y_MIDDLE, 45/2 },

};

    private static Dictionary<SpawnEnums, int> mapBoundsCave = new Dictionary<SpawnEnums, int>()
{
    { SpawnEnums.X_MIN, 95 },
    { SpawnEnums.X_MAX, 140 },
    { SpawnEnums.Y_MIN, 1 },
    { SpawnEnums.Y_MAX, 50 },
    { SpawnEnums.X_MIDDLE, 95/2 },
    { SpawnEnums.Y_MIDDLE, 45/2 },
};



    private void Awake()
    {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);

    }


    /// <summary>
    /// Get a randomly located empty tile within a given environment.
    /// </summary>
    /// <param name="searchRange">How much space in pixels around the given tile should also be empty.</param>
    /// <param name="environment">The given environment</param>
    /// <param name="excludedMidAreaSideLength">If greater that 0 this represents a mid area in the tilemap that will be excluded</param>
    /// <returns></returns>
    public Vector2 GetEmptyTile(int searchRange, EnvironmentEnums environment, int excludedMidAreaSideLength = -1)
    {
        Dictionary<SpawnEnums, int> boundaries;
        List<Tilemap> tilemaps;
        List<Tilemap> outdoorTilemaps = new List<Tilemap> { forestTilemap, treeTilemap };
        List<Tilemap> caveTilemaps = new List<Tilemap> { caveTilemap };

        // Set the correct boundaries and tilemaps based on the environment.
        if (environment == EnvironmentEnums.Outdoor)
        {
            boundaries = mapBoundsOutdoor;
            tilemaps = outdoorTilemaps;
        }
        else
        {
            boundaries = mapBoundsCave;
            tilemaps = caveTilemaps;
        }

        Vector2 emptyTile = Vector2.zero;

        // Try to find an empty tile.
        for (int i = 0; i < MAX_TRIES_TO_FIND_EMPTY_TILE; i++)
        {

            // Generate a random position within the boundaries.
            Vector3Int randomPosition = new Vector3Int(Random.Range(boundaries[SpawnEnums.X_MIN] + searchRange, boundaries[SpawnEnums.X_MAX] - searchRange),
                                                        Random.Range(boundaries[SpawnEnums.Y_MIN] + searchRange, boundaries[SpawnEnums.Y_MAX] - searchRange), 0
                                                        );

            // If an area in the middle should be excluded, check if the random position is within that area and if so, skip this iteration.
            if (excludedMidAreaSideLength != -1)
            {
                Dictionary<SpawnEnums, int> midArea = GetMidAreaFromOutdoor(excludedMidAreaSideLength);
                if (
                    (randomPosition.x >= midArea[SpawnEnums.X_MIN] && randomPosition.x <= midArea[SpawnEnums.X_MAX])
                    && (randomPosition.y >= midArea[SpawnEnums.Y_MIN] && randomPosition.y <= midArea[SpawnEnums.Y_MAX])
                    )
                { continue; }
            }

            // Assume the tile is empty until proven otherwise.
            bool isTileEmpty = true;

            // Check the tile at the random position and its neighboring tiles.
            for (int dx = -searchRange; dx <= searchRange; dx++)
            {
                for (int dy = -searchRange; dy <= searchRange; dy++)
                {
                    Vector3Int position = randomPosition + new Vector3Int(dx, dy, 0);

                    // If a tile is not empty, break out of the loop.
                    if (!IsTileEmptyAt(position, tilemaps))
                    {
                        isTileEmpty = false;
                        break;
                    }
                }

                if (!isTileEmpty) break;
            }

            // If an empty tile was found, convert its position to world coordinates and break the loop.
            if (isTileEmpty)
            {
                emptyTile = tilemaps[0].CellToWorld(randomPosition);
                break;
            }
        }

        return new Vector2(emptyTile.x, emptyTile.y);
    }


    /// <summary>
    /// This method checks if a tile is empty in every tilemap provided.
    /// </summary>
    /// <param name="position"> The position to check </param>
    /// <param name="tilemaps"> The tilemaps that are inspected </param>
    /// <returns>Returns true if tile is empty in all tilemaps provided, false otherwise.</returns>
    private bool IsTileEmptyAt(Vector3Int position, List<Tilemap> tilemaps)
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.GetTile(position) != null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calculates and returns the mid-area bounds of the outdoor environment based on the given side length.
    /// </summary>
    /// <param name="sideLength">The side length for the square mid-area to calculate.</param>
    /// <returns>
    /// A dictionary with keys as SpawnEnums (X_MIN, X_MAX, Y_MIN, Y_MAX) representing the bounds of the mid-area,
    /// and values as integers representing the respective positions within the outdoor environment.
    /// </returns>
    /// <remarks>
    /// The mid-area is calculated as a square with the given side length, centered around the middle point of the outdoor environment.
    /// </remarks>
    private Dictionary<SpawnEnums, int> GetMidAreaFromOutdoor(int sideLength)
    {

        Dictionary<SpawnEnums, int> midArea = new Dictionary<SpawnEnums, int>()
        {
            { SpawnEnums.X_MIN, mapBoundsOutdoor[SpawnEnums.X_MIDDLE] - sideLength },
            { SpawnEnums.X_MAX, mapBoundsOutdoor[SpawnEnums.X_MIDDLE] + sideLength },
            { SpawnEnums.Y_MIN, mapBoundsOutdoor[SpawnEnums.Y_MIDDLE] - sideLength },
            { SpawnEnums.Y_MAX, mapBoundsOutdoor[SpawnEnums.Y_MIDDLE] + sideLength },
        };

        return midArea;
    }



}

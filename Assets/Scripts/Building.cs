using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public bool Plantable = false;
    public BoundsInt bounds;
    public Vector3Int buildingOffset;

    public bool CanBePlaced(Tilemap tilemap)
    {
        foreach (var position in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);

            if (!this.CanBePlacedOnTile(tile, position))
            {
                return false;
            }
        }

        return true;
    }


    public bool CanBePlacedOnTile(TileBase tile, Vector3Int position)
    {
        TileData tileData = MapManager.instance.getTileDataForTile(tile, position);

        if (!tileData.isBuildable || tileData.occupied)
        {
            return false;
        }

        return true;
    }

    public void PlaceInTilemap(Tilemap tilemap)
    {
        MapManager.instance.setOccupied(bounds);
        this.Placed = true;
    }
}

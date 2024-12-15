using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public Tilemap map;

    public List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> tileDataDictionary;
    private Dictionary<Vector3Int, bool> tileOccupancyDirectory;

    private void Awake()
    {
        instance = this;

        tileDataDictionary = new Dictionary<TileBase, TileData>();
        tileOccupancyDirectory = new Dictionary<Vector3Int, bool>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                tileDataDictionary.Add(tile, tileData);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);

            //Debug.Log("buildable: " + MapManager.instance.getTileDataForTile(clickedTile, gridPosition).isBuildable + ", plantable: " + MapManager.instance.getTileDataForTile(clickedTile, gridPosition).isPlantable);
        }
    }

    public TileData getTileDataForTile(TileBase tile, Vector3Int position)
    {
        bool occupied = tileOccupancyDirectory.ContainsKey(position);
        TileData tileData;

        if (tileDataDictionary.ContainsKey(tile))
        {
            tileData = tileDataDictionary[tile];
            tileData.occupied = occupied;
            return tileDataDictionary[tile];
        } else
        {
            tileData = (TileData)ScriptableObject.CreateInstance("TileData");
            tileData.occupied = occupied;
            tileDataDictionary.Add(tile, tileData);
            return tileDataDictionary[tile];
        }   
        
    }

    public void setOccupied(BoundsInt bounds)
    {
        foreach (var position in bounds.allPositionsWithin)
        {
            tileOccupancyDirectory[position] = true;
        }
        
    }


}

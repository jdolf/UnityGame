using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem instance;

    public GridLayout gridLayout;
    public Tilemap floorTilemap;
    public Tilemap indicationTilemap;

    private static Dictionary<TileType, TileBase> indicationTiles = new Dictionary<TileType, TileBase>();

    private Building building;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    private void Awake()
    {
        instance = this;

        // Detect existing sprites from editor and set their floor tiles as occupied
        foreach (Building buildingToMark in GameObject.FindObjectsOfType<Building>())
        {
            Sprite buildingSprite = buildingToMark.GetComponent<SpriteRenderer>().sprite;
            Vector2 pivotOffset = new Vector2(MathF.Round(buildingSprite.pivot.x / buildingSprite.pixelsPerUnit * 2, MidpointRounding.AwayFromZero) / 2, MathF.Round(buildingSprite.pivot.y / buildingSprite.pixelsPerUnit * 2, MidpointRounding.AwayFromZero) / 2);
            Vector3 buildingPosition = buildingToMark.transform.position;
            Vector3 buildingBottomLeftPosition = new Vector3(buildingPosition.x - pivotOffset.x, buildingPosition.y - pivotOffset.y, buildingPosition.z);
            Vector3Int buildingCellPosition = gridLayout.LocalToCell(buildingBottomLeftPosition) + buildingToMark.buildingOffset;
            Debug.Log(buildingPosition.x + " " + pivotOffset.x);
            buildingToMark.bounds.position = buildingCellPosition;
            MapManager.instance.setOccupied(buildingToMark.bounds);
        }
    }

    public void InitializeWithBuilding(GameObject gameObject)
    {
        building = Instantiate(gameObject, Vector3.zero, Quaternion.identity).GetComponent<Building>();
    }

    void Start()
    {
        indicationTiles.Add(TileType.Invalid, Resources.Load<TileBase>("indicator_invalid"));
        indicationTiles.Add(TileType.Valid, Resources.Load<TileBase>("indicator_valid"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!building)
        {
            return;
        }

        if (this.IsMouseOverGameWindow)
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            if (!building.Placed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                if (prevPos != cellPos)
                {

                    building.bounds.position = cellPos;

                    // Adjust Sprite x and y offset by its pivot
                    Sprite buildingSprite = building.GetComponent<SpriteRenderer>().sprite;
                    Vector2 pivotOffset = new Vector2(MathF.Round(buildingSprite.pivot.x / buildingSprite.pixelsPerUnit * 2, MidpointRounding.AwayFromZero) / 2, MathF.Round(buildingSprite.pivot.y / buildingSprite.pixelsPerUnit * 2, MidpointRounding.AwayFromZero) / 2);
                    float xOffset = pivotOffset.x - building.buildingOffset.x;
                    float yOffset = pivotOffset.y - building.buildingOffset.y;
                    building.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0f + xOffset, 0f + yOffset, 0f));

                    // Clear existing indication tiles
                    indicationTilemap.ClearAllTiles();

                    // Make indication tiles
                    foreach (Vector3Int position in building.bounds.allPositionsWithin)
                    {
                        TileBase tile = floorTilemap.GetTile(position);
                        if (building.CanBePlacedOnTile(tile, position))
                        {
                            indicationTilemap.SetTile(position, indicationTiles[TileType.Valid]);
                        } else
                        {
                            indicationTilemap.SetTile(position, indicationTiles[TileType.Invalid]);
                        }
                    }

                    prevPos = cellPos;
                    //FollowBuilding();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (building.CanBePlaced(floorTilemap))
                    {
                        building.PlaceInTilemap(floorTilemap);
                        indicationTilemap.ClearAllTiles();
                    }
                }
            }
        }
    }

    bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }

    public enum TileType
    {
        Valid,
        Invalid
    }
}

using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class MapComponent : MonoBehaviour
{
    [SerializeField]
    protected BlockComponent _blockPrefab;
    [SerializeField]
    protected BlockComponent _fuelBlock;
    [SerializeField]
    protected List<BlockComponent> _moneyBlocks;
    [SerializeField]
    protected BlockComponent _impassableBlockPrefab;

    [SerializeField]
    protected GameObject _exitPrefab;

    protected int _width = 30;
    protected int _height = 30;

    protected BlockComponent[,] _blocks;

    public bool IsOnMap(int x, int y) => (x >= 0) && (y >= 0) && (x < _blocks.GetLength(0) ) && (y < _blocks.GetLength(1));

    private void Awake()
    {
        GenerateMap();
    }

    protected void GenerateMap()
    {
        // Fill with blocks
        _blocks = new BlockComponent[_width, _height];
        Enumerable.Range(0, _height)
            .ToList()
            .ForEach(y => {
                Enumerable.Range(0, _width)
                    .ToList()
                    .ForEach(x => {
                        var name = $"Block: ({x} ,{y})";
                        var position = new Vector3(x, y, transform.position.z);
                        var isEdge = (x == 0) || (y == 0) || (x == _width - 1) || (y == _height - 1);
                        var prefab = isEdge ? _impassableBlockPrefab : _blockPrefab;
                        if(prefab != _impassableBlockPrefab)
                        {
                            prefab = (Random.Range(0, 10) < 1) ? _moneyBlocks[Random.Range(0, _moneyBlocks.Count)] : prefab;
                        }
                        if (prefab != _impassableBlockPrefab)
                        {
                            prefab = (Random.Range(0, 75) < 1) ? _fuelBlock : prefab;
                        }
                        var block = Instantiate(prefab, position, Quaternion.identity);
                        block.name = name;
                        _blocks[x, y] = block;
                    });
            });

        // Spawn Player
        var spawn = new Vector2Int(
            Random.Range((int)(0.15f * _width), (int)(0.85f * _width)),
            Random.Range((int)(0.15f * _height), (int)(0.85f * _height))
        );

        DoForEachInRange(spawn, 2, (block) => Destroy(block.gameObject));

        var player = FindObjectOfType<PlayerControllerComponent>();
        player.transform.position = new Vector3(spawn.x, spawn.y, transform.position.z);
        var elevator = GameObject.FindGameObjectWithTag("Elevator");
        elevator.transform.position = new Vector3(spawn.x, spawn.y, transform.position.z);

        // Generate Exit

        var isX = Random.Range(0, 2) == 0;
        var isMaximum = Random.Range(0, 2) == 0;
        var position = Random.Range(0, isX ? _width : _height);
        var x = isX ? position : (isMaximum ? _width - 1 : 0);
        var y = !isX ? position : (isMaximum ? _height - 1 : 0);

        DoForEachInRange(new Vector2Int(x, y), 1, (block) =>
        {
            Destroy(block.gameObject);
            Instantiate(_exitPrefab, block.gameObject.transform.position, Quaternion.identity);
        });
    }

    protected void DoForEachInRange(Vector2Int center, int range, Action<BlockComponent> action)
    {
        for(var yOffset = -range; yOffset <= range; yOffset++)
        {
            for (var xOffset = -range; xOffset <= range; xOffset++)
            {
                var position = center + new Vector2Int(xOffset, yOffset);
                if(!IsOnMap(position.x, position.y)) { continue; }
                action(_blocks[position.x, position.y]);
            }
        }
    }
}
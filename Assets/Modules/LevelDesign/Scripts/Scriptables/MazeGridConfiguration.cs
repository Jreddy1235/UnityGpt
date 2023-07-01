using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Grid Configuration", fileName = "MazeGridConfiguration")]
    public class MazeGridConfiguration : ScriptableObject
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2Int offset;
        [SerializeField] private int iterations;
        [SerializeField] private MockPathInfo mockPathInfo;
        [SerializeField] private ShortcutsInfo shortcutsInfo;
        [SerializeField] private Character[] characters;
        [SerializeField] private CharacterMapping[] characterMapping;
        [SerializeField] private Collectable[] collectables;
        [SerializeField] private Obstacle[] obstacles;
        [SerializeField] private BaseBoardElement[] extraElements;

        public Vector2Int GridSize => gridSize;
        public Vector2Int Offset => offset;
        public int Iterations => iterations;
        public Character[] Characters => characters;
        public Collectable[] Collectables => collectables;
        public Obstacle[] Obstacles => obstacles;
        public BaseBoardElement[] ExtraElements => extraElements;
        public MockPathInfo MockPathInfo => mockPathInfo;
        public ShortcutsInfo ShortcutsInfo => shortcutsInfo;

        public int GetMappedCharacter(int collectableId)
        {
            foreach (var mapping in characterMapping)
            {
                if (mapping.CollectableId == collectableId)
                    return mapping.CharacterId;
            }

            return -1;
        }

        public List<BaseBoardElement> BoardElements => characters.Concat<BaseBoardElement>(collectables)
            .Concat(obstacles).Concat(characters).Concat(extraElements).ToList();
    }

    [Serializable]
    public class Character : BaseBoardElement
    {
    }

    [Serializable]
    public class CharacterMapping
    {
        [SerializeField] private int collectableId;
        [SerializeField] private int characterId;

        public int CollectableId => collectableId;
        public int CharacterId => characterId;
    }

    [Serializable]
    public class Collectable : BaseBoardElement
    {
        [SerializeField] private Range pathLength;

        public Range PathLength => pathLength;
    }

    [Serializable]
    public class Obstacle : BaseBoardElement
    {
        [SerializeField] [Range(0, 1)] private float priority;
        [SerializeField] private int weight;
        [SerializeField] private int[] associateElementIds;

        public float Priority => priority;
        public int Weight => weight;
        public int[] AssociateElementIds => associateElementIds;
    }

    [Serializable]
    public class Range
    {
        [SerializeField] private int min;
        [SerializeField] private int max;

        public int Min => min;
        public int Max => max;
    }

    [Serializable]
    public class BaseBoardElement
    {
        [SerializeField] private string name;
        [SerializeField] private int id;
        [SerializeField] private Range amount;
        [SerializeField] private string categoryId;

        public string Name => name;
        public int Id => id;
        public Range Amount => amount;
        public string CategoryId => categoryId;
    }

    [Serializable]
    public class MockPathInfo
    {
        [SerializeField] [Range(0, 1)] private float frequency;
        [SerializeField] [Range(0, 1)] private float exitChances;

        public float Frequency => frequency;
        public float ExitChances => exitChances;
    }
    
    [Serializable]
    public class ShortcutsInfo
    {
        [SerializeField] [Range(0, 1)] private float frequency;

        public float Frequency => frequency;
    }
}
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
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private bool avoidShortcuts;
        [SerializeField] private Character[] characters;
        [SerializeField] private CharacterMapping[] characterMapping;
        [SerializeField] private Collectable[] collectables;
        [SerializeField] private Obstacle[] obstacles;
        [SerializeField] private BaseBoardElement[] extraElements;

        public Vector2Int GridSize => gridSize;
        public Vector2Int Offset => offset;
        public Difficulty Difficulty => difficulty;
        public Character[] Characters => characters;
        public Collectable[] Collectables => collectables;
        public Obstacle[] Obstacles => obstacles;
        public BaseBoardElement[] ExtraElements => extraElements;
        public bool AvoidShortcuts => avoidShortcuts;
        
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

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
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
        [SerializeField] private int weight;

        public int Weight => weight;
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
}
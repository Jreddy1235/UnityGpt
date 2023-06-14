using System;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Grid Configuration", fileName = "MazeGridConfiguration")]
    public class MazeGridConfiguration : ScriptableObject
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2Int offset;
        [SerializeField] private Difficulty difficulty;
        [SerializeField] private Character[] characters;
        [SerializeField] private Collectable[] collectables;
        [SerializeField] private Obstacle[] obstacles;

        public Vector2Int GridSize => gridSize;
        public Vector2Int Offset => offset;
        public Difficulty Difficulty => difficulty;
        public Character[] Characters => characters;
        public Collectable[] Collectables => collectables;
        public Obstacle[] Obstacles => obstacles;
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    [Serializable]
    public class Character
    {
        [SerializeField] private string name;
        [SerializeField] private int id;
        [SerializeField] private Vector2Int amount;

        public string Name => name;
        public int Id => id;
        public Vector2Int Amount => amount;
    }

    [Serializable]
    public class Collectable
    {
        [SerializeField] private string name;
        [SerializeField] private int id;
        [SerializeField] private Vector2Int amount;
        [SerializeField] private Vector2Int pathLength;

        public string Name => name;
        public int Id => id;
        public Vector2Int Amount => amount;
        public Vector2Int PathLength => pathLength;
    }

    [Serializable]
    public class Obstacle
    {
        [SerializeField] private string name;
        [SerializeField] private int id;
        [SerializeField] private Vector2Int amount;
        [SerializeField] private int weight;

        public string Name => name;
        public int Id => id;
        public Vector2Int Amount => amount;
        public int Weight => weight;
    }
}
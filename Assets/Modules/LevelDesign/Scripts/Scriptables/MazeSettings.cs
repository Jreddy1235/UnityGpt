using System;
using System.Linq;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Settings", fileName = "MazeSettings")]
    public class MazeSettings : ScriptableObject
    {
        [SerializeField] private SpriteMapping[] spritesMapping;

        public SpriteMapping GetSpriteMapping(int id)
        {
            return spritesMapping.FirstOrDefault(t => t.Id == id);
        }

        [Serializable]
        public class SpriteMapping
        {
            [SerializeField] private int id;
            [SerializeField] private Sprite sprite;
            [SerializeField] private Color color = Color.white;

            public int Id => id;
            public Sprite Sprite => sprite;
            public Color Color => color;
        }
    }
}
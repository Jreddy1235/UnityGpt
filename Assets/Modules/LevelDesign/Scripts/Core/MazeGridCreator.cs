using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Modules.LevelDesign.Scripts.Core
{
    public class MazeGridCreator : MonoBehaviour
    {
        private static MazeGridCreator _instance;

        [InitializeOnLoad]
        private class TickHandler
        {
            static TickHandler()
            {
                EditorApplication.update += Tick;
            }
        }

        private int[,] _grid = new int[,]
        {
            {-2, -2, -2},
            {-2, -2, -2},
            {-2, -2, -2},
        };

        [SerializeField] private BehaviorTree tree;

        private bool _isEnded;

        [Button]
        private void CreateGrid()
        {
            _instance = this;
            _isEnded = false;
            tree = new BehaviorTreeBuilder(gameObject)
                .Sequence()
                .Condition("Custom Condition", () => true)
                .Do("Custom Action", () =>
                {
                    Debug.Log("Hi");
                    _isEnded = true;
                    return TaskStatus.Success;
                })
                .End()
                .Build();
        }

        private static void Tick()
        {
            if (_instance != null && _instance.tree?.Root != null && !_instance._isEnded)
                _instance.tree.Tick();
        }
    }
}
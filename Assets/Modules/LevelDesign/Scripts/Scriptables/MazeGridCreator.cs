using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using NaughtyAttributes;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Grid Creator", fileName = "MazeGridCreator")]
    public class MazeGridCreator : ScriptableObject
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

        private GameObject _gameObject;
        private bool _isEnded;

        [Button]
        private void CreateGrid()
        {
            CreateGrid((_, _, _) => Debug.Log("Hi"));
        }

        public void CreateGrid(Action<string, int, int> onComplete)
        {
            _instance = this;
            _isEnded = false;
            SpawnGameObject();
            tree = new BehaviorTreeBuilder(_gameObject)
                .Sequence()
                .Condition("Custom Condition", () => true)
                .Do("Custom Action", () =>
                {
                    _isEnded = true;
                    onComplete?.Invoke(GetGridString(), _grid.GetLength(1), _grid.GetLength(0));
                    Observable.ReturnUnit().DelayFrame(1)
                        .Subscribe(_ => DestroyGameObject()).AddTo(_gameObject);

                    return TaskStatus.Success;
                })
                .End()
                .Build();
        }

        private string GetGridString()
        {
            var gridStr = "";
            for (var i = 0; i < _grid.GetLength(1); i++)
            {
                for (var j = 0; j < _grid.GetLength(0); j++)
                {
                    gridStr += _grid[j, i] + ",";
                }
            }

            return gridStr[..^1];
        }

        private void SpawnGameObject()
        {
            DestroyGameObject();
            _gameObject = new GameObject
            {
                name = GetType().Name,
                hideFlags = HideFlags.HideInHierarchy
            };
        }

        private void DestroyGameObject()
        {
            if (_gameObject != null)
                DestroyImmediate(_gameObject);
        }

        private static void Tick()
        {
            if (_instance != null && _instance.tree?.Root != null && !_instance._isEnded)
                _instance.tree.Tick();
        }
    }
}
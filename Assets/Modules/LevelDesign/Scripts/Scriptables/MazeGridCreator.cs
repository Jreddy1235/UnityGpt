using System;
using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using JetBrains.Annotations;
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

        [SerializeField] private BehaviorTree tree;
        [SerializeField] private MazeTreeInfo treeNodes;

        private GameObject _gameObject;
        private bool _isEnded;

        private int[,] _grid =
        {
            {-2, -2, -2},
            {-2, -2, -2},
            {-2, -2, -2},
        };

        [UsedImplicitly]
        [Button]
        private void CreateGrid()
        {
            CreateGrid((_, _, _) => Debug.Log("Grid Created"));
        }

        public void CreateGrid(Action<string, int, int> onComplete)
        {
            _instance = this;
            _isEnded = false;
            SpawnGameObject();
            //var nodes = treeNodes.GetNodes();
            tree = new BehaviorTreeBuilder(_gameObject)
                .Sequence()
                .AddNode(treeNodes.GetNodes())
                .Do("Custom Action", () =>
                {
                    _isEnded = true;
                    onComplete?.Invoke(GetGridString(), _grid.GetLength(0), _grid.GetLength(1));
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
            _gameObject.AddComponent<MazeGridController>();
        }

        private bool FilterType(Type type)
        {
            return type.IsSubclassOf(typeof(MazeBaseAction));
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
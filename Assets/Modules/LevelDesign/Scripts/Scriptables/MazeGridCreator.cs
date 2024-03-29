using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using JetBrains.Annotations;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Grid Creator", fileName = "MazeGridCreator")]
    public class MazeGridCreator : ScriptableObject
    {
        private static MazeGridCreator _instance;

        [SerializeField] private MazeGridConfiguration configuration;
        [SerializeField] private BehaviorTree tree;
        [SerializeField] private MazeTreeInfo[] treeNodes;

        public MazeGridConfiguration Configuration => configuration;

        private GameObject _gameObject;
        private bool _isEnded;

        private static void Tick()
        {
            if (_instance != null && _instance.tree?.Root != null && !_instance._isEnded)
                _instance.tree.Tick();
        }

        public void CreateGrid(Action<string, int, int> onComplete)
        {
            _instance = this;
            _isEnded = false;
            SpawnGameObject();

            TaskStatus OnGridCreationDone()
            {
                _isEnded = true;
                var grid = _gameObject.GetComponent<MazeGridController>().Grid.ToIntArray();
                onComplete?.Invoke(GetGridString(grid), grid.GetLength(0), grid.GetLength(1));
                Observable.ReturnUnit()
                    .DelayFrame(1)
                    .Subscribe(_ => DestroyGameObject())
                    .AddTo(_gameObject);

                return TaskStatus.Success;
            }

            var treeBuilder = new BehaviorTreeBuilder(_gameObject).Sequence();
            foreach (var treeNode in treeNodes)
            {
                treeBuilder.AddNode(treeNode.GetNodes(_gameObject));
            }

            tree = treeBuilder.Do(OnGridCreationDone).End().Build();
        }

        [Button]
        public void DoReset()
        {
            try
            {
                tree?.Reset();
                tree = null;
            }
            catch
            {
                // ignored
            }
        }

        [UsedImplicitly]
        [Button]
        private void CreateGrid()
        {
            CreateGrid((_, _, _) => Debug.Log("Grid Created"));
        }

        private string GetGridString(int[,] grid)
        {
            var gridStr = "";
            for (var i = 0; i < grid.GetLength(1); i++)
            {
                for (var j = 0; j < grid.GetLength(0); j++)
                {
                    gridStr += grid[j, i] + ",";
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

            var gridController = _gameObject.AddComponent<MazeGridController>();
            gridController.SetData(configuration);
        }

        private void DestroyGameObject()
        {
            if (_gameObject != null)
                DestroyImmediate(_gameObject);
        }

#if UNITY_EDITOR
        [InitializeOnLoad]
        private class TickHandler
        {
            static TickHandler()
            {
                EditorApplication.update += Tick;
            }
        }
#endif
    }
}
using System;
using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using TypeReferences;
using UnityEngine;

namespace UnityGPT
{
    [Serializable]
    public class MazeTreeInfo
    {
        [Inherits(typeof(ITask))] [SerializeField]
        private TypeReference parentNode;

        [Inherits(typeof(MazeBaseRule), ExcludeTypes = new[] {typeof(MazeBasePathRule)})] [SerializeField]
        private TypeReference actionRule;

        [SerializeField] private MazeTreeInfo[] childNodes;

        public ITask GetNodes(GameObject owner)
        {
            if (parentNode == null) return null;

            ITask nodes;
            var type = parentNode.Type;
            if (type.IsSubclassOf(typeof(CompositeBase)))
            {
                var parent = (CompositeBase) Activator.CreateInstance(type);
                foreach (var child in childNodes)
                {
                    parent.AddChild(child.GetNodes(owner));
                }

                nodes = parent;
            }
            else
            {
                nodes = (ITask) Activator.CreateInstance(type);
            }

            if (nodes is MazeBaseAction action && actionRule.Type != null)
                action.ActionRule = (MazeBaseRule) Activator.CreateInstance(actionRule.Type);
            
            nodes.Owner = owner;
            return nodes;
        }
    }
}
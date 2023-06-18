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

            nodes.Owner = owner;
            return nodes;
        }
    }
}
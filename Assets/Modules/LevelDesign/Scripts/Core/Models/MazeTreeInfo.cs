using System;
using CleverCrow.Fluid.BTs.Tasks;
using TypeReferences;
using UnityEngine;

namespace UnityGPT
{
    [Serializable]
    public class MazeTreeInfo
    {
        [Inherits(typeof(ITask))] [SerializeField]
        private TypeReference action;

        [Inherits(typeof(MazeBaseRule))] [SerializeField]
        private TypeReference[] rules;

        public ITask GetNodes(GameObject owner)
        {
            if (action == null) return null;

            var nodes = (ITask) Activator.CreateInstance(action.Type);

            if (nodes is MazeBaseAction baseAction && rules?.Length > 0)
            {
                baseAction.Rules = new MazeBaseRule[rules.Length];
                var i = 0;
                foreach (var rule in rules)
                {
                    baseAction.Rules[i++] = (MazeBaseRule) Activator.CreateInstance(rule.Type);
                }
            }

            nodes.Owner = owner;
            return nodes;
        }
    }
}
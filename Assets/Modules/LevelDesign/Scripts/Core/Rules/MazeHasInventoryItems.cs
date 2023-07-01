using UnityEngine;

namespace UnityGPT
{
    public class MazeHasInventoryItems : MazeBaseRule
    {
        private int _inventoryItems;

        protected override void OnInit()
        {
            base.OnInit();
            _inventoryItems = Random.Range(Configuration.ReqInventoryItems.Min, Configuration.ReqInventoryItems.Max);
        }

        public override bool Apply(MazeTile tile)
        {
            if (_inventoryItems <= 0) return false;
            
            _inventoryItems--;
            return true;
        }
    }
}
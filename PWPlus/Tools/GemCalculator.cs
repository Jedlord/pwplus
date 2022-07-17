namespace PWPlus.Tools
{
    public static class GemCalculator
    {
        public static int GetGemAmount(World.BlockType blockType)
        {
            if(ConfigData.IsFish(blockType))
            {
                return ConfigData.GetFishRecycleValueForFishRecycler(blockType);
            }
            if(ConfigData.IsBlockMiningGemstone(blockType))
            {
                return ConfigData.GetGemstoneRecycleValueForMiningGemstoneRecycler(blockType);
            }
            if(ConfigData.IsConsumableTreasurePouch(blockType))
            {
                return ConfigData.GetTreasurePouchRewardAmount(blockType);
            }

            return 0;
        }
    }
}

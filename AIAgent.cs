using CustomConstants;

namespace AIAgent
{
    public class AIAgent
    {

    }

    public class AIAction
    {
        private TileTypes tileType { get; }
        private int x1 { get; }
        private int y1 { get; }

        private int x2 { get; }
        private int y2 { get; }

        public AIAction(TileTypes tileType, int x1, int y1, int x2, int y2)
        {
            this.tileType = tileType;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
    }
}

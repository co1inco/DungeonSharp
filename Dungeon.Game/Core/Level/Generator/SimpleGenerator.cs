using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Level.Elements.Tiles;
using Dungeon.Game.Core.Level.Utils;
using Dungeon.Game.Core.Systems;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Level.Generator;

public class SimpleGenerator : IGenerator
{
    public ILevel Level(DesignLabel label, LevelSize size)
    {
        return new SimpleLevel();
    }

    public class SimpleLevel : ILevel
    {


        public Tile[][] Layout { get; private set;  } = [];

        public Maybe<Tile> TileAt(Coordinate coordinate)
        {
            if (Layout.Length < coordinate.Y && coordinate.Y >= 0)
            {
                if (Layout[coordinate.Y].Length < coordinate.X && coordinate.X >= 0)
                {
                    return Layout[coordinate.Y][coordinate.X];
                } 
            }
            return Maybe<Tile>.None;
        }
        
        public Tile StartTile { get => Layout[5][5]; set {} }
        
        public IEnumerable<Tile> EndTiles()
        {
            return [];
        }
        
        public void OnFirstLoad(Action callback)
        {
             
        }
        
        public void OnLoad()
        {
            var path = "Assets/dungeon/default/floor/floor_1.png";
            
            Layout = new Tile[10][];

            for (int i = 0; i < 10; i++)
            {
                Layout[i] = new Tile[10];
                for (int j = 0; j < 10; j++)
                {
                    Layout[i][j] = new FloorTile(path, new Vector2(j, i), DesignLabel.Default);
                }
            }

        }

        public int GetNodeCount()
        {
            return Layout[0].Length * Layout.Length;
        }

        public void AddTile(Tile tile)
        {
            throw new NotImplementedException();
        }

        public void RemoveTile(Tile tile)
        {
            throw new NotImplementedException();
        }
        

        public void AddFloorTile()
        {
            throw new NotImplementedException();
        }

        public void AddWallTile()
        {
            throw new NotImplementedException();
        }

        public void AddHoleTile()
        {
            throw new NotImplementedException();
        }
    }
}
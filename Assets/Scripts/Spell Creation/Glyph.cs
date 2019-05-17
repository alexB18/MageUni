using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Glyph
{
    public string name = "";
    public class Connection
    {
        private Vector2 coordinates;
        private Connection connectedEnd;

        public Vector2 Coordinates { get => coordinates; set => coordinates = value; }
        public void Connect(Connection newConnection) => connectedEnd = newConnection;
        public Connection getConnectedEnd() => connectedEnd;
        
        public Connection(float x, float y)
        {
            coordinates.Set(x, y);
        }

        public Connection(Vector2 vec)
        {
            coordinates = vec;
        }

        public bool IsNull() => Coordinates == null;
        public bool Connected() => connectedEnd == null;
    }

    public class ConnectionMap
    {
        private readonly List<List<bool>> map;

        public ConnectionMap(bool[][] newMap)
        {
            map = new List<List<bool>>();
            foreach(bool[] row in newMap)
                map.Add(new List<bool>(row));
        }
        public ConnectionMap(List<bool[]> newMap)
        {
            map = new List<List<bool>>();
            foreach (bool[] row in newMap)
                map.Add(new List<bool>(row));
        }
        public ConnectionMap(List<List<bool>> newMap)
        {
            map = new List<List<bool>>();
            map = newMap;
        }

        public bool Connected(int connection1, int connection2) => map[connection1][connection2];
    }
    
    private readonly SpellComponent component;
    private readonly string glyphSprite;

    private readonly List<Connection> connections;
    private ConnectionMap connectionMap;


    public SpellComponent Component => component;
    public string GlyphSprite => glyphSprite;
    public bool IsShape => component.IsShape;

    public List<Connection> Connections => connections;
    public bool Connected(int index1, int index2) => connectionMap.Connected(index1, index2);

    public Glyph(SpellComponent spellComponent, string spriteResourceName, List<Connection> newConnections, ConnectionMap map)
    {
        // Initialise ReadOnly fields
        component = spellComponent;
        glyphSprite = spriteResourceName;

        connections = newConnections;
        connectionMap = map;
        name = spriteResourceName;
    }

}

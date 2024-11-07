using System;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Netrogue
{
    [Serializable] // Ensure this class is serializable
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position() { }

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector2() => new Vector2(X, Y);
        public static Position FromVector2(Vector2 vector) => new Position(vector.X, vector.Y);
    }
}

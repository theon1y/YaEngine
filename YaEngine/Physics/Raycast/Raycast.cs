using System.Numerics;

namespace YaEngine.Physics
{
    public struct Raycast
    {
        public bool IsHit;
        public Vector3 From;
        public Vector3 To;
        public Vector3 Hit;
        public Vector3 Normal;

        public override string ToString()
        {
            return $"Raycast(IsHit: {IsHit}; From: {From}; To: {To};\nHit: {Hit}; Normal: {Normal}";
        }
    }
}
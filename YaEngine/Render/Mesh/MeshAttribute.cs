namespace YaEngine.Render
{
    public struct MeshAttribute
    {
        public readonly string Name;
        public readonly int Size;
        public int Offset;
        
        public MeshAttribute(string name, int size)
        {
            Name = name;
            Size = size;
            Offset = 0;
        }
    }
}
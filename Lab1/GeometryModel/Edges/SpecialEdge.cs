namespace Lab1.GeometryModel.Edges
{
    public abstract class SpecialEdge : Edge
    {
        protected SpecialEdge(Vertex start, Vertex end) : base(start, end) { }
        public override bool IsBasic { get => false; }

    }
}

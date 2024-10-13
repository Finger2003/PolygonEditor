namespace Lab1.Visitors
{
    public interface IEdgeVisitable
    {
        void Accept(IEdgeVisitor visitor);
    }
}

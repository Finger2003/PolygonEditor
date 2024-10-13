namespace Lab1
{
    public interface IEdgeVisitable
    {
        void Accept(IEdgeVisitor visitor);
    }
}

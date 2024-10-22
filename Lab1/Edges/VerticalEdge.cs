using Lab1.Exceptions;
using Lab1.Visitors;

namespace Lab1.Edges
{
    public class VerticalEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsVertical { get => true; }

        public VerticalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.SetPosition(Start.X, End.Y);
            End.WasMoved = true;
            End.InvokeStartPositionChanged();
            SetButtonPosition();
        }

        public override void StartChanged()
        {
            //SetButtonPosition();
            //Start.WasChecked = true;

            if (Start.Position.X == End.Position.X)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }
            if (End.WasMoved && Start.WasMoved)
                throw new VertexAlreadyMovedException();


            //End.Position = new Point(Start.Position.X, End.Position.Y);
            End.SetPosition(Start.X, End.Y);
            End.WasMoved = true;
            End.InvokeStartPositionChanged();


            //if (!End.WasMoved /*Start.WasMoved*/)
            //{
            //    End.Position = new Point(Start.Position.X, End.Position.Y);
            //    End.WasMoved = true;
            //    End.NeighbourPositionChanged();
            //}
            //else if (!Start.WasMoved/*End.WasMoved*/)
            //{
            //    Start.Position = new Point(End.Position.X, Start.Position.Y);
            //    Start.WasMoved = true;
            //    //Start.NeighbourPositionChanged();
            //}

            //End.Position = new Point(Start.Position.X, End.Position.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void EndChanged()
        {
            if (Start.Position.X == End.Position.X)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }
            if (End.WasMoved && Start.WasMoved)
                throw new VertexAlreadyMovedException();


            //Start.Position = new Point(End.Position.X, Start.Position.Y);
            Start.SetPosition(End.X, Start.Y);
            Start.WasMoved = true;
            Start.InvokeEndPositionChanged();
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}

﻿using Lab1.Exceptions;
using Lab1.Visitors;

namespace Lab1.Edges
{
    public class VerticalEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsVertical { get => true; }

        public VerticalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.Position = new Point(Start.Position.X, End.Position.Y);
            End.WasMoved = true;
            End.NeighbourPositionChanged();
            SetButtonPosition();
        }

        public override void StartChanged()
        {
            //SetButtonPosition();
            Start.WasChecked = true;

            if (Start.Position.X == End.Position.X)
            {
                if (!End.WasChecked)
                    End.NeighbourPositionChanged();
                return;
            }
            if (End.WasMoved && Start.WasMoved)
                throw new VertexAlreadyMovedException();



            if (!End.WasMoved /*Start.WasMoved*/)
            {
                End.Position = new Point(Start.Position.X, End.Position.Y);
                End.WasMoved = true;
                End.NeighbourPositionChanged();
            }
            else if (!Start.WasMoved/*End.WasMoved*/)
            {
                Start.Position = new Point(End.Position.X, Start.Position.Y);
                Start.WasMoved = true;
                //Start.NeighbourPositionChanged();
            }

            //End.Position = new Point(Start.Position.X, End.Position.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}

﻿using Lab1.Exceptions;
using Lab1.Visitors;

namespace Lab1.Edges
{
    public class HorizontalEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsHorizontal { get => true; }

        public HorizontalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.Position = new Point(End.Position.X, Start.Position.Y);
            End.WasMoved = true;
            End.InvokeStartPositionChanged();
            SetButtonPosition();
        }

        public override void StartChanged()
        {
            //SetButtonPosition();
            //Start.WasChecked = true;

            if (Start.Position.Y == End.Position.Y)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }

            if (End.WasMoved && Start.WasMoved)
                throw new VertexAlreadyMovedException();


            End.Position = new Point(End.Position.X, Start.Position.Y);
            End.WasMoved = true;
            End.InvokeStartPositionChanged();

            //if (!End.WasMoved /*Start.WasMoved*/)
            //{
            //    End.Position = new Point(End.Position.X, Start.Position.Y);
            //    End.WasMoved = true;
            //    End.NeighbourPositionChanged();
            //}
            //else if (!Start.WasMoved /*End.WasMoved*/)
            //{
            //    Start.Position = new Point(Start.Position.X, End.Position.Y);
            //    Start.WasMoved = true;
            //    //Start.NeighbourPositionChanged();
            //}

            //End.Position = new Point(End.Position.X, Start.Position.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void EndChanged()
        {
            if (Start.Position.Y == End.Position.Y)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }

            if (End.WasMoved && Start.WasMoved)
                throw new VertexAlreadyMovedException();


            Start.Position = new Point(Start.Position.X, End.Position.Y);
            Start.WasMoved = true;
            Start.InvokeStartPositionChanged();
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}

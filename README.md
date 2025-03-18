# PolygonEdtor #
A simple Windows Forms application .NET 8.0 that provides an AutoCAD-like interface for drawing and editing polygons. The app supports creating closed polygons with customizable constraints and advanced drawing options for both straight lines and Bézier curves.


## Features ##
- ### Polygon Creation ###
    - Click on the canvas to place vertices and create a closed polygon.

- ### Edge Interactions ###
    - **Add Vertex:** Click on an edge (or on one of the control lines of a Bézier edge) to add a new vertex. This action splits the selected edge (if it is Bézier curve then it is chaged into simple edges) into two new edges and removes any existing constraints on that edge
    
    - **Modify Constraints:** Click on an edge to toggle constraints such as:
        - Constant Length
        - Vertical Orientation
        - Horizontal Orientation
        - Convert to Bézier Curve (Degree 3)

- ### Bézier Curve Editing ###
    - When an edge is converted into a Bézier curve, it is drawn using an implemented incremental algorithm that leverages the derivatives of the Bézier curve formula.

    - **Vertex Continuity:** At a vertex that is the endpoint of a Bézier curve, set the desired continuity:
        - G0 (Positional Continuity)
        - G1 (Tangent Continuity)
        - C1 (Parametric Continuity)

    - The default continuity when creating a Bézier curve is C1 unless the neighboring edge is also a Bézier curve with a pre-set different continuity.

- ### Vertex Removal ###
    - Right-click on a vertex to remove it from the polygon.
    
    - When fewer than three vertices remain, the canvas is automatically cleared.

- ### Scene Management ###
    - Clear the entire scene via the butto in the top right corner

- ### Straight Line Rendering ###
    - Choose the rendering algorithm via radio buttons in the top left corner:
        - The built-in .NET method.
        - A custom-implemented Bresenham algorithm.

- ### Example scene ###
    - When the app starts, an **example scene** is automatically loaded, showcasing a sample polygon with different constraints and Bézier curves to demonstrate the app's capabilities.
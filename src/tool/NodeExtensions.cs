using Godot;
using Godot.Collections;

public static class NodeExtensions
{
    public static Array<Node> GetAllChildren(this Node node) {
        var allChildren = new Array<Node>();
        foreach (var child in node.GetChildren()) {
            if (child.GetChildCount() > 0) {
                allChildren.AddRange(child.GetAllChildren());
            }
            else {
                allChildren.Add(child);
            }
        }

        return allChildren;
    }

    // All nodes
    public static void ChangeOwner(this Node node, Node parent) {
        if (node.GetParent() != null) {
            node.GetParent().RemoveChild(node);
        }

        parent.AddChild(node);
        node.Owner = parent;
    }

    // Control Nodes
    public static void ChangeOwner(this Control node, Node parent, bool copyTransform) {
        var globalPos = node.GlobalPosition;

        node.ChangeOwner(parent);

        if (copyTransform) {
            node.GlobalPosition = globalPos;
        }
    }
}
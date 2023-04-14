using Godot;
using System;
using System.Collections.Generic;

public partial class main : Node2D
{
    PackedScene BoidScene = GD.Load<PackedScene>("res://scenes/Boid.tscn");
    PackedScene BoidScene2 = GD.Load<PackedScene>("res://scenes/Boid2.tscn");
    PackedScene PredScene = GD.Load<PackedScene>("res://scenes/Pred.tscn");
	List<Node> boids = new List<Node>(3);

    public override void _Ready()
	{
		boids.Add(BoidScene.Instantiate());
		boids.Add(BoidScene2.Instantiate());
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				_CreateBoid();
			}

			else if (mouseEvent.ButtonIndex == MouseButton.Right)
				_CreatePred();
		}
    }

    private void _CreateBoid()
	{
		Random rnd = new Random();
		int indx = rnd.Next(0, boids.Count);
		Node boid = boids[indx];

		if (indx == 0) boids.Add(BoidScene.Instantiate());
		else if(indx == 1) boids.Add(BoidScene2.Instantiate());
		boids.Remove(boids[indx]);

		AddChild(boid);
		((BoidHelper)boid).GlobalPosition = GetViewport().GetMousePosition();
        ((BoidHelper)boid).SetDirection(new Vector2((rnd.NextSingle() - 0.5f) * 2, (rnd.NextSingle() - 0.5f) * 2));
	}

	private void _CreatePred()
	{
		Random rnd = new Random();
        Node pred = PredScene.Instantiate();
        AddChild(pred);
		((Pred)pred).GlobalPosition = GetViewport().GetMousePosition();
        ((Pred)pred).SetDirection(new Vector2((rnd.NextSingle() - 0.5f) * 2, (rnd.NextSingle() - 0.5f) * 2));
	}

}

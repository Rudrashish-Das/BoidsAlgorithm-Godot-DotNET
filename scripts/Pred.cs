using Godot;
using System;

public partial class Pred : CharacterBody2D
{
	Vector2 _direction = Vector2.Up;
    float _speed = 2;

	public override void _Ready()
	{
		
	}

    public override void _PhysicsProcess(double delta)
    {
        Vector2 screenSize = GetViewportRect().Size;
        if (GlobalPosition.Y < 0)
            GlobalPosition = new Vector2(GlobalPosition.X, screenSize.Y);

        else if (GlobalPosition.Y > screenSize.Y)
            GlobalPosition = new Vector2(GlobalPosition.X, 0);

        if (GlobalPosition.X < 0)
            GlobalPosition = new Vector2(screenSize.X, GlobalPosition.Y);

        else if (GlobalPosition.X > screenSize.X)
            GlobalPosition = new Vector2(0, GlobalPosition.Y);

        KinematicCollision2D collision = this.MoveAndCollide(_direction * _speed);
        if (collision is not null)
        {
            _direction = _ReflectionDirection(collision);
        }
    }

    Vector2 _ReflectionDirection(KinematicCollision2D collision)
    {
        return (collision.GetPosition() - collision.GetNormal()).DirectionTo(this.Position);
    }

    public void SetDirection(Vector2 direction)
	{
		_direction = direction;
	}

	public Vector2 GetDirection()
	{
		return _direction;
	}
}



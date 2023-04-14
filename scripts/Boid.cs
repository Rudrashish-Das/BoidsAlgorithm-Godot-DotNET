using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class Boid : BoidHelper
{
	Random rnd = new Random();

	List<CharacterBody2D> _localEntity = new List<CharacterBody2D>();
	Shape2D collisionNode = null;

	public override void _Ready(){
        //this.MoveAndSlide();
        collisionNode = this.GetNode<CollisionShape2D>("Area2D/CollisionShape2D").Shape;
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 screenSize = GetViewportRect().Size;
		if(GlobalPosition.Y < 0)
			GlobalPosition = new Vector2(GlobalPosition.X, screenSize.Y);

		else if (GlobalPosition.Y > screenSize.Y)
			GlobalPosition = new Vector2(GlobalPosition.X, 0);

		if (GlobalPosition.X < 0)
			GlobalPosition = new Vector2(screenSize.X, GlobalPosition.Y);

		else if (GlobalPosition.X > screenSize.X)
			GlobalPosition = new Vector2(0, GlobalPosition.Y);

		this.Rotation = new Vector2(0,-1).AngleTo(_direction);
		
		KinematicCollision2D collision = this.MoveAndCollide(_direction * _speed);
		
		if (collision is not null)
		{
			_direction = _ReflectionDirection(collision);
		}
		else
		{
			_direction = _FlockDirection();
		}

		if (this.IsInGroup("test"))
		{
			GD.Print(_direction.ToString());
		}
	}

	Vector2 _FlockDirection()
	{
		Vector2 separation = _direction * (rnd.NextSingle() + 1);
		Vector2 heading = _direction * (rnd.NextSingle() + 1);
		Vector2 cohesion = _direction * (rnd.NextSingle() + 1);

		int mult = -1;
		int sepDistance = this._separationDistance;
		int flockmates = 0;

		foreach(var body in _localEntity)
		{
			if (body.IsInGroup(this.GetGroups()[0]))
			{
				flockmates++;
				mult = 1;
				sepDistance = this._separationDistance;
				heading += ((BoidHelper)body).GetDirection();
			}
			else if (body.IsInGroup("pred"))
			{
				//mult = -10000;
				sepDistance = this._separationDistance * 100;
				//heading -= body.Velocity;// * (-100);
			}
			else
				//sepDistance = this._separationDistance * 50;
				continue;

			cohesion += body.Position * mult;

			float distance = this.Position.DistanceTo(body.Position);

			if (distance < _separationDistance)
			{
				separation -= (body.Position - this.Position).Normalized() * (sepDistance / distance * _speed);
			}

			if (flockmates > 0)
			{
				heading /= flockmates;          // Average of heading of all local boids
				cohesion /= flockmates;         // Center of mass of all local boids

				Vector2 centerDirection = this.Position.DirectionTo(cohesion);
				var centerSpeed = _topSpeed * this.Position.DistanceTo(cohesion) / ((CircleShape2D)collisionNode).Radius;
				cohesion = centerDirection * centerSpeed;
			}

		}

		return (
			_direction +
			separation * 0.5f +
			heading * 0.3f +
			cohesion * 0.01f
		).LimitLength(_topSpeed);
	}

	public void _on_area_2d_body_entered(Node2D body)
	{
		if (body == this) return;

		if (body is CharacterBody2D)
			_localEntity.Add((CharacterBody2D)body);
	}

	public void _on_area_2d_body_exited(Node2D body)
	{
		if (body is CharacterBody2D)
			_localEntity.Remove((CharacterBody2D)body);
	}
}

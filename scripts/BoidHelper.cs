using Godot;

public partial class BoidHelper : CharacterBody2D
{
    protected Vector2 _direction = new Vector2(0, -1);
    protected float _speed = 2;
    protected float _topSpeed = 2;
    protected int _separationDistance = 15;

    protected Vector2 _ReflectionDirection(KinematicCollision2D collision)
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
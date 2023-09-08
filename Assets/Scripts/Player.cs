using UnityEngine;
public class Player : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    private Vector3 moveVector;
    private bool isControl = true;
    private void Start()
    {
        stages[currentStage].SetActiveBrick(color, true);
    }
    private void Update()
    {
        if (isControl)
        {
            Control();
        }
    }
    public override void Control()
    {
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, moveSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            ChangeAnim(Constants.RunAnim);
        }
        else if (joystick.Horizontal == 0 || joystick.Vertical == 0)
        {
            ChangeAnim(Constants.IdleAnim);
        }
        rb.MovePosition(rb.position + moveVector);
    }
    public void SetJoystick(FloatingJoystick floatingJoystick)
    {
        this.joystick = floatingJoystick;
    }
    public void Falling()
    {
        isControl = false;
        ChangeAnim(Constants.FallAnim);
        RemoveAllBrick();
        Invoke(nameof(Respawn), 3f);
    }
    public void Respawn()
    {
        ChangeAnim(Constants.RespawnAnim);
        Invoke(nameof(ResetIdle), 1f);
    }
    public void ResetIdle()
    {
        ChangeAnim(Constants.IdleAnim);
        isControl = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.BrickTag) && isControl)
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick.Color == color || brick.Color == BrickColor.Grey)
            {
                base.AddBrick(brick);
                other.GetComponent<Brick>().OnDespawn();
            }
        }
        if (other.CompareTag(Constants.PlayerTag) && isControl)
        {
            if (bricks.Count < other.GetComponent<Character>().GetLengthBrick())
            {
                Falling();
            }
        }
    }

}

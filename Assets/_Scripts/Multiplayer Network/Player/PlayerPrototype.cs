using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using TPSBR;

public struct PlayerStatistics : INetworkStruct
{
    public PlayerRef PlayerRef;
    public short ExtraLives;
    public short Kills;
    public short Deaths;
    public short Score;
    public TickTimer RespawnTimer;
    public byte Position;

    public byte KillsInRow;
    public TickTimer KillsInRowCooldown;
    public byte KillsWithoutDeath;

    public bool IsValid => PlayerRef.IsValid;
    public bool IsAlive { get { return _flags.IsBitSet(0); } set { _flags.SetBit(0, value); } }
    public bool IsEliminated { get { return _flags.IsBitSet(1); } set { _flags.SetBit(1, value); } }

    private byte _flags;
}

public class Player : NetworkBehaviour
{
    private TMP_Text _messages;
    [SerializeField] private SphereBall _prefabBall;
    [SerializeField] private PhysicsBall _prefabPhysxBall;
    [Networked] private TickTimer delay { get; set; }

    [Networked(OnChanged = nameof(OnBallSpawned))] public NetworkBool spawned { get; set; }
    public static void OnBallSpawned(Changed<Player> changed)
    {
        changed.Behaviour.material.color = Color.white;
    }

    private Material _material;
    Material material
    {
        get
        {
            if (_material == null)
                _material = GetComponentInChildren<MeshRenderer>().material;
            return _material;
        }
    }

    public override void Render()
    {
        material.color = Color.Lerp(material.color, Color.blue, Time.deltaTime);
    }

    private NetworkCharacterControllerPrototype _cc;
    private Vector3 _forward;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
        _forward = transform.forward;
    }

    private void Update()
    {
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;

            if (delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabBall,
                      transform.position + _forward,
                      Quaternion.LookRotation(_forward),
                      Object.InputAuthority,
                      (runner, o) =>
                      {
                           // Initialize the Ball before synchronizing it
                           o.GetComponent<SphereBall>().Init();
                      });
                    spawned = !spawned;
                }
                else if ((data.buttons & NetworkInputData.MOUSEBUTTON2) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabPhysxBall,
                      transform.position + _forward,
                      Quaternion.LookRotation(_forward),
                      Object.InputAuthority,
                      (runner, o) =>
                      {
                          // Initialize the PhysicsBall before synchronizing it
                          o.GetComponent<PhysicsBall>().Init(10 * _forward);
                      });
                    spawned = !spawned;
                }
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        if (_messages == null)
            _messages = FindObjectOfType<TMP_Text>();
        if (info.IsInvokeLocal)
            message = $"You said: {message}\n";
        else
            message = $"Some other player said: {message}\n";
        _messages.text += message;
    }
}

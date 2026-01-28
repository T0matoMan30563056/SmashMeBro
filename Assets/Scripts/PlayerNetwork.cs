using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> _netState = new (writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 _vel;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;




    void Update()
    {
        if (IsOwner)
        {
            _netState.Value = new PlayerNetworkData
            {
                Position = transform.position,
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _netState.Value.Position, ref _vel, _cheapInterpolationTime);
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y;
        private float _xScale;

        internal Vector3 Position
        {
            get => new Vector3(_x, _y, 0);
            set
            {
                _x = value.x;
                _y = value.y;
            }
        }
        /*
        internal Vector3 Scale
        {
            get => new Vector3(_xScale, 1, 1);
            set
            {
                _xScale = value.x;
            }
        }*/

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter 
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _xScale);

        }
    }

}

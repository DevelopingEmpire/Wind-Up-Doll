using Fusion;

enum Buttons
{
    forward = 0,
    back = 1,
    right = 2,
    left =3,
}

// string -> NetworkString
// bool -> NetworkBool
// float -> angle 

public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;
    public Angle yaw;
    public Angle pitch;

    
}

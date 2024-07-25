using mqttMultimeter.Common;
using mqttMultimeter.Controls.SignalGeneratorType;
using System;
using System.Linq;

namespace mqttMultimeter.Controls;

public sealed class SignalGeneratorTypeSelectorViewModel : BaseSingleSelectionViewModel
{
    public SignalGeneratorTypeSelectorViewModel() : base(6)
    {
        Value = SignalGeneratorTypeEnum.Heartbeat;
    }

    public bool IsHeartbeat
    {
        get => GetState(0);
        set => UpdateStates(0, value);
    }

    public bool IsSine
    {
        get => GetState(1);
        set => UpdateStates(1, value);
    }

    public bool IsSawtooth
    {
        get => GetState(2);
        set => UpdateStates(2, value);
    }

    public bool IsTriangle
    {
        get => GetState(3);
        set => UpdateStates(3, value);
    }

    public bool IsRandom
    {
        get => GetState(4);
        set => UpdateStates(4, value);
    }

    public bool IsWeightedRandom
    {
        get => GetState(5);
        set => UpdateStates(5, value);
    }

    public SignalGeneratorTypeEnum Value
    {
        get
        {

            if (IsHeartbeat)
            {
                return SignalGeneratorTypeEnum.Heartbeat;
            }

            if (IsSine)
            {
                return SignalGeneratorTypeEnum.Sine;
            }

            if (IsSawtooth)
            {
                return SignalGeneratorTypeEnum.Sawtooth;
            }

            if (IsTriangle)
            {
                return SignalGeneratorTypeEnum.Triangle;
            }

            if (IsRandom)
            {
                return SignalGeneratorTypeEnum.Random;
            }

            if (IsWeightedRandom)
            {
                return SignalGeneratorTypeEnum.WeightedRandom;
            }

            throw new NotSupportedException();
        }

        set
        {
            foreach (var i in Enumerable.Range(0, 5))
            {
                UpdateStates(i, value == (SignalGeneratorTypeEnum)i);
            }
        }
    }
}
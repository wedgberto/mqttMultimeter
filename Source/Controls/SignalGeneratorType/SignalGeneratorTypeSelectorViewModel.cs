using mqttMultimeter.Common;
using mqttMultimeter.Controls.SignalGeneratorType;
using System;
using System.Linq;

namespace mqttMultimeter.Controls;

public sealed class SignalGeneratorTypeSelectorViewModel : BaseSingleSelectionViewModel
{
    private const int SIZE = 8;

    public SignalGeneratorTypeSelectorViewModel() : base(SIZE)
    {
        Value = SignalGeneratorTypeEnum.None;
    }

    public bool HasInterval
    {
        get => (int)this.Value > (int)SignalGeneratorTypeEnum.None;
    }

    public bool HasRange
    {
        get => (int)this.Value > (int)SignalGeneratorTypeEnum.Heartbeat;
    }

    public bool HasWave
    {
        get => ((int)this.Value == (int)SignalGeneratorTypeEnum.Wanderer) || (int)this.Value > (int)SignalGeneratorTypeEnum.Heartbeat && (int)this.Value < (int)SignalGeneratorTypeEnum.Random;
    }

    public bool IsNone
    {
        get => GetState((int)SignalGeneratorTypeEnum.None);
        set => UpdateStates(0, value);
    }

    public bool IsHeartbeat
    {
        get => GetState((int)SignalGeneratorTypeEnum.Heartbeat);
        set => UpdateStates((int)SignalGeneratorTypeEnum.Heartbeat, value);
    }

    public bool IsSine
    {
        get => GetState((int)SignalGeneratorTypeEnum.Sine);
        set => UpdateStates((int)SignalGeneratorTypeEnum.Sine, value);
    }

    public bool IsSquare
    {
        get => GetState((int)SignalGeneratorTypeEnum.Square);
        set => UpdateStates((int)SignalGeneratorTypeEnum.Square, value);
    }

    public bool IsTriangle
    {
        get => GetState((int)SignalGeneratorTypeEnum.Triangle);
        set => UpdateStates((int)SignalGeneratorTypeEnum.Triangle, value);
    }

    public bool IsRandom
    {
        get => GetState((int)SignalGeneratorTypeEnum.Random);
        set => UpdateStates((int)SignalGeneratorTypeEnum.Random, value);
    }

    public bool IsWeightedRandom
    {
        get => GetState((int)SignalGeneratorTypeEnum.WeightedRandom);
        set => UpdateStates((int)SignalGeneratorTypeEnum.WeightedRandom, value);
    }

    public bool IsWanderer
    {
        get => GetState((int)SignalGeneratorTypeEnum.Wanderer);
        set => UpdateStates((int)SignalGeneratorTypeEnum.Wanderer, value);
    }

    public SignalGeneratorTypeEnum Value
    {
        get
        {
            if (IsNone)
            {
                return SignalGeneratorTypeEnum.None;
            }

            if (IsHeartbeat)
            {
                return SignalGeneratorTypeEnum.Heartbeat;
            }

            if (IsSine)
            {
                return SignalGeneratorTypeEnum.Sine;
            }

            if (IsSquare)
            {
                return SignalGeneratorTypeEnum.Square;
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

            if (IsWanderer)
            {
                return SignalGeneratorTypeEnum.Wanderer;
            }

            throw new NotSupportedException();
        }

        set
        {
            foreach (var i in Enumerable.Range(0, SIZE))
            {
                UpdateStates(i, value == (SignalGeneratorTypeEnum)i);
            }
        }
    }
}
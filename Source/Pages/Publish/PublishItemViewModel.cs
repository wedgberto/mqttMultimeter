using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Data;
using DynamicData.Binding;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using ReactiveUI;

namespace mqttMultimeter.Pages.Publish;

public sealed class PublishItemViewModel : BaseViewModel
{
    string? _contentType;
    TimeSpan _interval = TimeSpan.FromSeconds(1);
    uint _messageExpiryInterval;
    int _min;
    int _max;
    string _name = string.Empty;
    string _payload = string.Empty;
    TimeSpan _period = TimeSpan.FromSeconds(1.5);
    ushort _phase = 0;
    BufferFormat _payloadFormat;
    string? _responseTopic;
    bool _retain;
    uint _subscriptionIdentifier;
    string? _topic;
    ushort _topicAlias;
    bool _canStart;
    bool _canStop;
    bool _isGenerating;
    readonly Timer _timer;

    public PublishItemViewModel()
    {
        Topic = "Preview";
        Payload = "Demo Payload";
        LastSignalTimestamp = DateTimeOffset.Now.ToString("O");
        LastSignalValue = "0.0";
    }

    public PublishItemViewModel(PublishPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));

        PayloadFormatIndicator.IsUnspecified = true;
        Response.UserProperties.IsReadOnly = true;

        SignalGeneratorType.Changed.Subscribe(v =>
        {
            if (v.Sender is SignalGeneratorTypeSelectorViewModel vm)
            {
                CanStart = !vm.IsNone && !this.IsGenerating;
                CanStop = this.IsGenerating;
            }
        });

        _timer = new Timer(o =>
        {
            this.Publish();
        });
    }

    public string? ContentType
    {
        get => _contentType;
        set => this.RaiseAndSetIfChanged(ref _contentType, value);
    }

    public TimeSpan SignalGeneratorInterval
    {
        get => _interval;
        set => this.RaiseAndSetIfChanged(ref _interval, value);
    }

    public uint MessageExpiryInterval
    {
        get => _messageExpiryInterval;
        set => this.RaiseAndSetIfChanged(ref _messageExpiryInterval, value);
    }

    public bool CanStart
    {
        get => _canStart;
        private set => this.RaiseAndSetIfChanged(ref _canStart, value);
    }

    public bool CanStop
    {
        get => _canStop;
        private set => this.RaiseAndSetIfChanged(ref _canStop, value);
    }

    public bool IsGenerating
    {
        get => _isGenerating;
        private set => this.RaiseAndSetIfChanged(ref _isGenerating, value);
    }

    public int SignalGeneratorMax
    {
        get => _max;
        set
        {
            if (value < _min)
            {
                throw new DataValidationException("min value must be less than or equal max value");
            }
            this.RaiseAndSetIfChanged(ref _max, value);
        }
    }

    public int SignalGeneratorMin
    {
        get => _min;
        set
        {
            if (value > _max)
            {
                throw new DataValidationException("min value must be less than or equal max value");
            }
            this.RaiseAndSetIfChanged(ref _min, value);
        }
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public PublishPageViewModel OwnerPage { get; }

    public string Payload
    {
        get => _payload;
        set => this.RaiseAndSetIfChanged(ref _payload, value);
    }

    public BufferFormat PayloadFormat
    {
        get => _payloadFormat;
        set => this.RaiseAndSetIfChanged(ref _payloadFormat, value);
    }

    public TimeSpan SignalGeneratorPeriod
    {
        get => _period;
        set => this.RaiseAndSetIfChanged(ref _period, value);
    }

    public ushort SignalGeneratorPhase
    {
        get => _phase;
        set => this.RaiseAndSetIfChanged(ref _phase, value);
    }

    public PayloadFormatIndicatorSelectorViewModel PayloadFormatIndicator { get; } = new();

    public QualityOfServiceLevelSelectorViewModel QualityOfServiceLevel { get; } = new();

    public PublishResponseViewModel Response { get; } = new();

    public string? ResponseTopic
    {
        get => _responseTopic;
        set => this.RaiseAndSetIfChanged(ref _responseTopic, value);
    }

    private string _lastSignalValue;
    private string _lastSignalTimestamp;

    public string LastSignalValue
    {
        get => _lastSignalValue; 
        private set => this.RaiseAndSetIfChanged(ref _lastSignalValue, value);
    }

    public string LastSignalTimestamp
    {
        get => _lastSignalTimestamp;
        private set => this.RaiseAndSetIfChanged(ref _lastSignalTimestamp, value);
    }

    public bool Retain
    {
        get => _retain;
        set => this.RaiseAndSetIfChanged(ref _retain, value);
    }

    public SignalGeneratorTypeSelectorViewModel SignalGeneratorType { get; } = new();

    public uint SubscriptionIdentifier
    {
        get => _subscriptionIdentifier;
        set => this.RaiseAndSetIfChanged(ref _subscriptionIdentifier, value);
    }

    public string? Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }

    public ushort TopicAlias
    {
        get => _topicAlias;
        set => this.RaiseAndSetIfChanged(ref _topicAlias, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();

    public void StartSignalGeneration()
    {
        CanStop = !SignalGeneratorType.IsNone;
        CanStart = false;
        IsGenerating = true;
        _timer.Change(TimeSpan.Zero, this.SignalGeneratorInterval);
    }

    public void StopSignalGeneration()
    {
        CanStop = false;
        CanStart = !SignalGeneratorType.IsNone;
        IsGenerating = false;
        _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    public Task Publish()
    {
        return OwnerPage.PublishItem(this);
    }

    internal void UpdateLastSignalValue(string signalValue, string timestamp)
    {
        this.LastSignalValue = signalValue;
        this.LastSignalTimestamp = timestamp;
    }
}
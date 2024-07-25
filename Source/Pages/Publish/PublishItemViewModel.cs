using System;
using System.Threading.Tasks;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using ReactiveUI;

namespace mqttMultimeter.Pages.Publish;

public sealed class PublishItemViewModel : BaseViewModel
{
    string? _contentType;
    TimeSpan? _interval = TimeSpan.FromSeconds(1);
    uint _messageExpiryInterval;
    int _min;
    int _max;
    string _name = string.Empty;
    string _payload = string.Empty;
    TimeSpan? _period = TimeSpan.FromSeconds(1.5);
    ushort? _phase = 0;
    BufferFormat _payloadFormat;
    string? _responseTopic;
    bool _retain;
    uint _subscriptionIdentifier;
    string? _topic;
    ushort _topicAlias;

    public PublishItemViewModel()
    {
        Topic = "Preview";
        Payload = "Demo Payload";
    }

    public PublishItemViewModel(PublishPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));

        PayloadFormatIndicator.IsUnspecified = true;
        Response.UserProperties.IsReadOnly = true;
    }


    public bool EnableKnobs
    {
        get => !this.SignalGeneratorType.IsHeartbeat;
    }

    public string? ContentType
    {
        get => _contentType;
        set => this.RaiseAndSetIfChanged(ref _contentType, value);
    }

    public TimeSpan? Interval
    {
        get => _interval;
        set => this.RaiseAndSetIfChanged(ref _interval, value);
    }

    public uint MessageExpiryInterval
    {
        get => _messageExpiryInterval;
        set => this.RaiseAndSetIfChanged(ref _messageExpiryInterval, value);
    }

    public int Max
    {
        get => _max;
        set => this.RaiseAndSetIfChanged(ref _max, value);
    }

    public int Min
    {
        get => _min;
        set => this.RaiseAndSetIfChanged(ref _min, value);
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

    public TimeSpan? Period
    {
        get => _period;
        set => this.RaiseAndSetIfChanged(ref _period, value);
    }

    public ushort? Phase
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

    public Task Publish()
    {
        return OwnerPage.PublishItem(this);
    }
}
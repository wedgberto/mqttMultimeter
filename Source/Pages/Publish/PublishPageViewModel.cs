using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Inflight;
using mqttMultimeter.Pages.Publish.State;
using mqttMultimeter.Services.Mqtt;
using mqttMultimeter.Services.State;
using ReactiveUI;

namespace mqttMultimeter.Pages.Publish;

public sealed class PublishPageViewModel : BasePageViewModel
{
    readonly MqttClientService _mqttClientService;
    private bool _isConnected;

    public PublishPageViewModel()
    {
        var item = new PublishItemViewModel(this)
        {
            Name = "Preview",
            Topic = "preview/topic1",
            Payload = "{\"value\": 1.0}",
        };
        Items.Collection.Add(item);
        Items.SelectedItem = item;
    }

    public PublishPageViewModel(MqttClientService mqttClientService, StateService stateService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        if (stateService == null)
        {
            throw new ArgumentNullException(nameof(stateService));
        }

        stateService.Saving += SaveState;
        LoadState(stateService);

        mqttClientService.Connected += (_, e) =>
        {
            IsConnected = true;
        };

        mqttClientService.Disconnected += (_, e) =>
        {
            IsConnected = false;
        };
    }

    private bool _isAllStarted;

    public bool IsAllStarted
    {
        get => _isAllStarted;
        set
        {
            this.RaiseAndSetIfChanged(ref _isAllStarted, value);
            this.RaisePropertyChanged(nameof(StartAllButtonTooltip));
            if (_isAllStarted)
            {
                StartAll();
            }
            else
            {
                StopAll();
            }
        }
    }

    public void ToggleAllStarted()
    {
        IsAllStarted = !IsAllStarted;
    }

    public string StartAllButtonTooltip => $"{(_isAllStarted ? "Stop" : "Start")} all signal generators";

    public bool IsConnected
    {
        get => _isConnected;
        set => this.RaiseAndSetIfChanged(ref _isConnected, value);
    }

    public PageItemsViewModel<PublishItemViewModel> Items { get; } = new();

    public void AddItem()
    {
        var newItem = new PublishItemViewModel(this)
        {
            Name = "Untitled"
        };

        // Prepare the UI with at lest one user property.
        // It will not be send when the name is empty.
        newItem.UserProperties.AddEmptyItem();

        Items.Collection.Add(newItem);
        Items.SelectedItem = newItem;
    }

    public void CopyItem()
    {
        int numberOfDigitsAtEnd = 0;
        for (var i = this.Items.SelectedItem.Topic.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(this.Items.SelectedItem.Topic[i]))
            {
                break;
            }

            numberOfDigitsAtEnd++;
        }

        var result = this.Items.SelectedItem.Topic[^numberOfDigitsAtEnd..];
        if (int.TryParse(result, out var topicSeq))
        {
            topicSeq++;
        }
        else
        {
            topicSeq = 1;
        }

        var newItem = new PublishItemViewModel(this)
        {
            ContentType = this.Items.SelectedItem.ContentType,
            Name = this.Items.SelectedItem.Name + " (copy)",
            MessageExpiryInterval = this.Items.SelectedItem.MessageExpiryInterval,
            Payload = this.Items.SelectedItem.Payload,
            PayloadFormat = this.Items.SelectedItem.PayloadFormat,
            PayloadFormatIndicator =
            {
                Value = this.Items.SelectedItem.PayloadFormatIndicator.Value,
            },
            QualityOfServiceLevel =
            {
                Value = this.Items.SelectedItem.QualityOfServiceLevel.Value,
            },
            ResponseTopic = this.Items.SelectedItem.ResponseTopic,
            Retain = this.Items.SelectedItem.Retain,
            SignalGeneratorInterval = this.Items.SelectedItem.SignalGeneratorInterval,
            SignalGeneratorMin = this.Items.SelectedItem.SignalGeneratorMin,
            SignalGeneratorMax = this.Items.SelectedItem.SignalGeneratorMax,
            SignalGeneratorPeriod = this.Items.SelectedItem.SignalGeneratorPeriod,
            SignalGeneratorPhase = this.Items.SelectedItem.SignalGeneratorPhase,
            SignalGeneratorType =
            {
                Value = this.Items.SelectedItem.SignalGeneratorType.Value,
            },
            SubscriptionIdentifier = this.Items.SelectedItem.SubscriptionIdentifier,
            Topic = this.Items.SelectedItem.Topic.Substring(0, this.Items.SelectedItem.Topic.Length - numberOfDigitsAtEnd) + topicSeq,
            TopicAlias = this.Items.SelectedItem.TopicAlias,
        };

        foreach (var userProperty in this.Items.SelectedItem.UserProperties.Items)
        {
            newItem.UserProperties.AddItem(userProperty.Name ?? string.Empty, userProperty.Value ?? string.Empty);
        }

        // Prepare the UI with at lest one user property.
        // It will not be send when the name is empty.
        if (newItem.UserProperties.Items.Count == 0)
        {
            newItem.UserProperties.AddEmptyItem();
        }

        Items.Collection.Add(newItem);
        Items.SelectedItem = newItem;
    }

    public void StartAll()
    {
        try
        {
            foreach (var item in Items.Collection)
            {
                item.StartSignalGeneration();
            }
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }

    public void StopAll()
    {
        try
        {
            foreach (var item in Items.Collection)
            {
                item.StopSignalGeneration();
            }
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }

    public async Task PublishItem(PublishItemViewModel item)
    {
        try
        {
            await _mqttClientService.Publish(item);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }

    public void RepeatMessage(InflightPageItemViewModel inflightPageItem)
    {
        if (inflightPageItem == null)
        {
            throw new ArgumentNullException(nameof(inflightPageItem));
        }

        var publishItem = new PublishItemViewModel(this)
        {
            Name = $"Repeat '{inflightPageItem.Topic}'",
            ContentType = inflightPageItem.ContentType,
            Topic = inflightPageItem.Topic,
            Payload = Encoding.UTF8.GetString(inflightPageItem.Payload)
        };

        Items.Collection.Add(publishItem);
        Items.SelectedItem = publishItem;

        RequestActivation();
    }

    void LoadState(StateService stateService)
    {
        stateService.TryGet(PublishPageState.Key, out PublishPageState? state);
        PublishPageStateLoader.Apply(this, state);

        Items.SelectedItem = Items.Collection.FirstOrDefault();
    }

    void SaveState(object? sender, SavingStateEventArgs eventArgs)
    {
        var state = PublishPageStateFactory.Create(this);
        eventArgs.StateService.Set(PublishPageState.Key, state);
    }
}
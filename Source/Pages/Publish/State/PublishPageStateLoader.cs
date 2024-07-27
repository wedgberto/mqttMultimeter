using System;

namespace mqttMultimeter.Pages.Publish.State;

public static class PublishPageStateLoader
{
    public static void Apply(PublishPageViewModel target, PublishPageState? state)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (state == null)
        {
            // Create one empty demo item.
            target.AddItem();
            return;
        }

        foreach (var publishState in state.Publishes)
        {
            target.Items.Collection.Add(CreateItem(target, publishState));
        }
    }

    static PublishItemViewModel CreateItem(PublishPageViewModel ownerPage, PublishState publishState)
    {
        var item = new PublishItemViewModel(ownerPage)
        {
            Name = publishState.Name ?? string.Empty,
            Topic = publishState.Topic ?? string.Empty,
            Retain = publishState.Retain,
            ContentType = publishState.ContentType ?? string.Empty,
            ResponseTopic = publishState.ResponseTopic ?? string.Empty,
            SubscriptionIdentifier = publishState.SubscriptionIdentifier,
            TopicAlias = publishState.TopicAlias,
            MessageExpiryInterval = publishState.MessageExpiryInterval,
            Payload = publishState.Payload ?? string.Empty,
            PayloadFormatIndicator =
            {
                Value = publishState.PayloadFormatIndicator
            },
            QualityOfServiceLevel =
            {
                Value = publishState.QualityOfServiceLevel
            },
            SignalGeneratorType = {
                Value = publishState.SignalGeneratorType
            },
            SignalGeneratorInterval = publishState.SignalGeneratorInterval,
            SignalGeneratorPeriod = publishState.SignalGeneratorPeriod,
            SignalGeneratorPhase = publishState.SignalGeneratorPhase,
        };

        try
        {
            item.SignalGeneratorMin = publishState.SignalGeneratorMin;
            item.SignalGeneratorMax = publishState.SignalGeneratorMax;
        }
        catch { }

        foreach (var userProperty in publishState.UserProperties)
        {
            item.UserProperties.AddItem(userProperty.Name ?? string.Empty, userProperty.Value ?? string.Empty);
        }

        return item;
    }
}
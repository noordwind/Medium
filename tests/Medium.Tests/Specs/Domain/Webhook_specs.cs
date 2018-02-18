using System;
using FluentAssertions;
using Machine.Specifications;
using Medium.Domain;

namespace Medium.Tests.Specs.Domain
{
    public abstract class Webhook_specs : Domain_specs
    {
        protected static string Name = "Webhook Name";
        protected static Webhook Webhook;

        protected static void Initialize()
        {
            Webhook = new Webhook(Name);
        }
    }

    [Subject("Webhook initialize")]
    public class when_creating_new_webhook : Webhook_specs
    {
        Establish context = () => {};

        Because of = () => Initialize();

        It should_not_be_null = () => Webhook.Should().NotBeNull();
        It should_have_assigned_id = () => Webhook.Id.Should().NotBe(Guid.Empty);
        It should_have_assigned_name = () => Webhook.Name.Should().Be(Name.ToLowerInvariant());
        It should_have_assigned_endpoint = () => Webhook.Endpoint.Should().Be(Name.Replace(" ", "-").ToLowerInvariant());
        It should_be_active = () => Webhook.Inactive.Should().BeFalse();
        It should_not_have_any_actions = () => Webhook.Actions.Should().BeEmpty();
        It should_not_have_any_triggers = () => Webhook.Triggers.Should().BeEmpty();
    }

    [Subject("Webhook initialize without name")]
    public class when_creating_new_webhook_without_name : Webhook_specs
    {
        Establish context = () => Name = string.Empty;

        Because of = () => Exception = Catch.Exception(() => Initialize());

        It should_throw_argument_exception = () =>
        {
            Exception.Should().BeOfType<ArgumentException>();
        };

        It should_contain_error_message = () =>
        {
            Exception.Message.Should().StartWith("Webhook name can not be empty.");
        };
    }
}
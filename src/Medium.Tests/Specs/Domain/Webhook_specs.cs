using System;
using Machine.Specifications;
using Medium.Core.Domain;

namespace Medium.Tests.Specs.Domain
{
    public abstract class Webhook_specs : Domain_specs
    {
        protected static string Name = "Webhook 1";
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

        It should_not_be_null = () => Webhook.ShouldNotBeNull();
        It should_have_assigned_id = () => Webhook.Id.ShouldNotEqual(Guid.Empty);
        It should_have_assigned_name = () => Webhook.Name.ShouldEqual(Name);
        It should_have_assigned_code = () => Webhook.Code.ShouldEqual(Name.Trim().Replace(" ", "-").ToLowerInvariant());
        It should_be_enabled = () => Webhook.Enabled.ShouldBeTrue();
        It should_not_have_any_actions = () => Webhook.Actions.ShouldBeEmpty();
        It should_not_have_any_triggers = () => Webhook.Triggers.ShouldBeEmpty();
    }

    [Subject("Webhook initialize without name")]
    public class when_creating_new_webhook_without_name : Webhook_specs
    {
        Establish context = () => Name = string.Empty;

        Because of = () => Exception = Catch.Exception(() => Initialize());

        It should_throw_argument_exception = () =>
        {
            Exception.ShouldBeOfExactType<ArgumentException>();
        };

        It should_contain_error_message = () =>
        {
            Exception.Message.ShouldStartWith("Webhook name can not be empty.");
        };
    }
}
using BatiBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatiBot.DialogBot
{
    public class MainDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<Users> _userProfileAccessor;
        public MainDialog(UserState userState) : base(nameof(MainDialog))
        {
            
             _userProfileAccessor = userState.CreateProperty<Users>("Users");

            var waterfallSteps = new WaterfallStep[]
            {
                NameStepAsync,
                GadgetStepAsync,
                GadgetChoiceAsync,
                ConfirmAgeAsync,
                AgeStepAsync,
                ConfirmStepAsync,
                SummaryStepASync,
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt), IfEmptyFieldValidatorAsync));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), AgePromptValidatorAsync));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

            AddDialog(new CellphoneDialog(userState));
            AddDialog(new ComputerDialog(userState));

            InitialDialogId = nameof(WaterfallDialog);

        }
        private static async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions
            {
                Prompt = MessageFactory.Text("What do you want to call you?"),
                RetryPrompt = MessageFactory.Text("Please enter your name to proceed.")
            }, cancellationToken);
           
        }
        private async Task<DialogTurnResult> GadgetStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new Users(), cancellationToken);
            userProfile.Name = (string)stepContext.Result;
            
            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text($"Okay {userProfile.Name}, Please enter what gadget do you use?"),
                    RetryPrompt = MessageFactory.Text($"Not found, please select or type the correct option"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Cellphone", "Computer" }),
                }, cancellationToken); ;
            
        }
        private async Task<DialogTurnResult> GadgetChoiceAsync (WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new Users(), cancellationToken);
            userProfile.Gadgets = (stepContext.Result as FoundChoice).Value;
            var choice = (stepContext.Result as FoundChoice).Value.ToString();
            if(choice == "Computer")
            {
                return await stepContext.BeginDialogAsync(nameof(ComputerDialog), cancellationToken);
            }
            else if(choice == "Cellphone")
            {
                return await stepContext.BeginDialogAsync(nameof(CellphoneDialog), cancellationToken);
            }
            return await stepContext.ReplaceDialogAsync(nameof(MainDialog));
        }
        private async Task<DialogTurnResult> ConfirmAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new Users(), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I see that you are using a {userProfile.Gadgets}."), cancellationToken);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = MessageFactory.Text("Would you like to add your age?") }, cancellationToken);
        }
        
        private async Task<DialogTurnResult> AgeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if((bool)stepContext.Result)
            {
                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please enter your age."),
                    RetryPrompt = MessageFactory.Text("The number you enter must be greater by 1 and less than to 150.")
                };
                return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(-1, cancellationToken);
            }
        }
        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new Users(), cancellationToken);
            userProfile.Age = (int)stepContext.Result;

            var msg = userProfile.Age == -1 ? "No age Given." : $"I have your age as {userProfile.Age}.";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = MessageFactory.Text("Is this okay?") }, cancellationToken);

        }
        private async Task<DialogTurnResult> SummaryStepASync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new Users(), cancellationToken);

                var msg = $"I have gathered your profile, please wait for your details {userProfile.Name}.";
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);
                if(userProfile.ComputerName == null)
                {
                    var details = $"You are using an {userProfile.CellphoneName} {userProfile.Gadgets} brand, {userProfile.Name}.";
                    if (userProfile.Age != -1)
                    {
                        details += $" And age as {userProfile.Age}";
                    }
                }
               else if(userProfile.CellphoneName == null)
                {
                    var details = $"The gadget you are using right now are a {userProfile.ComputerName}, {userProfile.Gadgets} , {userProfile.Name}.";
                    if (userProfile.Age != -1)
                    {
                        details += $" And age as {userProfile.Age}";
                    }
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(details), cancellationToken);
                }
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Okay, your profile will not be kept."), cancellationToken);
            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        private static Task<bool> AgePromptValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(promptContext.Recognized.Succeeded && promptContext.Recognized.Value > 0 && promptContext.Recognized.Value < 150);
        }
        private static Task<bool> IfEmptyFieldValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(promptContext.Recognized.Succeeded && !string.IsNullOrWhiteSpace(promptContext.Recognized.Value));
        }
    }
}

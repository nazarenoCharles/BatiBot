using BatiBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatiBot.DialogBot
{
    public class ComputerDialog : CancelAndHelpDialog
    {

        private readonly IStatePropertyAccessor<Users> _userProfileAccessor;
        public ComputerDialog(UserState userState) : base(nameof(ComputerDialog))
        {
            _userProfileAccessor = userState.CreateProperty<Users>("Users");
            var waterfallSteps = new WaterfallStep[]
            {
                ComputerNameStepAsync,
                ConfirmFinalStepAsync
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
        }
        private static async Task<DialogTurnResult> ComputerNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var attachments = new List<Attachment>();
            var cards = MessageFactory.Attachment(attachments);
            cards.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            cards.Attachments.Add(Cards.ComputerCards.GetAsusCard().ToAttachment());
            cards.Attachments.Add(Cards.ComputerCards.GetAcerCard().ToAttachment());
            cards.Attachments.Add(Cards.ComputerCards.GetLenovoCard().ToAttachment());
            await stepContext.Context.SendActivityAsync(cards, cancellationToken);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("") }, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmFinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new Users(), cancellationToken);

            var choice = stepContext.Result.ToString();
            userProfile.ComputerName = choice;

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"You've chosen {userProfile.ComputerName}."), cancellationToken);

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        //private IList<Choice> GetChoices()
        //{
        //    var computerOptions = new List<Choice>()
        //    {
        //        new Choice() { Value = "Asus", Synonyms = new List<string>() { "asus" } },
        //        new Choice() { Value = "Acer", Synonyms = new List<string>() { "acer" } },
        //    };
        //    return computerOptions;
        //}
    }
}

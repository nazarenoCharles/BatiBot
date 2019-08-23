using BatiBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatiBot.DialogBot
{
    public class CellphoneDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<Users> _userProfileAccessor;
        public CellphoneDialog(UserState userState) : base(nameof(CellphoneDialog))
        {
            _userProfileAccessor = userState.CreateProperty<Users>("Users");
            var waterfallSteps = new WaterfallStep[]
            {
                CellphoneNameStepAsync,
                FinalStepAsync,
                
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
        }
        private static async Task<DialogTurnResult> CellphoneNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var attachments = new List<Attachment>();
            var cards = MessageFactory.Attachment(attachments);
            cards.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            cards.Attachments.Add(Cards.CellphoneCards.SamsungCard().ToAttachment());

            await stepContext.Context.SendActivityAsync(cards, cancellationToken);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("What cellphone brand do you use?") }, cancellationToken);            
        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new Users(), cancellationToken);

            var choice = (string)stepContext.Result;
            userProfile.CellphoneName = choice;

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Your Cellphone Brand is {userProfile.CellphoneName}"), cancellationToken);
            return await stepContext.EndDialogAsync(userProfile.CellphoneName, cancellationToken: cancellationToken);
        }
    }
}

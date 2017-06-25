using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace Template_Bot_Application.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private int Count { get; set; }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Bonjour je suis Francis, ton assistant mobile Franprix.Je vais gérer ta liste d'achats récurrente. Prenons 5 min pour établir tes indispensables (alimentation ; hygiène & entretien ; bébé & animaux). Tu n’auras plus à t’en soucier en magasin !");
            var reply = new Activity();
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
               
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {                    
                    new CardAction(){ Value = $"utiliser mon dernier panier en ligne", Type = ActionTypes.ImBack, Title = "utiliser mon dernier panier en ligne"},  
                    new CardAction(){ Value = $"go", Type = ActionTypes.ImBack, Title = "Go!"}  
                }
            };
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("go"))
            {
                context.Call(new ListDialog(), MessageResumeAfterFormDialogAsync);
            }
            else
            {
                List<CardAction> choiceListButton = new List<CardAction>();
                CardAction goButton = new CardAction()
                {
                    Value = $"go",
                    Type = "imBack",
                    Title = "go"
                };

                CardAction existingList = new CardAction()
                {
                    Value = $"utiliser mon dernier panier en ligne",
                    Type = "imBack",
                    Title = "utiliser mon dernier panier en ligne"
                };

                choiceListButton.Add(goButton);
                choiceListButton.Add(existingList);                
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task MessageResumeAfterFormDialogAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync("Voici votre liste");
            var list = activity.Value as List<string>;
            foreach(var s in list)
            {
                await context.PostAsync(s);
            }
        }
    }
}
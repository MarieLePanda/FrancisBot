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
            await context.PostAsync("Bonjour je suis Francis");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("liste"))
            {
                context.Call(new ListDialog(), MessageResumeAfterFormDialogAsync);
            }
            else
            {
                await context.PostAsync("Dite liste pour commencer");
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
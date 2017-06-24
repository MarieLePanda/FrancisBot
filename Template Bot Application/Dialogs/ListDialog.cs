using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace Template_Bot_Application.Dialogs
{
    [Serializable]
    public class ListDialog : IDialog<object>
    {
        List<string> liste = new List<string>();
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Je vous écoutes");
            await context.PostAsync("dite terminé quand vous avez fini");

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            
            if (activity.Text.ToLower().Contains("terminé"))
            {
                await context.PostAsync($"Ok voici votre liste");
                string resume = string.Empty;
                foreach(var s in liste)
                {
                    resume += s+"<br>";
                }
                await context.PostAsync($"{resume}");
                await context.PostAsync($"Voulez vous changer quelque chose ?");
                context.Wait(OverdAsync);
            }
            else
            {
                liste.Add(activity.Text);
                await context.PostAsync($"Ok je rajoute {activity.Text} dans votre liste");
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task OverdAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("oui"))
            {
                await context.PostAsync($"Ok dites moi ce que vous voulez supprimer");
                context.Wait(RemoveAsync);
            }
            else
            {
                activity.Value = liste;
                context.Done(activity);
            }
        }

        private async Task RemoveAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string temp = string.Empty;
            foreach (var s in liste)
            {
                if (activity.Text.ToLower().Contains(s))
                {
                    temp = s;
                }
            }
            liste.Remove(temp);
            await context.PostAsync($"{temp} supprimé, voulez vous supprimer autre chose ?");
            context.Wait(OverdAsync);
        }
    }

}
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
            await context.PostAsync("C’est parti <br>. Liste tes produits d’alimentations indispensables (sauces, condiments, céréales, etc.)");
            await context.PostAsync("dite \"terminé\" quand vous avez fini");

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
                await context.PostAsync($"Je ne vois pas de boissons (jus, soda, eau, etc.). Sélectionnes un bouton ou continue d’écrire.");
                List<CardAction> takeJuice = new List<CardAction>();
                CardAction noTakeJuice = new CardAction()
                {
                    Value = $"non",
                    Type = "imBack",
                    Title = "Pas besoin"
                };

                CardAction okTakeJuice = new CardAction()
                {
                    Value = $"oui",
                    Type = "imBack",
                    Title = "Commander cette liste"
                };

                takeJuice.Add(noTakeJuice);
                takeJuice.Add(okTakeJuice);
                context.Wait(launchCleaning);
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

            if (activity.Text.ToLower().Contains("non"))
            {
                await context.PostAsync($"Passons à l'hygiène d'entretient:");
                context.Wait(cleaningObjectListAsync);
            }
            else
            {
                activity.Value = liste;
                context.Done(activity);
            }
        }

        private async Task launchCleaning(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("non"))
            {
                await context.PostAsync($"Passons à l'hygiène d'entretient:");
                await context.PostAsync("dite \"terminé\" quand vous avez fini");
                context.Wait(cleaningObjectListAsync);
            }
            else
            {
                await context.PostAsync($"non prévu");
                activity.Value = liste;
                context.Done(activity);
            }
        }

        private async Task deleteAsync(IDialogContext context, IAwaitable<object> result)
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

    private async Task cleaningObjectListAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            
            if (activity.Text.ToLower().Contains("terminé"))
            {
                await context.PostAsync($"Ok voici votre liste de produits d'entretient");        
                string resume = string.Empty;
                foreach(var s in liste)
                {
                    resume += s+"<br>";
                }
                await context.PostAsync($"{resume}");
                await context.PostAsync($"As tu pensé au produit vaiselle ? Sélectionnes un bouton ou continue d’écrire.");
                List<CardAction> takeClean = new List<CardAction>();
                CardAction noTakeClean = new CardAction()
                {
                    Value = $"non",
                    Type = "imBack",
                    Title = "Pas besoin"
                };

                CardAction okTakeClean = new CardAction()
                {
                    Value = $"oui",
                    Type = "imBack",
                    Title = "Commander cette liste"
                };

                takeClean.Add(noTakeClean);
                takeClean.Add(okTakeClean);
                context.Wait(launchBabiesAsync);
            }
            else
            {
                liste.Add(activity.Text);
                await context.PostAsync($"Ok je rajoute {activity.Text} dans votre liste");
                context.Wait(cleaningObjectListAsync);
            }
        }

        private async Task launchBabiesAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("non"))
            {
                await context.PostAsync($"Passons aux produits bébés et animaux:");
                await context.PostAsync("dite \"terminé\" quand vous avez fini");
                context.Wait(babyListAsync);
            }
            else
            {
                await context.PostAsync($"non prévu");
                activity.Value = liste;
                context.Done(activity);
            }
        }

        private async Task babyListAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            
            if (activity.Text.ToLower().Contains("terminé"))
            {
                await context.PostAsync($"Ok voici votre liste de produits pour bébés et animaux");        
                string resume = string.Empty;
                foreach(var s in liste)
                {
                    resume += s+"<br>";
                }
                await context.PostAsync($"{resume}");
                await context.PostAsync($"Super, ta liste est constituée et sauvegardé. Ecris-moi si tu penses à de nouveaux produits ;) .");
                List<CardAction> takeClean = new List<CardAction>();
                CardAction goShop = new CardAction()
                {
                    Value = $"https://en.wikipedia.org/wiki/",
                    Type = "openUrl",
                    Title = "Mon panier"
                };

                //goShop.Add(goShop);          
                
            }
            else
            {
                liste.Add(activity.Text);
                await context.PostAsync($"Ok je rajoute {activity.Text} dans votre liste");
                context.Wait(babyListAsync);
            }
        }
    }

}
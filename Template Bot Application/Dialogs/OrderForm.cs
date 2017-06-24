using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Template_Bot_Application.Dialogs
{
    [Serializable]
    public class OrderForm
    {
        public List<string> morning;

        public static IForm<OrderForm> BuildForm()
        {
            return new FormBuilder<OrderForm>()
                    .Message("je suis prêt à noter votre liste de course")
                    .Build();
        }
    };


}